using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	[Library]
	public partial class BSandbags : BuildableItem
	{
		public override string ModelPath => "models/rust_props/barricades/barricade.sandbags.vmdl_c";
		public override float MaxHealth => 250f;

		public override Dictionary<DamageFlags, float> Resistances => new()
		{
			[DamageFlags.Bullet] = 0.25f,
		};

	}

}
