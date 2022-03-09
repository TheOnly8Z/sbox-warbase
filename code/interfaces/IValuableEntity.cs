using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	/*
	 * Entity has value that should be accounted for as part of its owner's net worth.
	 */
	public interface IValuableEntity
	{
		public int GetWorth();
	}
}
