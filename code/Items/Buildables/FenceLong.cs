using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class FenceLong : BuildableItem
	{
		// Descriptive
		public override string Name => "Fence (Long)";
		public override string UniqueId => "buildable.fence_long";
		public override string Description => "A long see-through barrier. Cheap and quick to make, but easily destroyed.";

		// Visuals and physics
		public override Model Model => Model.Load( "models/rust_structures/fences_walls/chainlink_fence_3x9.vmdl" );
		public override Vector3 SizeShrink => new Vector3( -60, 0, 0 );
		public override List<SnapPoint> SnapPoints => new()
		{
			new SnapPoint( SnapFlags.WallEdge, new Vector3( 0, 0, 0 ) ),
			new SnapPoint( SnapFlags.WallEdge, new Vector3( -360, 0, 0 ) ),
		};


		// Attributes
		public override int CostMoney => 50;
		public override float MaxHealth => 400f;
		public override float RequiredProgress => 100f;
		public override BuildableTier Tier => BuildableTier.Tier0;
		public override BuildableFlags Flags => BuildableFlags.EToolBuildable;
	}
}
