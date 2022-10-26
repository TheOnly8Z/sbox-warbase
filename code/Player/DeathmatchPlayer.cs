using Warbase;
public partial class DeathmatchPlayer : Player
{
	TimeSince timeSinceDropped;

	[Net]
	public float Armour { get; set; } = 0;

	[Net]
	public float MaxHealth { get; set; } = 100;

	public bool SupressPickupNotices { get; private set; }
	public bool InBuildMode { get; set; }

	public int ComboKillCount { get; set; } = 0;
	public TimeSince TimeSinceLastKill { get; set; }


	// Should be clientside only but idk if anything needs to be different here
	private AnimatedEntity BuildPreview;
	private float PreviewYawOffset = 0f;
	private float PreviewGridSize = 4f;

	private BuildableItem _selected;

	private static Color _previewGood = new Color( 0.5f, 1f, 0.5f, 0.75f );
	private static Color _previewBad = new Color( 1f, 0.5f, 0.5f, 0.75f );

	public DeathmatchPlayer()
	{
		Inventory = new DmInventory( this );
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController
		{
			WalkSpeed = 125,
			SprintSpeed = 270,
			DefaultSpeed = 150,
			AirAcceleration = 10,

		};

		Animator = new StandardPlayerAnimator();

		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		ClearAmmo();
		Clothing.DressEntity( this );

		SupressPickupNotices = true;

		Inventory.DeleteContents();
		Inventory.Add( new EntrenchingTool() );

		GiveAmmo( AmmoType.Pistol, 1000 );
		GiveAmmo( AmmoType.Python, 1000 );
		GiveAmmo( AmmoType.Buckshot, 1000 );
		GiveAmmo( AmmoType.Crossbow, 1000 );
		GiveAmmo( AmmoType.Grenade, 1000 );
		GiveAmmo( AmmoType.Tripmine, 1000 );

		Inventory.Add( new Python() );
		Inventory.Add( new Shotgun() );
		Inventory.Add( new SMG() );
		Inventory.Add( new Crossbow() );
		Inventory.Add( new GrenadeWeapon() );
		Inventory.Add( new TripmineWeapon() );

		SupressPickupNotices = false;
		Health = 100;
		Armour = 0;

		base.Respawn();
	}

	[ConCmd.Admin]
	public static void GiveAll()
	{
		var ply = ConsoleSystem.Caller.Pawn as DeathmatchPlayer;

		ply.GiveAmmo( AmmoType.Pistol, 1000 );
		ply.GiveAmmo( AmmoType.Python, 1000 );
		ply.GiveAmmo( AmmoType.Buckshot, 1000 );
		ply.GiveAmmo( AmmoType.Crossbow, 1000 );
		ply.GiveAmmo( AmmoType.Grenade, 1000 );
		ply.GiveAmmo( AmmoType.Tripmine, 1000 );

		ply.Inventory.Add( new Python() );
		ply.Inventory.Add( new Shotgun() );
		ply.Inventory.Add( new SMG() );
		ply.Inventory.Add( new Crossbow() );
		ply.Inventory.Add( new GrenadeWeapon() );
		ply.Inventory.Add( new TripmineWeapon() );
	}

	public override void OnKilled()
	{
		base.OnKilled();

		var coffin = new Coffin();
		coffin.Position = Position + Vector3.Up * 30;
		coffin.Rotation = Rotation;
		coffin.PhysicsBody.Velocity = Velocity + Rotation.Forward * 100;

		coffin.Populate( this );

		Inventory.DeleteContents();

		if ( LastDamage.Flags.HasFlag( DamageFlags.Blast ) )
		{
			using ( Prediction.Off() )
			{
				var particles = Particles.Create( "particles/gib.vpcf" );
				if ( particles != null )
				{
					particles.SetPosition( 0, Position + Vector3.Up * 40 );
				}
			}
		}
		else
		{
			BecomeRagdollOnClient( LastDamage.Force, LastDamage.BoneIndex );
		}

		Controller = null;

		CameraMode = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach ( var child in Children.OfType<ModelEntity>() )
		{
			child.EnableDrawing = false;
		}
	}

	public override void BuildInput( InputBuilder input )
	{
		if ( DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd )
		{
			input.ViewAngles = input.OriginalViewAngles;
			return;
		};

		base.BuildInput( input );
	}


