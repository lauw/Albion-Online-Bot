using System;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.Dijkstra
{
    public class DijkstraMap : BaseDijkstraMap<DijkstraNode>
    {
        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="DijkstraMap"/> class.
        /// </summary>
        public DijkstraMap() : base() { }

        #endregion

        #region << BaseDijkstraMap >>

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnCreateFirstNode"/> for more details.
        /// </summary>
        protected override DijkstraNode OnCreateFirstNode(Vector2 startPosition, Vector2 endPosition)
        {
            return new DijkstraNode(startPosition);
        }

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnCreateNode"/> for more details.
        /// </summary>
        protected override DijkstraNode OnCreateNode(Vector2 position, DijkstraNode origin, params object[] arguments)
        {
            Int32 score = arguments != null && arguments.Length > 0 ? (Int32)arguments[0] : 0;
            return new DijkstraNode(position, origin, score);
        }

        #endregion
    }
}
