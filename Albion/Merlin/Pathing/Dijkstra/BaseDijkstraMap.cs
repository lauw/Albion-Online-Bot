using System;

using UnityEngine;

using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.Dijkstra
{
    public abstract class BaseDijkstraMap<TNode> : BaseGraphSearchMap<TNode, Vector2> where TNode : BaseGraphSearchNode<TNode, Vector2>, IComparable<TNode>
    {
        #region | Fields |

        private readonly PriorityQueue<TNode> priorityQueue;

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDijkstraMap{TNode}"/> class.
        /// </summary>
        protected BaseDijkstraMap() : base()
        {
            priorityQueue = new PriorityQueue<TNode>();
        }

        #endregion

        #region << BaseGraphSearchMap >>

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnGetCount"/> for more details.
        /// </summary>
        protected override Int32 OnGetCount()
        {
            return priorityQueue.Count;
        }

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnAddNewNode"/> for more details.
        /// </summary>
        protected override void OnAddNewNode(TNode result)
        {
            priorityQueue.Enqueue(result);
        }

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnGetTopNode"/> for more details.
        /// </summary>
        protected override TNode OnGetTopNode()
        {
            return priorityQueue.Dequeue();
        }

        /// <summary>
        /// See <see cref="BaseGraphSearchMap{TNode}.OnClear"/> for more details.
        /// </summary>
        protected override void OnClear()
        {
            priorityQueue.Clear();
        }

        #endregion
    }
}
