using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Albion.Common.GameData.WorldInfos;

namespace Merlin.API
{
	public class Cluster
	{
		#region Static

		#endregion

		#region Fields

		private aif _cluster;

		#endregion

		#region Properties and Events

		public ClusterTypes ClusterType => _cluster.al().ClusterType;
		public Biome Biome => _cluster.al().Biome;
		public int Tier => _cluster.al().Tier;
		public Continents Continent => _cluster.al().Continent;
		public ClusterQualities ClusterQuality => _cluster.al().ClusterQuality;
		public Faction Faction => _cluster.al().Faction;

		public string Name => _cluster.ak();
		public string InternalName => _cluster.ah();

		public it.PvpRules PvPRules => _cluster.an().ao();

		public aif Internal => _cluster;

		#endregion

		#region Constructors and Cleanup
		
		public Cluster(aif cluster)
		{
			_cluster = cluster;
		}

		#endregion

		#region Methods

		public List<ClusterExit> GetExits()
		{
			var list = new List<ClusterExit>();

			foreach (var exit in _cluster.ax())
				list.Add(new ClusterExit(exit));

			return list;
		}

		#endregion
	}

	public class ClusterExit
	{
		#region Static

		#endregion

		#region Fields

		private aig _clusterExit;

		#endregion

		#region Properties and Events

		public Cluster Origin => new Cluster(_clusterExit.l());
		public Cluster Destination => new Cluster(_clusterExit.o());

		public bool IsRestricted => _clusterExit.u();

		public aig.Kind Kind => _clusterExit.r();

		public aig Internal => _clusterExit;

		#endregion

		#region Constructors and Cleanup
		
		public ClusterExit(aig clusterExit)
		{
			_clusterExit = clusterExit;
		}

		#endregion

		#region Methods

		#endregion
	}
}