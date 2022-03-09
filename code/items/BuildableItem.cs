using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{

	public enum BuildableTier
	{
		Tier0,
		Tier1,
		Tier2,
		Tier3,
		Special,
	}

	[Flags]
	public enum BuildableFlags
	{
		None = 0,
		/// <summary>
		/// If set, the building can be built with the E-Tool (handheld weapon).
		/// </summary>
		EToolBuildable		= 1,
		/// <summary>
		/// If set, other buildables are allowed to be built on top of this buildable.
		/// </summary>
		CanStackBuildables	= 2,
	}

	public partial class BuildableItem : BaseItem
	{

		// Cost and attributes
		/// <summary>
		/// Maximum health of the building.
		/// </summary>
		public virtual float MaxHealth => 1f;
		/// <summary>
		/// The amount of "construction progress" required to finish building.
		/// </summary>
		public virtual float RequiredProgress => 100f;
		/// <summary>
		/// The tier of the building. Used by construction buildings to determine what they can build.
		/// </summary>
		public virtual BuildableTier Tier => BuildableTier.Tier0;
		/// <summary>
		/// Flags this building holds - this usually modifies behavior somewhere else.
		/// </summary>
		public virtual BuildableFlags Flags => BuildableFlags.None;
		/// <summary>
		/// Damage multipliers of this buildable.
		/// </summary>
		public virtual Dictionary<DamageFlags, float> Resistances => new();
		/// <summary>
		/// The type and amount of supplies required when buying the item.
		/// </summary>
		public virtual Dictionary<SupplyType, uint> CostSupply => new();

		// Visuals and physics
		public virtual Model Model => Model.Load( "models/rust_structures/fences_walls/chainlink_fence_3x3.vmdl" );
		public virtual int Skin => 0;
		public virtual CollisionGroup CollisionGroup => CollisionGroup.Always;
		/// <summary>
		/// The bounding box used to determine building suitability. If value is equal to BuildingHelper.EmptyBBox, uses model bounding box.
		/// For some models like sandbags, making the model clip a little may allow for better looking builds.
		/// </summary>
		public virtual BBox PlacementBBox => BuildingHelper.EmptyBBox;

		// Functions
		public bool HasFlag( BuildableFlags flags ) => Flags.HasFlag( flags );
	}
}
