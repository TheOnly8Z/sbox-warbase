using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	/*
	 * Entity can be owned by a player or a team.
	 */
	public interface IOwnableEntity
	{
		/// <summary>
		/// Check if a team has ownership of this entity.
		/// </summary>
		/// <returns>True if a team owns this entity (even if there is also a player owner); false otherwise.</returns>
		public bool IsOwnedByTeam();
		/// <summary>
		/// Check if a player has ownership of this entity.
		/// </summary>
		/// <returns>True if a player owns this entity and not a team; false otherwise.</returns>
		public bool IsOwnedByPlayer();
		/// <summary>
		/// Check if a player or team has ownership of this entity.
		/// </summary>
		/// <returns>True if a team or player owns this entity; false otherwise.</returns>
		public bool HasOwner();
		public Player GetOwnerPlayer();
		public Team GetOwnerTeam();

		public void SetOwner( Player player );
		public void SetOwner( Team team );
		/// <summary>
		/// Checks if a player has ownership of this entity.
		/// </summary>
		/// <param name="player"></param>
		/// <returns>If a team owns this entity, returns true if and only if the player is part of the team. Otherwise, returns true if the player owner is this player.</returns>
		public bool CheckOwner( Player player );
	}
}
