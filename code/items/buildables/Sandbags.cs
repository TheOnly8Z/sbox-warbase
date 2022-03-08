using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase.Buildables
{
	[Library]
	public partial class Sandbags : BuildableItem
	{
		// Descriptive
		public override string Name => "Sandbags";
		public override string UniqueId => "buildable.sandbags";
		public override string Description => "Waist-high stacks of bags of sand and dirt. Literally dirt cheap, and effective against bullets.";
		public override string ModelPath => "models/rust_props/barricades/barricade.sandbags.vmdl_c";

		// Attributes
		public override float MaxHealth => 150f;
		public override float RequiredProgress => 100f;
		public override Dictionary<DamageFlags, float> Resistances => new()
		{
			[DamageFlags.Bullet] = 0.5f,
		};

	}

}
