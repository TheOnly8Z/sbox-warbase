using Sandbox;
using System;
using System.Linq;
using Warbase;

partial class WarbasePlayer : Player
{

	private static Color _previewGood = new Color( 0.5f, 1f, 0.5f, 0.75f );
	private static Color _previewBad = new Color( 1f, 0.5f, 0.5f, 0.75f );

	TimeSince timeSinceDropped;

	public bool SupressPickupNotices { get; private set; }
	public bool InBuildMode { get; set; }

	// Should be clientside only but idk if anything needs to be different here
	private AnimEntity BuildPreview;
	private float PreviewYawOffset = 0f;
	private float PreviewGridSize = 4f;

	private BuildableItem _selected;

	public WarbasePlayer()
	{
		Inventory = new DmInventory( this );
	}

	public override void Respawn()
	{
		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();

		CameraMode = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		ClearAmmo();
		Clothing.DressEntity( this );

		SupressPickupNotices = true;

		Inventory.Add( new ETool(), true );
		// Inventory.Add( new Pistol(), true );
		Inventory.Add( new Shotgun() );
		Inventory.Add( new SMG() );
		Inventory.Add( new Crossbow() );

		GiveAmmo( AmmoType.Pistol, 100 );
		GiveAmmo( AmmoType.Buckshot, 8 );
		GiveAmmo( AmmoType.Crossbow, 4 );

		SupressPickupNotices = false;
		Health = 100;

		base.Respawn();
	}
	public override void OnKilled()
	{
		base.OnKilled();

		Inventory.DropActive();
		Inventory.DeleteContents();

		BecomeRagdollOnClient( LastDamage.Force, GetHitboxBone( LastDamage.HitboxIndex ) );

		Controller = null;

		CameraMode = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach ( var child in Children.OfType<ModelEntity>() )
		{
			child.EnableDrawing = false;
		}
	}

