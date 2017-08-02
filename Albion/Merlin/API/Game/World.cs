







using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using WorldMap;


namespace Merlin.API
{
	/* Internal Type: ak8 */
	public class World
	{
		#region Static

		public static World Instance
		{
			get 
			{ 
				var internalWorld = ak8.a();

				if (internalWorld != null)
					return new World(internalWorld);

				return default(World);
			}
		}

		private static MethodInfo _getEntitiesCollection;
		private static FieldInfo _getWorldmapClusters;

		static World()
		{
			_getEntitiesCollection = typeof(ak8).GetMethod("ai", BindingFlags.NonPublic | BindingFlags.Instance);
			_getWorldmapClusters = typeof(Worldmap).GetField("c", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		#endregion

		#region Fields

		private ak8 _internal;

		#endregion

		#region Properties and Events

		public WorldmapCluster CurrentCluster => GetCluster(_internal.u());

		#endregion

		#region Constructors and Cleanup

		protected World(ak8 world)
		{
			_internal = world;
		}

		#endregion

		#region Methods

		public Dictionary<long, are> GetEntities() 
		{
			return _getEntitiesCollection.Invoke(_internal, new object[] { }) as Dictionary<long, are>;
		}

		public Dictionary<string, WorldmapCluster> GetClusters()
		{
			return _getWorldmapClusters.GetValue(GameGui.Instance.WorldMap) as Dictionary<string, WorldmapCluster>;
		}

		public WorldmapCluster GetCluster(akb info)
		{
			var clusters = GetClusters();

			if (clusters.TryGetValue(info.ak(), out WorldmapCluster cluster))
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