using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	public partial class ItemEntity<T> : AnimEntity, IOwnableEntity, IValuableEntity where T : BaseItem
	{
		[Net, Change] public uint ItemNetworkId { get; private set; }

		/// <summary>
		/// Not to be confused with Entity.Owner.
		/// </summary>
		[Net] protected Player PlayerOwner { get; private set; }
		[Net] protected Team TeamOwner { get; private set; }

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

		public void Assign( T item )
		{
			Host.AssertServer();

			var oldItem = Item;

			ItemNetworkId = item.NetworkId;

			ClearItemCache();
			OnItemChanged( item, oldItem );
		}

		protected virtual void OnItemChanged( T item, T oldItem ) { }
		protected virtual void OnItemNetworkIdChanged()
		{
			ClearItemCache();
		}

		// Interface stuff

		public bool CheckOwner( Player player ) => TeamOwner != null ? TeamOwner.HasPlayer(player ) : player == PlayerOwner;
		public bool IsOwnedByTeam() => TeamOwner != null;
		public bool IsOwnedByPlayer() => TeamOwner == null && PlayerOwner != null;
		public bool HasOwner() => TeamOwner != null || PlayerOwner != null;
		public Player GetOwnerPlayer() => PlayerOwner;
		public Team GetOwnerTeam() => TeamOwner;
		public void SetOwner( Player player )
		{
			PlayerOwner = player;
		}
		public void SetOwner( Team team )
		{
			TeamOwner = team;
		}
		public int GetWorth()
		{
			if ( Item != null )
			{
				return Item.Worth;
			}
			return 0;
		}
	}
}
