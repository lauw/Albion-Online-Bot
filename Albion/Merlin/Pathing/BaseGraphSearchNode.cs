using System;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders
{
    public abstract class BaseGraphSearchNode<TNode, TValue> where TNode : BaseGraphSearchNode<TNode, TValue>
    {
        #region | Properties |

        /// <summary>
        /// Gets the node coordinates in a map.
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// Gets the origin (the node from which this node was opened).
        /// </summary>
        public TNode Origin { get; protected set; }

        /// <summary>
        /// Determiens whether the node was already processed (true) or not.
        /// </summary>
        public Boolean IsClosed { get; private set; }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="TNode" /> struct.
        /// </summary>
        protected BaseGraphSearchNode(TValue value, TNode origin = null)
        {
            Value = value;
            Origin = origin;
            IsClosed = false;
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Marks node as closed.
        /// </summary>
        public void MarkClosed()
        {
            IsClosed = true;
        }

        #endregion

        #region << IEquatable >>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public Boolean Equals(TNode other)
        {
            return Value.Equals(other.Value);
        }

        #endregion

        #region << Object >>

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Value = {0}", Value);
        }

        #endregion
    }
}
