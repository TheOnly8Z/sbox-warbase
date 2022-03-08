using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warbase
{
	public partial class BaseItem
	{
		public uint NetworkId { get; set; }
		public virtual string Name => "";
		public virtual string Description => "";
		public virtual string Entity => "";
		public virtual string UniqueId => "";

		// TODO: cost (money, supplies)

	}
}
