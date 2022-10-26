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
		Suitable = 0,
		TooFarAway = 1,
		Obstruction = 2,
		NotOnGround = 4,
		BlockedByBeacon = 8,
		CannotStack = 16,
		TooSteep = 32,
		NeedBuilding = 64,
		NeedSnapPoint = 128,
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

		public static PlacementInfo TrySuitablePlacement( DeathmatchPlayer player, BuildableItem item, Vector3 pos, Rotation rot )
		{
			BBox box = item.Model.PhysicsBounds;

			PlacementInfo placementInfo = new();
			placementInfo.item = item;
			placementInfo.flags = PlacementFlags.Suitable;

			var raise = 8f;
			// Raise the bbox by a little so slight elevation doesn't ruin the placement
			box.Mins = box.Mins.WithZ( box.Mins.z + raise );

			// Drop the position on the ground
			var tr = Trace.Ray( pos + Vector3.Up * raise, pos + Vector3.Down * 1000 )
						.Ignore( Sweeper )
						.Run();
			pos = tr.EndPosition; // + Vector3.Up * raise;

			// Check if there are any buildings we can snap to
			var snapped = false;
			if ( item.SnapPoints.Count > 0 )
			{
				Transform ourTransform = new Transform( pos, rot );

				Vector3 delta = Vector3.Zero;
				float minDist = -1f;

				foreach ( BuildableEntity entity in player.GetBuildables() )
				{
					// Skip entities that don't have snap points
					if ( entity.Item == null || entity.Item.SnapPoints.Count == 0 )
						continue;

					var skip = false;

					foreach ( SnapPoint snap in entity.Item.SnapPoints )
					{
						foreach ( SnapPoint snap2 in item.SnapPoints )
						{
							if ( snap.Flags == snap2.Flags )
							{
								var pos1 = ourTransform.PointToWorld( snap2.Position );
								var pos2 = entity.Transform.PointToWorld( snap.Position );
								var dist = pos2.Distance( pos1 );
								if ( dist <= 16 && (minDist < 0 || dist < minDist) )
								{
									// Calculate the difference we need to move to make these two points snap
									delta = pos2 - pos1;
									minDist = dist;
								}
								else if ( dist > 500 )
								{
									// Absolutely no chance any snap points on the entity will ever snap to us
									skip = true;
									break;
								}
							}
						}
						if ( skip )
							break;
					}
				}

				if ( minDist >= 0 )
				{
					pos = pos + delta;
					snapped = true;
				}
			}

			if ( !snapped && item.HasFlag( BuildableFlags.MustSnap ) )
			{
				placementInfo.flags |= PlacementFlags.NeedSnapPoint;
			}

			// Is the trace too steep?
			if ( tr.Normal.Dot( Vector3.Up ) < 0.75f )
			{
				placementInfo.flags |= PlacementFlags.TooSteep;
			}

			// Did the trace find something we are not supposed to be building upon? (Only the world and buildables with the CanStackBuildables flag are ok)
			if ( tr.Hit && !tr.Entity.IsWorld && (!(tr.Entity is BuildableEntity) || !(tr.Entity as BuildableEntity).Item.HasFlag( BuildableFlags.CanStackBuildables )) )
			{
				placementInfo.flags |= PlacementFlags.CannotStack;
			}

			// Does the sweeper (a trigger ModelEntity) detect any colliding models?
			if ( Sweeper == null || !Sweeper.IsValid() )
			{
				Sweeper = new Sweeper();
			}
			Sweeper.SetModel( item.Model.Name );
			// Sweeper.SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
			Sweeper.SetupPhysicsFromOBB( PhysicsMotionType.Static, box.Mins - item.SizeShrink / 2, box.Maxs + item.SizeShrink / 2 );
			Sweeper.Position = pos;
			Sweeper.Rotation = rot;
			if ( !Sweeper.Check() )
			{
				placementInfo.flags |= PlacementFlags.Obstruction;
			}

			// TODO: check if building is near non-friendly beacon

			// Is the position is too far away from the player?
			if ( pos.Distance( player.Position ) > MaxPlacementDistance )
			{
				placementInfo.flags |= PlacementFlags.TooFarAway;
			}

			placementInfo.pos = pos;
			placementInfo.rot = rot;

			return placementInfo;
		}

	}
}
