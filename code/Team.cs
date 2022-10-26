using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{

	public enum TeamOpenness
	{
		Public,
		Password,
		FriendOnly,
		Private
	}

	public enum TeamPrivilege
	{
		Owner,
		Officer,
		Member,
	}

	public class Team
	{
		[Net]
		public static List<Team> All => new();


		public string Name = "My Team";
		public TeamOpenness Openness = TeamOpenness.Public;
		public string Password = ""; // TODO: implement password

		public List<Player> Players { get; private set; }
		public Dictionary<Player, TeamPrivilege> PlayerPrivilege { get; private set; }

		public static Team CreateTeam( Player owner, string teamName )
		{
			Host.AssertServer();

			var newTeam = new Team( owner, teamName );
			All.Add( newTeam );
			return newTeam;
		}

		public static void DisbandTeam( Team team )
		{
			Host.AssertServer();

			All.Remove( team );
		}

		private Team( Player owner, string teamName )
		{
			Players = new();
			PlayerPrivilege = new();

			Name = teamName;
			AddPlayer( owner, TeamPrivilege.Owner );
		}

		public void AddPlayer( Player player, TeamPrivilege privilege )
		{
			// We are only allowed to add owner if the team is empty (initializing)!
			if ( privilege == TeamPrivilege.Owner && Players.Count > 0 )
				throw new Exception();

			if ( PlayerPrivilege.ContainsKey( player ) )
				return;

			Players.Add( player );
			PlayerPrivilege.Add( player, privilege );
		}

		public void RemovePlayer( Player player )
		{
			if ( !PlayerPrivilege.ContainsKey( player ) )
				return;

			if ( PlayerPrivilege[player] == TeamPrivilege.Owner )
			{
				if ( Players.Count > 1 )
					// Replace owner with oldest member
					foreach ( Player ply in Players )
					{
						if ( ply != player )
						{
							// Congratulations, you are being promoted!
							PlayerPrivilege[ply] = TeamPrivilege.Owner;
							break;
						}
					}
				else
					// If owner is only member, disband
					DisbandTeam( this );
			}

			Players.Remove( player );
			PlayerPrivilege.Remove( player );
		}

		public void ChangePrivilege( Player player, TeamPrivilege privilege )
		{
			if ( !PlayerPrivilege.ContainsKey( player ) )
				return;

			// Don't allow owner to change privilege using this function
			if ( PlayerPrivilege[player] == TeamPrivilege.Owner )
				return;

			PlayerPrivilege[player] = privilege;
		}

		public bool HasPlayer( Player player )
		{
			return PlayerPrivilege.ContainsKey( player );
		}

		public Player GetOwner()
		{
			foreach ( KeyValuePair<Player, TeamPrivilege> kv in PlayerPrivilege )
			{
				if ( kv.Value == TeamPrivilege.Owner )
				{
					return kv.Key;
				}
			}
			throw new Exception();
		}

		public bool CanJoin( Player player )
		{
			switch ( Openness )
			{
				case (TeamOpenness.Public):
					return true;
				case (TeamOpenness.FriendOnly):
					return false; // TODO: how does Friend work?
				case (TeamOpenness.Password):
				case (TeamOpenness.Private):
					return false;
			}
			return false;
		}

		public List<Player> GetPlayersOfRank( TeamPrivilege teamPrivilege )
		{
			var list = new List<Player>();
			foreach ( KeyValuePair<Player, TeamPrivilege> kv in PlayerPrivilege )
			{
				if ( kv.Value == teamPrivilege )
				{
					list.Add( kv.Key );
				}
			}
			return list;
		}
	}
}
