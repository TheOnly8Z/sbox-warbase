using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

public enum BuildableState
{
	Blueprint,	// Placed down, no building progress
	Building,	// Placed down, partial building progress
	Built,		// Construction is complete; buildable should be functional
	Destroyed,	// Buildable has no health and is a husk; it should be nonfunctional
}

namespace Warbase
{
	public partial class BuildableEntity<T> : AnimEntity where T : BuildableItem
	{

		private static Color _colorBlueprint = new Color( 1, 1, 1, 0.3f );

		public BuildableState BuildableState;
		public float Progress; // max is Item.RequiredProgress

		[Net, Change] public uint ItemNetworkId { get; private set; }

		private T _itemCache;
		public T Item
		{
			get
			{
				if ( _itemCache == null )
					_itemCache = Items.Find<T>( ItemNetworkId );
				return _itemCache;
			}
		}

		public BuildableEntity()
		{
			SetModel( Item.ModelPath );

			Health = 1f;

			BuildableState = BuildableState.Blueprint;
			Progress = 0f;
			RenderColor = _colorBlueprint;

			MoveType = MoveType.None;
		}

		public void ProgressBuilding(float progressPower)
		{
			Progress += progressPower;

			Health = MathF.Min( Health + (progressPower / Item.RequiredProgress) * Item.MaxHealth, Item.MaxHealth);

			if (Progress >= Item.RequiredProgress)
            {
				BuildableState = BuildableState.Built;
				RenderColor = Color.White;
				// TODO: Do stuff on construction complete

			} else if (Progress > 0f)
			{
				if ( BuildableState == BuildableState.Blueprint )
					BuildableState = BuildableState.Building;
				
				// Slowly turn fully opaque as we are building
				// TODO: Maybe better effect?
				RenderColor = Color.White.WithAlpha( _colorBlueprint.a + (1 - _colorBlueprint.a) * (Progress / Item.RequiredProgress) );
			}

		}

		public float GetResistance(DamageFlags damageFlags)
		{
			float resistance = 1f;

			foreach ( KeyValuePair<DamageFlags, float> resist in Item.Resistances )
			{
				if ( damageFlags.HasFlag( resist.Key ) )
				{
					resistance *= resist.Value;
				}
			}

			return resistance;
		}

		public override void TakeDamage( DamageInfo info )
		{
			if ( info.Attacker == Owner ) return;

			// TODO: block damage if same team


			info.Damage *= GetResistance( info.Flags );
			base.TakeDamage( info );

			// TODO: on destroyed
		}
	}
}
