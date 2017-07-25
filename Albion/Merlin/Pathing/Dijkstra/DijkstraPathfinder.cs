using System;
using UnityEngine;
using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.Dijkstra
{
    public class DijkstraPathfinder : BaseGraphSearchPathfinder<DijkstraNode, DijkstraMap>
    {
        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="DijkstraPathfinder"/> class.
        /// </summary>
        public DijkstraPathfinder() : base() { }

		#endregion

		#region << BaseDijkstraFamilyPathfinder >>

		/// <summary>
		/// Determines the distance between neighbor points in an unified grid.
		/// </summary>
		/// <param name="start">The start point.</param>
		/// <param name="end">The neighbor point.</param>
		/// <returns></returns>
		/// <exception cref="System.NotSupportedException"></exception>
		protected override Int32 GetNeighborDistance(Vector2 start, Vector2 end)
		{
			return (int)(end - start).sqrMagnitude;
		}

		/// <summary>
		/// See <see cref="BaseGraphSearchPathfinder{TNode,TMap}.OnPerformAlgorithm"/> for more details.
		/// </summary>
		protected override void OnPerformAlgorithm(DijkstraNode currentNode, DijkstraNode neighborNode, Vector2 neighborPosition, Vector2 endPosition, StopFunction<Vector2> stopFunction)
        {
            Int32 neighborScore = currentNode.Score + GetNeighborDistance(currentNode.Value, neighborPosition);

            if (neighborNode == null)
            {
                Map.OpenNode(neighborPosition, currentNode, neighborScore);
            }
            else if (neighborScore < neighborNode.Score)
            {
                neighborNode.Update(neighborScore, currentNode);
            }
        }

        #endregion
    }
}
