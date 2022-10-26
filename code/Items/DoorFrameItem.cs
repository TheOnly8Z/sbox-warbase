using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	public partial class DoorFrameItem : BuildableItem
	{
		/// <summary>
		/// Where doors will snap to.
		/// </summary>
		public virtual Vector3 HingePosition => Vector3.Zero;
		/// <summary>
		/// Rotation of the door when it is closed.
		/// </summary>
		public virtual Rotation HingeRotation => Rotation.Identity;

	}
}
