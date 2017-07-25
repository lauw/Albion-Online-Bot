using System;
using System.Collections.Generic;
using UnityEngine;
using YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders
{
    public abstract class BaseGraphSearchMap<TNode, TValue> where TNode : BaseGraphSearchNode<TNode, TValue>
    {
        #region | Fields |

        private Dictionary<TValue, TNode> nodes;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets the open nodes count.
        /// </summary>
        public Int32 OpenCount
        {
            get { return OnGetCount(); }
        }

	    public Dictionary<TValue, TNode> Nodes
	    {
			get { return nodes; }
	    }

        #endregion

        #region | Indexers |

        /// <summary>
        /// Gets the <see cref="AStarNode"/> on a given coordinates.
        /// </summary>
        public TNode this[TValue value]
        {
            get { return nodes[value]; }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGraphSearchMap{TNode}"/> class.
        /// </summary>
        protected BaseGraphSearchMap()
        {
	        nodes = new Dictionary<TValue, TNode>();
        }

        #endregion

        #region | Helper methods |

        private void OpenNodeInternal(TValue value, TNode result)
        {
            nodes[value] = result;
            OnAddNewNode(result);
        }

        #endregion

        #region | Virtual/abstract methods |

        protected abstract TNode OnCreateFirstNode(TValue startPosition, TValue endPosition);
        protected abstract TNode OnCreateNode(TValue position, TNode origin, params object[] arguments);

        protected abstract Int32 OnGetCount();
        protected abstract void OnAddNewNode(TNode result);
        protected abstract TNode OnGetTopNode();
        protected abstract void OnClear();

        #endregion

        #region | Methods |

        /// <summary>
        /// Creates new open node on a map at given coordinates and parameters.
        /// </summary>
        public void OpenNode(TValue value, TNode origin, params object[] arguments)
        {
            TNode result = OnCreateNode(value, origin, arguments);
            OpenNodeInternal(value, result);
        }

        public void OpenFirstNode(TValue startPosition, TValue endPosition)
        {
            TNode result = OnCreateFirstNode(startPosition, endPosition);
            OpenNodeInternal(startPosition, result);
        }

        /// <summary>
        /// Creates the empty node at given point.
        /// </summary>
        /// <param name="position">The point.</param>
        /// <returns></returns>
        public TNode CreateEmptyNode(TValue position)
        {
            return OnCreateNode(position, null); 
        }

        /// <summary>
        /// Returns top node (best estimated score), and closes it.
        /// </summary>
        /// <returns></returns>
        public TNode CloseTopNode()
        {
            TNode result = OnGetTopNode();
            result.MarkClosed();
            return result;
        }

        /// <summary>
        /// Clears map for another round.
        /// </summary>
        public void Clear()
        {
            nodes.Clear();
            OnClear();
        }

        #endregion
    }
}
