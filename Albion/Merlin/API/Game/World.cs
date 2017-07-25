using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using WorldMap;


namespace Merlin.API
{
	/* Internal Type: ajb */
	public class World
	{
		#region Static

		public static World Instance
		{
			get 
			{ 
				var internalWorld = ajb.a();

				if (internalWorld != null)
					return new World(internalWorld);

				return default(World);
			}
		}

		private static MethodInfo _getEntitiesCollection;
		private static FieldInfo _getWorldmapClusters;

		static World()
		{
			_getEntitiesCollection = typeof(ajb).GetMethod("an", BindingFlags.NonPublic | BindingFlags.Instance);
			_getWorldmapClusters = typeof(Worldmap).GetField("b", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		#endregion

		#region Fields

		private ajb _internal;

		#endregion

		#region Properties and Events

		public WorldmapCluster CurrentCluster => GetCluster(_internal.w());

		#endregion

		#region Constructors and Cleanup

		protected World(ajb world)
		{
			_internal = world;
		}

		#endregion

		#region Methods

		public Dictionary<long, apd> GetEntities() 
		{
			return _getEntitiesCollection.Invoke(_internal, new object[] { }) as Dictionary<long, apd>;
		}

		public Dictionary<string, WorldmapCluster> GetClusters()
		{
			return _getWorldmapClusters.GetValue(GameGui.Instance.WorldMap) as Dictionary<string, WorldmapCluster>;
		}

		public WorldmapCluster GetCluster(aif info)
		{
			var clusters = GetClusters();

			if (clusters.TryGetValue(info.ah(), out WorldmapCluster cluster))
				return cluster;

			return default(WorldmapCluster);
		}

		public WorldmapCluster GetCluster(string name)
		{
			var clusters = GetClusters();

			foreach (var cluster in clusters.Values)
			{
				if (cluster.Info.ak().ToLower() == name.ToLower())
					return cluster;
			}

			return null;
		}

		#endregion
	}
}