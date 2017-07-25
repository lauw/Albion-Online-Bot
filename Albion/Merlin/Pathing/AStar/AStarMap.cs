using System;
using UnityEngine;
using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;
using YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.Dijkstra;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar
{
    public class AStarMap : BaseDijkstraMap<AStarNode>
    {
        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="AStarMap"/> class.
        /// </summary>
        public AStarMap() : base() { }

        #endregion

        #region << BaseDijkstraMap >>

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnCreateFirstNode"/> for more details.
        /// </summary>
        protected override AStarNode OnCreateFirstNode(Vector2 startPosition, Vector2 endPosition)
        {
            return new AStarNode(startPosition, null, 0, HeuristicHelper.FastEuclideanDistance(startPosition, endPosition));
        }

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnCreateNode"/> for more details.
        /// </summary>
        protected override AStarNode OnCreateNode(Vector2 position, AStarNode origin, params object[] arguments)
        {
            Int32 score = arguments != null && arguments.Length > 0 ? (Int32)arguments[0] : 0;
            Int32 estimatedScore = arguments != null && arguments.Length > 1 ? (Int32) arguments[1] : 0;
            return new AStarNode(position, origin, score, estimatedScore);
        }

        #endregion
    }
}
