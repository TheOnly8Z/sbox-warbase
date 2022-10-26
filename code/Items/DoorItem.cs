using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	public partial class DoorItem : BuildableItem
	{
		/// <summary>
		/// Where gates will snap to.
		/// </summary>
		public virtual Vector3 HingePosition => Vector3.Zero;
	}
}
