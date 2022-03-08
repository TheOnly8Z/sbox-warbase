using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	internal interface IOwnable
	{
		public void SetOwner( Player player );
		public bool CheckOwner(Player player);
	}
}
