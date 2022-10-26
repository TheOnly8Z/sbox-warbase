using Warbase;

[Library( "wb_etool" ), HammerEntity]
[EditorModel( "models/dm_crowbar.vmdl" )]
[Title( "Entrenching Tool" ), Category( "Weapons" )]
partial class EntrenchingTool : DeathmatchWeapon
{
	public static Model WorldModel = Model.Load( "models/dm_crowbar.vmdl" );
	public override string ViewModelPath => "models/v_dm_crowbar.vmdl";

	public override float PrimaryRate => 2.0f;
	public override float SecondaryRate => 1.0f;
	public override float ReloadTime => 3.0f;
	public override AmmoType AmmoType => AmmoType.None;
	public override int ClipSize => 0;
	public override int Bucket => 0;

	public override void Spawn()
	{
		base.Spawn();

		Model = WorldModel;
		AmmoClip = 0;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack();
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		// woosh sound
		// screen shake
		PlaySound( "dm.crowbar_attack" );

		Rand.SetSeed( Time.Tick );

		var forward = Owner.EyeRotation.Forward;
		forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * 0.1f;
		forward = forward.Normal;


		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 48, 16 ) )
		{
			if ( tr.Hit )
				tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			if ( Owner is Player && tr.Entity is BuildableEntity && (tr.Entity as BuildableEntity).CheckOwner( Owner as Player ) )
			{
				var buildable = tr.Entity as BuildableEntity;
				var item = buildable.Item;
				if ( item.HasFlag( BuildableFlags.EToolBuildable ) )
				{
					buildable.ProgressBuilding( 25f );
				}
				else
				{
					// Maybe give a warning or play a sound?
				}
			}
			else
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100, 15 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}
		ViewModelEntity?.SetAnimParameter( "attack_has_hit", true );
		ViewModelEntity?.SetAnimParameter( "attack", true );
		ViewModelEntity?.SetAnimParameter( "holdtype_attack", false ? 2 : 1 );
		if ( Owner is DeathmatchPlayer player )
		{
			player.SetAnimParameter( "b_attack", true );
		}
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 5 ); // TODO this is shit
		anim.SetAnimParameter( "aim_body_weight", 1.0f );

		if ( Owner.IsValid() )
		{
			ViewModelEntity?.SetAnimParameter( "b_grounded", Owner.GroundEntity.IsValid() );
			ViewModelEntity?.SetAnimParameter( "aim_pitch", Owner.EyeRotation.Pitch() );

		}
	}
}
