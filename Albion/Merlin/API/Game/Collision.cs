using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Merlin.API
{
	public class Collision
	{
		#region Static

		public static Collision Instance
		{
			get
			{
				var internalCollision = ajb.a().y();

				if (internalCollision != null)
					return new Collision(internalCollision);

				return default(Collision);
			}
		} 

		#endregion

		#region Fields

		#endregion

		#region Properties and Events

		private aca _collision;

		#endregion

		#region Constructors and Cleanup

		protected Collision(aca collision)
		{
			_collision = collision;
		}

		#endregion

		#region Methods

		public byte GetFlag(Vector3 location, float distance)
		{
			return _collision.f(location.c(), distance);
		}

		#endregion
	}
}