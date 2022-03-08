using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	public partial class ItemEntity<T> : AnimEntity where T : BaseItem
	{
		[Net] public Player Player { get; private set; }
		[Net, Change] public uint ItemNetworkId { get; private set; }

		private T _itemCache;
		public T Item
		{
			get
			{
				if ( _itemCache == null )
					_itemCache = Items.Find<T>( ItemNetworkId );
				return _itemCache;
			}
		}

		public void ClearItemCache() => _itemCache = null;

		public void Assign( Player player, T item )
		{
			Host.AssertServer();

			var oldItem = Item;

			Player = player;
			ItemNetworkId = item.NetworkId;

			ClearItemCache();
			OnItemChanged( item, oldItem );
			OnPlayerAssigned( player );
		}

		protected virtual void OnItemChanged( T item, T oldItem ) { }
		protected virtual void OnPlayerAssigned( Player player ) { }
		protected virtual void OnItemNetworkIdChanged()
		{
			ClearItemCache();
		}
	}
}
