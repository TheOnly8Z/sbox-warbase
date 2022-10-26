using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	/// <summary>
	/// Entity is capable of connecting to an electric grid of machines, batteries and generators.
	/// </summary>
	public interface IGridConnectable
	{
		public List<IGridConnectable> GetConnectedGrid();

		/// <summary>
		/// Whether the entity is willing to connect itself to a new grid.
		/// </summary>
		/// <param name="newGrid">The grid that wants to connect.</param>
		/// <returns>True if the entity should be connected.</returns>
		public bool ShouldAddToGrid(List<IGridConnectable> newGrid);
	}
}
