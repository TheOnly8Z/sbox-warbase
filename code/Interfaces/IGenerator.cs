using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	/// <summary>
	/// Entity is capable of producing power for an electic grid.
	/// </summary>
	public interface IGenerator : IGridConnectable
	{
		/// <summary>
		/// Get the amount of power the generator is currently making, in power per second.
		/// </summary>
		/// <returns>The amount of power.</returns>
		public float GetGeneratedPower();
	}
}
