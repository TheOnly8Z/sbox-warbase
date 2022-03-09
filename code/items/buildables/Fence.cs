﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class Fence : BuildableItem
	{
		// Descriptive
		public override string Name => "Fence";
		public override string UniqueId => "buildable.fence";
		public override string Description => "A see-through barrier. Cheap and quick to make, but easily destroyed.";
		public override Model Model => Model.Load( "models/rust_structures/fences_walls/chainlink_fence_3x3.vmdl" );

		// Attributes
		public override int CostMoney => 20;
		public override float MaxHealth => 150f;
		public override float RequiredProgress => 50f;
		public override BuildableTier Tier => BuildableTier.Tier0;
		public override BuildableFlags Flags => BuildableFlags.EToolBuildable;
	}
}