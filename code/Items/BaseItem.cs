using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{

	/// <summary>
	/// Enum representing different types of Supply, a primary resource. They can exist within a storage building or as a movable container.
	/// </summary>
	public enum SupplyType
	{
		Basic = 1,
		Advanced = 2,
		Experimenal = 4,
	}

	/// <summary>
	/// Enum representing secondary resources. They can exist within a machine, a storage building, or as a movable container.
	/// </summary>	
	public enum ResourceType
	{
		/// <summary>
		/// A common resource used to make basic Supplies. Spawns in scrap deposits around the map.
		/// </summary>	
		Scrap,

		/// <summary>
		/// An uncommon resource used to create advanced Supplies. Harvested by mining machines from certain areas of the map.
		/// </summary>		
		Metal,

		/// <summary>
		///	A common resource used to boost Energy production on Tier 0-1 machines. Harvested by pumping machines from certain areas of the map.
		/// </summary>
		Oil,

		/// <summary>
		///	An uncommon resource used to enhance Energy production on Tier 2-3 machines. Rarely spawns in deposits around the map.
		/// </summary>
		Uranium,

		/// <summary>
		///	A common resource used to tempoarily boost Money production from Bitcoin Miners. Mined from deposits or harvested by pumping machines.
		/// </summary>	
		Coolant,

		/// <summary>
		///	An uncommon resource used to permanently upgrade Money production from Bitcoin Miners. Rarely spawns in deposits around the map.
		/// </summary>
		Parts,

		/// <summary>
		///	A rare resource that falls from the sky once in a while. Can be used to create experimental supplies, boost production of energy, or sold for a large sum of money.
		/// </summary>
		Stardust,
	}

	public partial class BaseItem
	{
		public uint NetworkId { get; set; }
		public virtual string Name => "";
		public virtual string Description => "";
		public virtual string Entity => "";
		public virtual string UniqueId => "";
		/// <summary>
		/// The amount of money a player should pay when buying the item.
		/// </summary>
		public virtual int CostMoney => 0;
		/// <summary>
		/// The monetary value of this item, counted towards its owner's net worth. If negative, uses CostMoney.
		/// </summary>
		public virtual int Worth => -1;

	}
}
