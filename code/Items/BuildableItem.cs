using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		EToolBuildable = 1,
		/// <summary>
		/// If set, other buildables are allowed to be built on top of this buildable.
		/// </summary>
		CanStackBuildables = 2,
		/// <summary>
		/// If set, buildable cannot be placed unless it is snapping to an existing structure
		/// </summary>
		MustSnap = 4,
	}

	/// <summary>
	/// Flags used by SnapPoint structs to denote what kind of points can snap to it.
	/// </summary>
	[Flags]
	public enum SnapFlags
	{
		None = 0,
		/// <summary>
		/// Edges of a wall at its feet, used by walls to snap to one another
		/// </summary>
		WallEdge = 1,
		/// <summary>
		/// Center of a wall, used by foundations to snap to walls
		/// </summary>
		WallCenter = 2,
		/// <summary>
		/// Swivel point of a door, used by door frames and doors
		/// </summary>
		DoorHinge = 4,
		/// <summary>
		/// Edges of a square machine, used to align machines to each other
		/// </summary>
		MachineEdge = 8,
	}

	public readonly struct SnapPoint
	{
		public SnapPoint( SnapFlags flags, Vector3 pos )
		{
			Flags = flags;
			Position = pos;
			SlideVector = Vector3.Zero;
		}

		public SnapPoint( SnapFlags flags, Vector3 pos, Vector3 slide)
		{
			Flags = flags;
			Position = pos;
			SlideVector = slide;
		}

		public SnapFlags Flags { get; }
		public Vector3 Position { get; }
		public Vector3 SlideVector { get; }
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
		/// When checking building placement, shrink the bounding box of the model by this much (should be negative). 
		/// </summary>
		public virtual Vector3 SizeShrink => Vector3.Zero;
		/// <summary>
		/// A list of snapping points the building has.
		/// </summary>
		public virtual List<SnapPoint> SnapPoints => new();

		// Functions
		public bool HasFlag( BuildableFlags flags ) => Flags.HasFlag( flags );
	}
}
