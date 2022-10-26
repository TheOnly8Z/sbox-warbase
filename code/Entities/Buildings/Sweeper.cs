using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sandbox;

namespace Warbase
{
	public class Sweeper : ModelEntity
	{

		private List<Entity> Touching;

		public Sweeper() : base()
		{
			Touching = new();

			Transmit = TransmitType.Never;
			EnableDrawing = false;
			CollisionGroup = CollisionGroup.Trigger;

			EnableTouch = true;
			EnableTouchPersists = true;
		}

		public bool Check()
		{
			foreach ( Entity e in Touching )
			{
				if ( e.IsValid() )
				{
					return false;
				}
			}
			return true;
		}

		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );
			Touching.Add( other );
		}

		public override void EndTouch( Entity other )
		{
			base.EndTouch( other );
			Touching.Remove( other );
		}
	}
}
