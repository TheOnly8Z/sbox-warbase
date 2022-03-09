using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	/// <summary>
	/// Bit flags representing issues preventing placement. Zero means no issues exist.
	/// </summary>
	[Flags]
	public enum PlacementFlags
	{
		Suitable		= 0,
		TooFarAway		= 1,
		BlockedByObject = 2,
		NotOnGround		= 4,
		NearbyBeacon	= 8,
		CannotStack		= 16,
		TooSteep		= 32,
	}

	public struct PlacementInfo
	{
		public BuildableItem item;
		public PlacementFlags flags;
		public Vector3 pos;
		public Rotation rot;

		public bool IsSuitable() => flags == PlacementFlags.Suitable;
	}

	public static class BuildingHelper
	{
		public static float MaxPlacementDistance = 200f;
		public static BBox EmptyBBox = new BBox( Vector3.Zero );

		public static Sweeper Sweeper;

		public static PlacementInfo TrySuitablePlacement(Player player, BuildableItem item, Vector3 pos, Rotation rot)
		{

			if ( Sweeper == null || !Sweeper.IsValid() )
			{
				Sweeper = new Sweeper();
			}
			Sweeper.SetModel( item.Model.Name );
			Sweeper.SetupPhysicsFromModel( PhysicsMotionType.Dynamic );


			PlacementInfo placementInfo = new();
			placementInfo.item = item;
			placementInfo.flags = PlacementFlags.Suitable;

			BBox box = item.PlacementBBox;
			if ( box == EmptyBBox )
			{
				box = item.Model.PhysicsBounds;
			}

			var raise = 8f;
			// Raise the bbox by a little so slight elevation doesn't ruin the placement
			box.Mins = box.Mins.WithZ( box.Mins.z + raise );

			// Drop the position on the ground
			var tr = Trace.Ray( pos + Vector3.Up * raise, pos + Vector3.Down * 1000 )
						.Ignore( Sweeper )
						.Run();
			pos = tr.EndPosition; // + Vector3.Up * raise;

			// Is the trace too steep?
			if ( tr.Normal.Dot( Vector3.Up ) < 0.75f )
			{
				placementInfo.flags |= PlacementFlags.TooSteep;
			}

			// Did the trace find something we are not supposed to be building upon? (Only the world and buildables with the CanStackBuildables flag are ok)
			if ( tr.Hit && !tr.Entity.IsWorld && ( !(tr.Entity is BuildableEntity) || !(tr.Entity as BuildableEntity).Item.HasFlag(BuildableFlags.CanStackBuildables) ) )
			{
				placementInfo.flags |= PlacementFlags.CannotStack;
			}

			// Does the sweeper (a trigger ModelEntity) detect any colliding models?
			Sweeper.Position = pos;
			Sweeper.Rotation = rot;
			if (!Sweeper.Check())
			{
				placementInfo.flags |= PlacementFlags.BlockedByObject;
			}

			// TODO: check if building is near non-friendly beacon

			// Is the position is too far away from the player?
			if ( pos.Distance(player.Position) > MaxPlacementDistance )
			{
				placementInfo.flags |= PlacementFlags.TooFarAway;
			}

			placementInfo.pos = pos;
			placementInfo.rot = rot;

			return placementInfo;
		}

	}
}
