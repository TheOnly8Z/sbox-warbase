using Sandbox;
using Warbase;

[Library( "wb_etool", Title = "E-Tool" )]
[Hammer.EditorModel( "weapons/rust_pistol/rust_pistol.vmdl" )]
partial class ETool : BaseDmWeapon
{ 
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";
	public override AmmoType AmmoType => AmmoType.None;
	public override float PrimaryRate => 1f;
	public override float SecondaryRate => 1.0f;
	public override int ClipSize => -1;

	public override int Bucket => 1;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		AmmoClip = 12;
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && Input.Pressed( InputButton.Attack1 );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( !TakeAmmo( 1 ) )
		{
			DryFire();
			return;
		}


		//
		// Tell the clients to play the shoot effects
		//
		ShootEffects();
		PlaySound( "rust_pistol.shoot" );

		Rand.SetSeed( Time.Tick );


		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 48, 16 ) )
		{
			if ( tr.Hit )
				tr.Surface.DoBulletImpact( tr );

			if ( !IsServer ) continue;
			if ( !tr.Entity.IsValid() ) continue;

			if (Owner is Player && tr.Entity is BuildableEntity && (tr.Entity as BuildableEntity).CheckOwner(Owner as Player) )
			{
				var buildable = tr.Entity as BuildableEntity;
				var item = buildable.Item;
				if (item.HasFlag(BuildableFlags.EToolBuildable))
				{
					buildable.ProgressBuilding( 25f );
				}
				else
				{
					// Maybe give a warning or play a sound?
				}
			} else
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100, 15 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}


		}

	}
}
