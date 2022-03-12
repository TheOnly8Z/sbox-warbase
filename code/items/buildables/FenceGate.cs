using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class FenceGate : DoorFrameItem
	{
		// Descriptive
		public override string Name => "Fence (Gate)";
		public override string UniqueId => "buildable.fence_gate";
		public override string Description => "A see-through barrier with a door hole. Cheap and quick to make, but easily destroyed.";

		// Visuals and physics
		public override Model Model => Model.Load( "models/rust_structures/fences_walls/chainlink_fence_3x3_gate.vmdl" );
	
		// Attributes
		public override int CostMoney => 15;
		public override float MaxHealth => 150f;
		public override float RequiredProgress => 50f;
		public override BuildableTier Tier => BuildableTier.Tier0;
		public override BuildableFlags Flags => BuildableFlags.EToolBuildable;
	}
}
