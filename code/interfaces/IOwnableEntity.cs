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
		// TODO: Add team support for ownership
		public void SetOwner( Player player );
		public bool CheckOwner( Player player );
	}
}
