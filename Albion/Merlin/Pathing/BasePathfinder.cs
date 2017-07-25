using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Merlin;
using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders
{
    public abstract class BasePathfinder<TNode, TMap, TValue> : IPathfinder<TValue>
	    where TNode : BaseGraphSearchNode<TNode, TValue>
	    where TMap : BaseGraphSearchMap<TNode, TValue>
	{
		#region | Constants |

		#endregion

		#region | Fields |

		protected readonly TMap Map;

		protected TNode EndNode;

		#endregion

		#region | Calculated properties |

		/// <summary>
		/// Determines whether this algorithm supports diagonal directions or not.
		/// </summary>
		public virtual Boolean AllowDiagonal
		{
			get { return true; }
		}

		#endregion

		#region | Constructors |

		/// <summary>
		/// Initializes a new instance of the <see cref="BasePathfinder{TNode,TMap,TValue}"/> class.
		/// </summary>
		protected BasePathfinder()
		{
			Map = (TMap)Activator.CreateInstance(typeof(TMap));
		}

		#endregion


		#region | Abstract/virtual methods |

		protected abstract void OnPerformAlgorithm(TNode currentNode, TNode neighborNode, TValue neighborPosition, TValue endPosition, StopFunction<TValue> stopFunction);

		/// <summary>
		/// Reconstructs the path from end node, back to the start node using originating node.
		/// </summary>
		/// <param name="endPosition">The end point.</param>
		/// <returns></returns>
		protected List<TValue> ReconstructPath(TValue endPosition)
		{
			// starts at end point
			TNode origin;
			TValue currentPosition = endPosition;

			// use linked list for faster insertion (to avoid reversing the array)
			LinkedList<TValue> result = new LinkedList<TValue>(new[] { endPosition });

			do // tracks back the nodes to find the path
			{
				origin = Map[currentPosition];

				if (origin != null)
				{
					origin = origin.Origin;

					if (origin != null)
					{
						result.AddFirst(origin.Value);
						currentPosition = origin.Value;
					}
				}
			}
			while (origin != null);

			// converts it to a regular read-only collection
			return result.ToList();
		}

		/// <summary>
		/// Enumerates the neighbors points for a given node.
		/// </summary>
		/// <param name="currentNode">The current node.</param>
		/// <param name="stopFunction">The stop function.</param>
		/// <returns></returns>
		protected abstract IEnumerable<TValue> OnEnumerateNeighbors(TNode currentNode, StopFunction<TValue> stopFunction);

		/// <summary>
		/// See <see cref="IPathfinder.TryFindPath"/> for more details.
		/// </summary>
		protected virtual Boolean OnTryFindPath(TValue startValue, TValue endValue,
			StopFunction<TValue> stopFunction,
			out List<TValue> path,
			out List<TValue> pivotPoints,
			Boolean optimize = true)
		{
			// prepares main parameters
			Boolean result = false;
			pivotPoints = null;
			path = null;

			// clears the map
			Map.Clear();

			// creates start/finish nodes
			TNode endNode = EndNode = Map.CreateEmptyNode(endValue);

			// prepares first node
			Map.OpenFirstNode(startValue, endValue);

			var depth = 0;

			while (Map.OpenCount > 0)
			{
				TNode currentNode = Map.CloseTopNode();

				// if current node is obstacle, skip it
				if (stopFunction(currentNode.Value)) continue;

				if (depth++ > (600 * 600))
					return false;

				// if we've detected end node, reconstruct the path back to the start
				if (currentNode.Equals(endNode))
				{
					path = ReconstructPath(endValue);
					result = true;
					break;
				}

				// processes all the neighbor points
				foreach (TValue neighborPoint in OnEnumerateNeighbors(currentNode, stopFunction))
				{
					// if this neighbor is obstacle skip it, it is not viable node
					if (stopFunction(neighborPoint)) continue;

					// determines the node if possible, whether it is closed, and calculates its score
					TNode neighborNode = default(TNode);

					if (Map.Nodes.TryGetValue(neighborPoint, out neighborNode))
					{
						Boolean inClosedSet = neighborNode.IsClosed;

						// if this node was already processed, skip it
						if (inClosedSet)
							continue;
					}

					// performs the implementation specific variant of graph search algorithm
					OnPerformAlgorithm(currentNode, neighborNode, neighborPoint, endValue, stopFunction);
				}
			}

			return result;
		}

        /// <summary>
        /// Performs path optimization. This is only a stub, that passes original path (and pivot points).
        /// </summary>
        /// <param name="inputPath">The input path.</param>
        /// <param name="inputPivotPoints">The input pivot points.</param>
        /// <param name="stopFunction">The stop function.</param>
        /// <param name="optimizedPath">The optimized path.</param>
        /// <param name="optimizedPivotPoints">The optimized pivot points.</param>
        protected virtual void OnOptimizePath(List<TValue> inputPath, 
                                              List<TValue> inputPivotPoints,
                                              StopFunction<TValue> stopFunction,
                                              out List<TValue> optimizedPath, 
                                              out List<TValue> optimizedPivotPoints)
        {
            optimizedPath = inputPath;
            optimizedPivotPoints = inputPivotPoints;
        }

        #endregion

        #region << IPathFinder >>

        /// <summary>
        /// See <see cref="IPathfinder.TryFindPath"/> for more details.
        /// </summary>
        public bool TryFindPath(TValue startValue, TValue endValue, StopFunction<TValue> stopFunction, 
			out List<TValue> path, out List<TValue> pivotPoints, bool ignoreStartEnd = false, bool optimize = true)
        {
            // creates obstacle function
            pivotPoints = new List<TValue>();
            path = new List<TValue>();

            // start or finish are blocked, we cannot find path
	        if (!ignoreStartEnd && stopFunction(startValue))
		        return false;

	        if (!ignoreStartEnd && stopFunction(endValue))
				return false;

            // start and finish are the same point, return 1 step path
            if (startValue.Equals(endValue))
            {
				path.Add(startValue);
				return true;
            }

            // finds the path (alternatively also optimizes/smooths it afterwards)
            Boolean result = OnTryFindPath(startValue, endValue, stopFunction, out path, out pivotPoints, optimize);

            if (result && optimize)
				OnOptimizePath(path, pivotPoints, stopFunction, out path, out pivotPoints);

			return result;
        }

        #endregion
    }
}