	public void SelectBuildable( BuildableItem item )
	{

		_selected = item;

		if ( !IsClient )
			return;

		if ( item == null )
		{
			if ( BuildPreview != null && BuildPreview.IsValid() )
			{
				BuildPreview.Delete();
				BuildPreview = null;
			}
		}
		else if ( InBuildMode )
		{
			if ( BuildPreview == null || !BuildPreview.IsValid() )
			{
				BuildPreview = new AnimatedEntity( _selected.Model.Name );
				BuildPreview.PhysicsEnabled = false;
				BuildPreview.CollisionGroup = CollisionGroup.Never;
				BuildPreview.RenderColor = _previewGood;
			}
			BuildPreview.SetModel( _selected.Model.Name );
		}
	}

	public void SetBuildMode( bool enabled )
	{
		if ( enabled && !InBuildMode )
		{
			InBuildMode = true;
			ActiveChild = null;

			if ( IsClient )
			{
				SelectBuildable( _selected );
			}


		}
		else if ( !enabled && InBuildMode )
		{
			InBuildMode = false;
			if ( IsClient && BuildPreview != null && BuildPreview.IsValid() )
			{
				BuildPreview.Delete();
				BuildPreview = null;
			}
			SwitchToBestWeapon();
		}
	}

	public void MakeBuilding()
	{
		if ( !IsServer || !InBuildMode || _selected == null ) return;

		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * BuildingHelper.MaxPlacementDistance )
			.Ignore( this )
			.Run();
		PlacementInfo placementInfo = BuildingHelper.TrySuitablePlacement( this, _selected, tr.EndPosition, Rotation.FromYaw( PreviewYawOffset ) );

		// Bamboozled by the client!
		if ( !placementInfo.IsSuitable() ) return;

		BuildableEntity buildable;
		buildable = ItemLibrary.Create( this, _selected );

		buildable.Position = placementInfo.pos;
		buildable.Rotation = placementInfo.rot;

		buildable.SetOwner( this );

