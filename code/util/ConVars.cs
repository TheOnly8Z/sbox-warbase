using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	public static class ConVars
	{
		[ServerVar( "wb_buildable_friendlyfire", Help = "If enabled, players can damage their own buildings." )]
		public static bool wb_buildable_friendlyfire { get; set; } = false;
	}
}
