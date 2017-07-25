using System;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.Dijkstra
{
    public class DijkstraNode : BaseGraphSearchNode<DijkstraNode, Vector2>, IComparable<DijkstraNode>
    {
        #region | Properites |

        /// <summary>
        /// Gets the actual score (distance to a finish).
        /// </summary>
        public Int32 Score { get; private set; }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="DijkstraNode"/> class.
        /// </summary>
        /// <param name="positionhe point.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="score">The score.</param>
        public DijkstraNode(Vector2 value, DijkstraNode origin = null, Int32 score = 0) : base(value, origin)
        {
            Score = score;
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Updates the parameters on the fly.
        /// </summary>
        /// <param name="score">The score.</param>
        /// <param name="parent">The parent.</param>
        public void Update(Int32 score, DijkstraNode parent)
        {
            Score = score;
            Origin = parent;
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
            return string.Format("X = {0}, Y = {1}, Score = {2}", Value.x, Value.y, Score);
        }

        #endregion

        #region << IComparable >>

        /// <summary>
        /// See <see cref="IComparable{T}.CompareTo"/> for more details.
        /// </summary>
        public Int32 CompareTo(DijkstraNode other)
        {
            return Score.CompareTo(other.Score);
        }

        #endregion
    }
}
