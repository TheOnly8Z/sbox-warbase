using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	// Lifted from Facepunch.RTS; stores all items
	public static partial class ItemLibrary
	{
		public static Dictionary<string, BaseItem> Table { get; private set; }
		public static List<BaseItem> List { get; private set; }
		public static Dictionary<uint, Texture> Icons { get; private set; }

		public static void Initialize()
		{
			Icons = new();
			BuildTable();
		}

		private static void BuildTable()
		{
			Table = new();
			List = new();

			var list = new List<BaseItem>();

			foreach ( var type in TypeLibrary.GetTypes<BaseItem>() )
			{
				var item = TypeLibrary.Create<BaseItem>( type );
				if (item.UniqueId != "")
				{
					list.Add( item );
				}
			}

			// Sort alphabetically, this should result in the same index for client and server.
			list.Sort( ( a, b ) => a.UniqueId.CompareTo( b.UniqueId ) );

			for ( var i = 0; i < list.Count; i++ )
			{
				var item = list[i];

				Table.Add( item.UniqueId, item );
				List.Add( item );

				item.NetworkId = (uint)(i + 1);

				Log.Info( $"Adding {item.UniqueId} to ItemLibrary (id = {item.NetworkId})" );
			}
		}

		public static BuildableEntity Create( Player player, BuildableItem item )
		{
			Host.AssertServer();

			BuildableEntity entity;

			if ( string.IsNullOrEmpty( item.Entity ) )
				entity = new BuildableEntity();
			else
				entity = TypeLibrary.Create<BuildableEntity>( item.Entity );

			entity.Assign( item );

			return entity;
		}

		public static T Find<T>( string id ) where T : BaseItem
		{
			if ( Table.TryGetValue( id, out var item ) )
				return (item as T);

			return null;
		}

		public static T Find<T>( uint id ) where T : BaseItem
		{
			var index = id - 1;

			if ( index < List.Count )
				return (List[(int)index] as T);

			return null;
		}

	}
}