	public void SelectBuildable(BuildableItem item)
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
		} else if ( InBuildMode )
		{
			if ( BuildPreview == null || !BuildPreview.IsValid() )
			{
				BuildPreview = new AnimEntity();
				BuildPreview.PhysicsEnabled = false;
				BuildPreview.CollisionGroup = CollisionGroup.Never;
				BuildPreview.RenderColor = _previewGood;
			}
			BuildPreview.SetModel( _selected.Model.Name );
		}
	}

	public void SetBuildMode(bool enabled)
	{
		if (enabled && !InBuildMode)
		{
			InBuildMode = true;
			ActiveChild = null;

			if ( IsClient )
			{
				SelectBuildable( _selected );
			}


		} else if (!enabled && InBuildMode)
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
		buildable = Items.Create(this, _selected);

		buildable.Position = placementInfo.pos;
		buildable.Rotation = placementInfo.rot;

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
		//if ( cl.NetworkIdent == 1 )
		//	return;

		base.Simulate( cl );

		//
		// Input requested a weapon switch
		//
		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
			SetBuildMode( false );
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
		}

		if ( InBuildMode )
		{
			// TODO: actual buildable selection screen
			if ( Input.Pressed( InputButton.Attack1 ) )
			{
				MakeBuilding();
			} else if ( Input.Pressed( InputButton.Slot1 ) )
			{
				SelectBuildable( Items.Find<BuildableItem>( "buildable.sandbags" ) );
			} else if ( Input.Pressed( InputButton.Slot2 ) )
			{
				SelectBuildable( Items.Find<BuildableItem>( "buildable.fence" ) );
			} else if ( Input.Pressed( InputButton.Slot3 ) )
			{
				SelectBuildable( Items.Find<BuildableItem>( "buildable.fencelong" ) );
			} else if ( Input.Pressed( InputButton.Slot4 ) )
			{
				SelectBuildable( Items.Find<BuildableItem>( "buildable.fencegate" ) );
			} else if ( Input.Pressed( InputButton.Slot5 ) )
			{
				SelectBuildable( Items.Find<BuildableItem>( "buildable.fencedoor" ) );
			}

			var angDiff = Input.Down( InputButton.Walk ) ? 5f : 15f;
			if ( Input.Down( InputButton.SlotNext ) || Input.MouseWheel > 0 )
			{
				PreviewYawOffset = PreviewYawOffset + angDiff;
			} else if ( Input.Down( InputButton.SlotPrev ) || Input.MouseWheel < 0 )
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
		if ( !InBuildMode && ActiveChild is BaseDmWeapon weapon && !weapon.IsUsable() && weapon.TimeSincePrimaryAttack > 0.5f && weapon.TimeSinceSecondaryAttack > 0.5f )
		{
			SwitchToBestWeapon();
		}
	}

	public void SwitchToBestWeapon()
	{
		var best = Children.Select( x => x as BaseDmWeapon )
			.Where( x => x.IsValid() && x.IsUsable() )
			.OrderByDescending( x => x.BucketWeight )
			.FirstOrDefault();

		if ( best == null ) return;

		ActiveChild = best;
		SetBuildMode( false );
	}

	public override void StartTouch( Entity other )
	{
		if ( timeSinceDropped < 1 ) return;

		base.StartTouch( other );
	}

	Rotation lastCameraRot = Rotation.Identity;

	public override void PostCameraSetup( ref CameraSetup setup )
	{
		base.PostCameraSetup( ref setup );

		if ( lastCameraRot == Rotation.Identity )
			lastCameraRot = setup.Rotation;

		var angleDiff = Rotation.Difference( lastCameraRot, setup.Rotation );
		var angleDiffDegrees = angleDiff.Angle();
		var allowance = 20.0f;

		if ( angleDiffDegrees > allowance )
		{
			// We could have a function that clamps a rotation to within x degrees of another rotation?
			lastCameraRot = Rotation.Lerp( lastCameraRot, setup.Rotation, 1.0f - (allowance / angleDiffDegrees) );
		}
		else
		{
			//lastCameraRot = Rotation.Lerp( lastCameraRot, Camera.Rotation, Time.Delta * 0.2f * angleDiffDegrees );
		}

		// uncomment for lazy cam
		//camera.Rotation = lastCameraRot;

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
		lean = lean.LerpTo( Velocity.Dot( setup.Rotation.Right ) * 0.03f, Time.Delta * 15.0f );

		var appliedLean = lean;
		appliedLean += MathF.Sin( walkBob ) * speed * 0.2f;
		setup.Rotation *= Rotation.From( 0, 0, appliedLean );

		speed = (speed - 0.7f).Clamp( 0, 1 ) * 3.0f;

		fov = fov.LerpTo( speed * 20 * MathF.Abs( forwardspeed ), Time.Delta * 2.0f );

		setup.FieldOfView += fov;

		//	var tx = new Sandbox.UI.PanelTransform();
		//	tx.AddRotation( 0, 0, lean * -0.1f );

		//	Hud.CurrentPanel.Style.Transform = tx;
		//	Hud.CurrentPanel.Style.Dirty(); 

	}

	DamageInfo LastDamage;

	public override void TakeDamage( DamageInfo info )
	{
		LastDamage = info;

		// hack - hitbox 0 is head
		// we should be able to get this from somewhere
		if ( info.HitboxIndex == 0 )
		{
			info.Damage *= 2.0f;
		}

		base.TakeDamage( info );

		if ( info.Attacker is WarbasePlayer attacker && attacker != this )
		{
			// Note - sending this only to the attacker!
			attacker.DidDamage( To.Single( attacker ), info.Position, info.Damage, Health.LerpInverse( 100, 0 ) );

			TookDamage( To.Single( this ), info.Weapon.IsValid() ? info.Weapon.Position : info.Attacker.Position );
		}
	}

	[ClientRpc]
	public void DidDamage( Vector3 pos, float amount, float healthinv )
	{
		Sound.FromScreen( "dm.ui_attacker" )
			.SetPitch( 1 + healthinv * 1 );

		HitIndicator.Current?.OnHit( pos, amount );
	}

	[ClientRpc]
	public void TookDamage( Vector3 pos )
	{
		//DebugOverlay.Sphere( pos, 5.0f, Color.Red, false, 50.0f );

		DamageIndicator.Current?.OnHit( pos );
	}
}
