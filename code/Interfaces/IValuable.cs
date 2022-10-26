using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	/*
	 * Entity has a net worth value.
	 * If it has an owner, it will count towards its owner's net worth.
	 */
	public interface IValuable
	{

		/// <summary>
		/// Returns the value of the entity in its current state.
		/// </summary>
		/// <returns>Value of the entity.</returns>
		public int GetWorth();

	}
}
