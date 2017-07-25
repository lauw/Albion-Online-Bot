using System;
using UnityEngine;
using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar
{
    public class AStarPathfinder : BaseGraphSearchPathfinder<AStarNode, AStarMap>
    {
        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="AStarPathfinder"/> class.
        /// </summary>
        public AStarPathfinder() : base() { }

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
		protected override void OnPerformAlgorithm(AStarNode currentNode, AStarNode neighborNode, Vector2 neighborPosition, Vector2 endPosition, StopFunction<Vector2> stopFunction)
        {
            Int32 neighborScore = currentNode.Score + GetNeighborDistance(currentNode.Value, neighborPosition);

            // opens node at this position
            if (neighborNode == null)
            {
                Map.OpenNode(neighborPosition, currentNode, neighborScore, neighborScore + HeuristicHelper.FastEuclideanDistance(neighborPosition, endPosition));
            }
            else if (neighborScore < neighborNode.Score)
            {
                neighborNode.Update(neighborScore, neighborScore + HeuristicHelper.FastEuclideanDistance(neighborPosition, endPosition), currentNode);
            }
        }

        #endregion
    }
}