		/*
		var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 500 )
					.Ignore( this )
					.Ignore( BuildPreview )
					.Run();
		var pos = tr.EndPosition; // .SnapToGrid( PreviewGridSize );
		var tr2 = Trace.Ray( pos + Vector3.Up * 4f, pos + Vector3.Down * 100f )
					.Ignore( this )
					.Ignore( BuildPreview )
					.Run();
		buildable.Position = tr2.EndPosition;
		buildable.Rotation = Rotation.FromYaw( PreviewYawOffset );
		*/
	}
	public override void Simulate( Client cl )
	{
		if ( DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd )
			return;

		base.Simulate( cl );

		//
		// Input requested a weapon switch
		//
		if ( Input.ActiveChild != null && !InBuildMode )
		{
			ActiveChild = Input.ActiveChild;
		}

		if ( LifeState != LifeState.Alive )
			return;

		TickPlayerUse();

		if ( Input.Pressed( InputButton.View ) )
		{
			if ( CameraMode is ThirdPersonCamera )
			{
				CameraMode = new FirstPersonCamera();
			}
			else
			{
				CameraMode = new ThirdPersonCamera();
			}
		}

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				if ( dropped.PhysicsGroup != null )
				{
					dropped.PhysicsGroup.Velocity = Velocity + (EyeRotation.Forward + EyeRotation.Up) * 300;
				}

				timeSinceDropped = 0;
				SwitchToBestWeapon();
			}
		}


		if ( Input.Pressed( InputButton.Grenade ) )
		{
			SetBuildMode( !InBuildMode );

			Log.Info( ItemLibrary.Table == null );

			foreach ( var i in ItemLibrary.Table )
			{
				Log.Info(i);
			}
		}

		if ( InBuildMode )
		{
			// TODO: actual buildable selection screen
			if ( Input.Pressed( InputButton.PrimaryAttack ) )
			{
				MakeBuilding();
			}
			else if ( Input.Pressed( InputButton.Slot1 ) )
			{
				SelectBuildable( ItemLibrary.Find<BuildableItem>( "buildable.sandbags" ) );
			}
			else if ( Input.Pressed( InputButton.Slot2 ) )
			{
				SelectBuildable( ItemLibrary.Find<BuildableItem>( "buildable.fence" ) );
			}
			else if ( Input.Pressed( InputButton.Slot3 ) )
			{
				SelectBuildable( ItemLibrary.Find<BuildableItem>( "buildable.fence_long" ) );
			}
			else if ( Input.Pressed( InputButton.Slot4 ) )
			{
				SelectBuildable( ItemLibrary.Find<BuildableItem>( "buildable.fence_gate" ) );
			}
			else if ( Input.Pressed( InputButton.Slot5 ) )
			{
				SelectBuildable( ItemLibrary.Find<BuildableItem>( "buildable.fence_door" ) );
			}
			else if ( Input.Pressed( InputButton.Slot6 ) )
			{
				SelectBuildable( ItemLibrary.Find<BuildableItem>( "buildable.bastion" ) );
			}

			var angDiff = Input.Down( InputButton.Walk ) ? 5f : 15f;
			if ( Input.Down( InputButton.SlotNext ) || Input.MouseWheel > 0 )
			{
				PreviewYawOffset = PreviewYawOffset + angDiff;
			}
			else if ( Input.Down( InputButton.SlotPrev ) || Input.MouseWheel < 0 )
			{
				PreviewYawOffset = PreviewYawOffset - angDiff;
			}

			if ( IsClient && _selected != null && BuildPreview != null && BuildPreview.IsValid() )
			{
				var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * BuildingHelper.MaxPlacementDistance )
							.Ignore( this )
							.Ignore( BuildPreview )
							.Run();
				PlacementInfo placementInfo = BuildingHelper.TrySuitablePlacement( this, _selected, tr.EndPosition, Rotation.FromYaw( PreviewYawOffset ) );

				BuildPreview.Position = placementInfo.pos;
				BuildPreview.Rotation = placementInfo.rot;

				BuildPreview.RenderColor = placementInfo.IsSuitable() ? _previewGood : _previewBad;

				/*
				var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * 500 )
							.Ignore( this )
							.Ignore( BuildPreview )
							.Run();
				var pos = tr.EndPosition; // .SnapToGrid( PreviewGridSize );
				var tr2 = Trace.Ray( pos + Vector3.Up * 4f, pos + Vector3.Down * 100f )
							.Ignore( this )
							.Ignore( BuildPreview )
							.Run();
				BuildPreview.Position = tr2.EndPosition;
				BuildPreview.Rotation = Rotation.FromYaw( PreviewYawOffset );
				*/
				// TODO check if location valid
			}

		}

		SimulateActiveChild( cl, ActiveChild );

		//
		// If the current weapon is out of ammo and we last fired it over half a second ago
		// lets try to switch to a better wepaon
		//
		if ( ActiveChild is DeathmatchWeapon weapon && !weapon.IsUsable() && weapon.TimeSincePrimaryAttack > 0.5f && weapon.TimeSinceSecondaryAttack > 0.5f )
		{
			SwitchToBestWeapon();
		}
	}

	public void SwitchToBestWeapon()
	{
		var best = Children.Select( x => x as DeathmatchWeapon )
			.Where( x => x.IsValid() && x.IsUsable() )
			.OrderByDescending( x => x.BucketWeight )
			.FirstOrDefault();

		if ( best == null ) return;

		ActiveChild = best;
	}

	public override void StartTouch( Entity other )
	{
		if ( timeSinceDropped < 1 ) return;

		base.StartTouch( other );
	}

	public override void PostCameraSetup( ref CameraSetup setup )
	{
		setup.ZNear = 0.1f;

		if ( DeathmatchGame.CurrentState == DeathmatchGame.GameStates.GameEnd )
			return;

		base.PostCameraSetup( ref setup );

		if ( setup.Viewer != null )
		{
			AddCameraEffects( ref setup );
		}
	}

	float walkBob = 0;
	float lean = 0;
	float fov = 0;

	private void AddCameraEffects( ref CameraSetup setup )
	{
		var speed = Velocity.Length.LerpInverse( 0, 320 );
		var forwardspeed = Velocity.Normal.Dot( setup.Rotation.Forward );

		var left = setup.Rotation.Left;
		var up = setup.Rotation.Up;

		if ( GroundEntity != null )
		{
			walkBob += Time.Delta * 25.0f * speed;
		}

		setup.Position += up * MathF.Sin( walkBob ) * speed * 2;
		setup.Position += left * MathF.Sin( walkBob * 0.6f ) * speed * 1;

		// Camera lean
		lean = lean.LerpTo( Velocity.Dot( setup.Rotation.Right ) * 0.01f, Time.Delta * 15.0f );

		var appliedLean = lean;
		appliedLean += MathF.Sin( walkBob ) * speed * 0.3f;
		setup.Rotation *= Rotation.From( 0, 0, appliedLean );

		speed = (speed - 0.7f).Clamp( 0, 1 ) * 3.0f;

		fov = fov.LerpTo( speed * 20 * MathF.Abs( forwardspeed ), Time.Delta * 4.0f );

		setup.FieldOfView += fov;

	}

	DamageInfo LastDamage;

	public override void TakeDamage( DamageInfo info )
	{
		if ( LifeState == LifeState.Dead )
			return;

		LastDamage = info;

		if ( info.Hitbox.HasTag( "head" ) )
		{
			info.Damage *= 2.0f;
		}

		this.ProceduralHitReaction( info );

		LastAttacker = info.Attacker;
		LastAttackerWeapon = info.Weapon;

		if ( IsServer && Armour > 0 )
		{
			Armour -= info.Damage;

			if ( Armour < 0 )
			{
				info.Damage = Armour * -1;
				Armour = 0;
			}
			else
			{
				info.Damage = 0;
			}
		}

		if ( info.Flags.HasFlag( DamageFlags.Blast ) )
		{
			Deafen( To.Single( Client ), info.Damage.LerpInverse( 0, 60 ) );
		}

		if ( Health > 0 && info.Damage > 0 )
		{
			Health -= info.Damage;
			if ( Health <= 0 )
			{
				Health = 0;
				OnKilled();
			}
		}

		if ( info.Attacker is DeathmatchPlayer attacker )
		{
			if ( attacker != this )
			{
				attacker.DidDamage( To.Single( attacker ), info.Position, info.Damage, Health.LerpInverse( 100, 0 ) );
			}

			TookDamage( To.Single( this ), info.Weapon.IsValid() ? info.Weapon.Position : info.Attacker.Position );
		}

		//
		// Add a score to the killer
		//
		if ( LifeState == LifeState.Dead && info.Attacker != null )
		{
			if ( info.Attacker.Client != null && info.Attacker != this )
			{
				info.Attacker.Client.AddInt( "kills" );
			}
		}
	}

	[ClientRpc]
	public void DidDamage( Vector3 pos, float amount, float healthinv )
	{
		Sound.FromScreen( "dm.ui_attacker" )
			.SetPitch( 1 + healthinv * 1 );

		HitIndicator.Current?.OnHit( pos, amount );
	}

	public TimeSince TimeSinceDamage = 1.0f;

	[ClientRpc]
	public void TookDamage( Vector3 pos )
	{
		//DebugOverlay.Sphere( pos, 10.0f, Color.Red, true, 10.0f );

		TimeSinceDamage = 0;
		DamageIndicator.Current?.OnHit( pos );
	}

	[ClientRpc]
	public void PlaySoundFromScreen( string sound )
	{
		Sound.FromScreen( sound );
	}

	[ConCmd.Client]
	public static void InflictDamage()
	{
		if ( Local.Pawn is DeathmatchPlayer ply )
		{
			ply.TookDamage( ply.Position + ply.EyeRotation.Forward * 100.0f );
		}
	}

	TimeSince timeSinceLastFootstep = 0;

	public override void OnAnimEventFootstep( Vector3 pos, int foot, float volume )
	{
		if ( LifeState != LifeState.Alive )
			return;

		if ( !IsServer )
			return;

		if ( timeSinceLastFootstep < 0.2f )
			return;

		volume *= FootstepVolume();

		timeSinceLastFootstep = 0;

		//DebugOverlay.Box( 1, pos, -1, 1, Color.Red );
		//DebugOverlay.Text( pos, $"{volume}", Color.White, 5 );

		var tr = Trace.Ray( pos, pos + Vector3.Down * 20 )
			.Radius( 1 )
			.Ignore( this )
			.Run();

		if ( !tr.Hit ) return;

		tr.Surface.DoFootstep( this, tr, foot, volume * 10 );
	}

	[ConCmd.Admin]
	public static void MapVote()
	{
		var vote = new MapVoteEntity();
	}

	public void RenderHud( Vector2 screenSize )
	{
		if ( LifeState != LifeState.Alive )
			return;

		// RenderOverlayTest( screenSize );

		if ( ActiveChild is DeathmatchWeapon weapon )
		{
			weapon.RenderHud( screenSize );
		}
	}

	void RenderOverlayTest( Vector2 screenSize )
	{
		foreach ( var ent in Entity.FindInSphere( Position, 1500 ) )
		{
			var pos = ent.Position.ToScreen( screenSize );
			if ( !pos.HasValue ) continue;

			var str = $"{ent}";
			Render.Draw2D.FontFamily = "Poppins";
			Render.Draw2D.FontWeight = 1000;
			Render.Draw2D.FontSize = 14;

			var textRect = Render.Draw2D.TextSize( pos.Value, str );

			Render.Draw2D.BlendMode = BlendMode.Normal;
			Render.Draw2D.Color = Color.Black.WithAlpha( 0.7f );
			Render.Draw2D.BoxWithBorder( textRect.Expand( 16, 12 ), 2.0f, Color.Black.WithAlpha( 0.2f ), new Vector4( 4.0f ) );

			Render.Draw2D.Color = Color.White;
			Render.Draw2D.Text( pos.Value, str );
		}
	}

	public IEnumerable<BuildableEntity> GetBuildables()
	{
		return All.OfType<BuildableEntity>().Where( i => i.CheckOwner( this ) );
	}
}
