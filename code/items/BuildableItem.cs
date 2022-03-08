using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	public partial class BuildableItem : BaseItem
	{
		public virtual string ModelPath => "models/rust_structures/fences_walls/chainlink_fence_3x3.vmdl_c";
		public virtual float MaxHealth => 1f;
		public virtual float RequiredProgress => 100f;
		public virtual MoveType MoveType => MoveType.None;
		public virtual CollisionGroup CollisionGroup => CollisionGroup.Always;
		public virtual Dictionary<DamageFlags, float> Resistances => new();
	}
}
