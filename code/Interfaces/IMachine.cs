using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	/// <summary>
	/// An entity that is considered a machine.
	/// Machines consume power.
	/// </summary>
	public interface IMachine : IGridConnectable
	{
		/// <summary>
		/// Returns how much power the entity is currently requiring, in power per second.
		/// </summary>
		/// <returns></returns>
		public float GetPowerConsumption();
	}
}
