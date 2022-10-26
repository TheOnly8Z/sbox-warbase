using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class Bastion : BuildableItem
	{
		// Descriptive
		public override string Name => "Bastion";
		public override string UniqueId => "buildable.bastion";
		public override string Description => "Stackable block made of rock and sand. Slow to build but durable.";

		// Visuals and physics
		public override Model Model => Model.Load( "models/rust_props/barricades/barricade.stone.vmdl" );
		public override Vector3 SizeShrink => new Vector3( -8, -8, -8 );
		public override List<SnapPoint> SnapPoints => new()
		{
			new SnapPoint( SnapFlags.WallEdge, new Vector3( 0, 24, 0 ) ),
			new SnapPoint( SnapFlags.WallEdge, new Vector3( 0, -24, 0 ) ),
			new SnapPoint( SnapFlags.WallEdge, new Vector3( 24, 0, 0 ) ),
			new SnapPoint( SnapFlags.WallEdge, new Vector3( -24, 0, 0 ) ),
			new SnapPoint( SnapFlags.WallCenter, new Vector3( 0, 0, 48 ) ),
			new SnapPoint( SnapFlags.WallCenter, new Vector3( 0, 0, 0 ) ),
		};

		// Attributes
		public override int CostMoney => 200;
		public override float MaxHealth => 200f;
		public override float RequiredProgress => 200f;
		public override Dictionary<DamageFlags, float> Resistances => new()
		{
			[DamageFlags.Bullet] = 0.25f,
		};
		public override BuildableTier Tier => BuildableTier.Tier1;
		public override BuildableFlags Flags => BuildableFlags.CanStackBuildables | BuildableFlags.EToolBuildable;
		public override Dictionary<SupplyType, uint> CostSupply => new()

		{
			[SupplyType.Basic] = 1,
		};
	}

}
