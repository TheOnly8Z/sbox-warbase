using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class FenceDoor : BuildableItem
	{
		// Descriptive
		public override string Name => "Fence Door";
		public override string UniqueId => "buildable.fence_door";
		public override string Description => "A see-through door. Cheap and quick to make, but easily destroyed.";

		// Visuals and physics
		public override Model Model => Model.Load( "models/rust_structures/fences_walls/chainlink_fence_3x3_gate_door.vmdl" );
		public override List<SnapPoint> SnapPoints => new()
		{
			new SnapPoint( SnapFlags.DoorHinge, new Vector3( 0, 16, 0 ) ),
		};

		// Attributes
		public override int CostMoney => 10;
		public override float MaxHealth => 150f;
		public override float RequiredProgress => 50f;
		public override BuildableTier Tier => BuildableTier.Tier0;
		public override BuildableFlags Flags => BuildableFlags.EToolBuildable | BuildableFlags.MustSnap;
	}
}
