using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	/// <summary>
	/// An entity capable of storing power within itself.
	/// </summary>
	internal interface IBattery : IGridConnectable
	{
		public float GetStoredPower();
		public float GetMaxStoredPower();

		/// <summary>
		/// Returns whether the battery is willing to share its power with the grid.
		/// Some machines with internal batteries may only want the power for itself.
		/// </summary>
		/// <returns>True if the entity should have its power drawn from the grid, false otherwise.</returns>
		public bool IsProvidingPower();
	}
}
