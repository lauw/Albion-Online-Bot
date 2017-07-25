using System;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders.AStar
{
    public class AStarNode : BaseGraphSearchNode<AStarNode, Vector2>, IComparable<AStarNode>
    {
        #region | Properties |

        /// <summary>
        /// Gets the actual score (distance to a finish).
        /// </summary>
        public Int32 Score { get; private set; }

        /// <summary>
        /// Gets or sets the estimated score.
        /// </summary>
        public Int32 EstimatedScore { get; set; }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="AStarNode" /> struct.
        /// </summary>
        /// <param name="positionhe point.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="score">The score.</param>
        /// <param name="estimatedScore">The estimated score.</param>
        public AStarNode(Vector2 value, AStarNode origin = null, Int32 score = 0, Int32 estimatedScore = 0) : base(value, origin)
        {
            Score = score;
            EstimatedScore = estimatedScore;
        }

        #endregion

        #region | Methods |

        /// <summary>
        /// Updates the parameters on the fly.
        /// </summary>
        public void Update(Int32 score, Int32 estimatedScore, AStarNode origin)
        {
            Score = score;
            EstimatedScore = estimatedScore;
            Origin = origin;
        }

        #endregion

        #region << IComparable >>

        /// <summary>
        /// See <see cref="IComparable{T}.CompareTo"/> for more details.
        /// </summary>
        public Int32 CompareTo(AStarNode other)
        {
            return EstimatedScore.CompareTo(other.EstimatedScore);
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
            return string.Format("X = {0}, Y = {1}, Score = {2}, Estimated score = {3}", Value.x, Value.y, Score, EstimatedScore);
        }

        #endregion
    }
}
