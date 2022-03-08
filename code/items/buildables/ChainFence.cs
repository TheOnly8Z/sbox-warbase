using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class ChainFence : BuildableItem
	{
		// Descriptive
		public override string Name => "Chain Fence";
		public override string UniqueId => "buildable.chainfence";
		public override string Description => "See-through barriers. Cheap and quick to make, but easily destroyed.";
		public override string ModelPath => "models/rust_structures/fences_walls/chainlink_fence_3x3.vmdl_c";

		// Attributes
		public override float MaxHealth => 100f;
		public override float RequiredProgress => 50f;
	}
}
