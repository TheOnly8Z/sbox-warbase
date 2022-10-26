using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class Barrier : BuildableItem
	{
		// Descriptive
		public override string Name => "Barrier";
		public override string UniqueId => "buildable.barrier";
		public override string Description => "Waist-high concrete barrier. Durable and bullet resistant.";

		// Visuals and physics
		public override Model Model => Model.Load( "models/sbox_props/concrete_barrier/concrete_barrier.vmdl" );
		public override Vector3 SizeShrink => new Vector3(0, -40, 0);
		public override List<SnapPoint> SnapPoints => new()
		{
			new SnapPoint( SnapFlags.WallEdge, new Vector3( 0, 48, 0 ) ) ,
			new SnapPoint( SnapFlags.WallEdge, new Vector3( 0, -48, 0 ) ),
		};

		// Attributes
		public override int CostMoney => 1000;
		public override float MaxHealth => 400f;
		public override float RequiredProgress => 100f;
		public override Dictionary<DamageFlags, float> Resistances => new()
		{
			[DamageFlags.Bullet] = 0.25f,
		};
		public override BuildableTier Tier => BuildableTier.Tier2;
		public override Dictionary<SupplyType, uint> CostSupply => new()

		{
			[SupplyType.Advanced] = 2,
		};
	}

}
