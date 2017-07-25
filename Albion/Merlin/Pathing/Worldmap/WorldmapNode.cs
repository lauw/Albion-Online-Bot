using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldMap;
using YinYang.CodeProject.Projects.SimplePathfinding.PathFinders;

namespace Merlin.Pathing.Worldmap
{
	public class WorldmapNode : BaseGraphSearchNode<WorldmapNode, WorldmapCluster>, IComparable<WorldmapNode>
	{
		#region Static

		#endregion

		#region Fields

		/// <summary>
		/// Gets the actual score (distance to a finish).
		/// </summary>
		public Int32 Score { get; private set; }

		/// <summary>
		/// Gets or sets the estimated score.
		/// </summary>
		public Int32 EstimatedScore { get; set; }

		#endregion

		#region Properties and Events

		#endregion

		#region Constructors and Cleanup

		public WorldmapNode(WorldmapCluster cluster, WorldmapNode origin = null, Int32 score = 0, Int32 estimatedScore = 0) : base(cluster, origin)
		{
			Score = score;
			EstimatedScore = estimatedScore;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Updates the parameters on the fly.
		/// </summary>
		public void Update(Int32 score, Int32 estimatedScore, WorldmapNode origin)
		{
			Score = score;
			EstimatedScore = estimatedScore;
			Origin = origin;
		}

		public Boolean Equals(WorldmapCluster other)
		{
			return Value.Info.ak().Equals(other.Info.ak());
		}

		/// <summary>
		/// See <see cref="IComparable{T}.CompareTo"/> for more details.
		/// </summary>
		public Int32 CompareTo(WorldmapNode other)
		{
			return EstimatedScore.CompareTo(other.EstimatedScore);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return string.Format("Cluster = {0}, Score = {1}, Estimated score = {2}", Value.Info.ak(), Score, EstimatedScore);
		}

		#endregion
	}
}
