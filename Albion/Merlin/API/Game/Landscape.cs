







using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

namespace Merlin.API
{
	public class Landscape
	{
		#region Static

		public static Landscape Instance
		{
			get
			{
				var internalLandscape = ak8.a().x();

				if (internalLandscape != null)
					return new Landscape(internalLandscape);

				return default(Landscape);
			}
		} 

		#endregion

		#region Fields

		#endregion

		#region Properties and Events

		private aog _landscape;

		#endregion

		#region Constructors and Cleanup

		protected Landscape(aog landscape)
		{
			_landscape = landscape;
		}

		#endregion

		#region Methods

		public float GetLandscapeHeight(ajd position)
		{
			return _landscape.p(position);
		}

		#endregion
	}
}