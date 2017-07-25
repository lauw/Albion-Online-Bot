using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders
{
    public abstract class BaseGraphSearchPathfinder<TNode, TMap> : BasePathfinder<TNode, TMap, Vector2> 
        where TNode : BaseGraphSearchNode<TNode, Vector2>
        where TMap : BaseGraphSearchMap<TNode, Vector2>
    {
	    private const Int32 TooFewPoints = 3;
	    private const Int32 TooMuchPoints = 200;
	    private const Int32 OptimizationStep = 20;

		#region | Fields |

        protected readonly IEnumerable<DirectionType> Directions;

		#endregion

		#region | Constructors |

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseGraphSearchPathfinder{TNode,TMap}"/> class.
		/// </summary>
		protected BaseGraphSearchPathfinder() : base()
        {
            Directions = DirectionHelper.GetValues(AllowDiagonal);
        }

        #endregion

        #region | Helper methods |

        /// <summary>
        /// Determines the distance between neighbor points in an unified grid.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The neighbor point.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        protected virtual Int32 GetNeighborDistance(Vector2 start, Vector2 end)
        {
	        return 0;
        }

        /// <summary>
        /// Enumerates the neighbors points for a given node.
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <param name="stopFunction">The stop function.</param>
        /// <returns></returns>
        protected override IEnumerable<Vector2> OnEnumerateNeighbors(TNode currentNode, StopFunction<Vector2> stopFunction)
        {
	        return Directions.
		        // creates next step in this direction from current position
		        Select(direction => DirectionHelper.GetNextStep(currentNode.Value, direction));
        }

        #endregion

        #region | Virtual/abstract methods |

     

		#endregion

		#region | Optimization methods |

		/// <summary>
	    /// Performs path optimization. This is only a stub, that passes original path (and pivot points).
	    /// </summary>
	    /// <param name="inputPath">The input path.</param>
	    /// <param name="inputPivotPoints">The input pivot points.</param>
	    /// <param name="stopFunction">The stop function.</param>
	    /// <param name="optimizedPath">The optimized path.</param>
	    /// <param name="optimizedPivotPoints">The optimized pivot points.</param>
	    protected override void OnOptimizePath(List<Vector2> inputPath,
		    List<Vector2> inputPivotPoints,
		    StopFunction<Vector2> stopFunction,
		    out List<Vector2> optimizedPath,
		    out List<Vector2> optimizedPivotPoints)
	    {
		    optimizedPath = inputPath;
		    optimizedPivotPoints = inputPivotPoints;

		    // cannot optimize three points or less (2 - straight line, 3 - only one important point that cannot be optimized away)
		    if (inputPath.Count > TooFewPoints)
		    {
			    if (inputPath.Count > TooMuchPoints)
			    {
				    RecursiveDivisionOptimization(inputPath, stopFunction, out optimizedPath, out optimizedPivotPoints);
				    inputPath = optimizedPath;
			    }

			    VisibilityOptimization(inputPath, stopFunction, out optimizedPath, out optimizedPivotPoints);
			    Int32 lastCount = 0;

			    while (optimizedPath.Count != lastCount)
			    {
				    lastCount = optimizedPath.Count;
				    VisibilityOptimization(optimizedPath, stopFunction, out optimizedPath, out optimizedPivotPoints);
			    }
		    }
	    }

	    /// <summary>
	    /// This optimization should be used on limited point set (see <see cref="TooMuchPoints"/>).
	    /// It searches for unblocked connection from start to end point. The end point is moved,
	    /// until the connection is found (at worst the next connection = no optimizaton). If found
	    /// the start point is moved to this new point, and end point is reset to end point. 
	    /// This continues unless we're done.
	    /// </summary>
	    /// <param name="inputPath">The input path.</param>
	    /// <param name="stopFunction">The stop function.</param>
	    /// <param name="optimizedPath">The optimized path.</param>
	    /// <param name="optimizedPivotPoints">The optimized pivot points.</param>
	    protected static void VisibilityOptimization(List<Vector2> inputPath,
		    StopFunction<Vector2> stopFunction,
		    out List<Vector2> optimizedPath,
		    out List<Vector2> optimizedPivotPoints)
	    {
		    // creates result path
		    List<Vector2> result = new List<Vector2>();

		    // determines master point (one tested from), and last point (to detect cycle end)
		    Int32 masterIndex = 0;
		    Int32 lastIndex = inputPath.Count - 1;
		    Vector2 masterPosition = inputPath.ElementAt(masterIndex);

		    // adds first point
		    result.Add(masterPosition);

		    do // performs optimization loop
		    {
			    // starts at last points and work its way to the start point
			    for (Int32 index = Math.Min(OptimizationStep, lastIndex); index >= 0; index--)
			    {
				    Int32 referenceIndex = Math.Min(masterIndex + index, lastIndex);
				    Vector2 referencePosition = inputPath.ElementAt(referenceIndex);

				    // if reference point is visible from master point (or next, which is assumed as visible) reference point becomes master
				    if (referenceIndex == masterIndex + 1 || LineRasterizer.IsUnblocked(masterPosition, referencePosition, stopFunction))
				    {
					    // switches reference point as master, adding master to an optimized path
					    masterPosition = inputPath.ElementAt(referenceIndex);
					    masterIndex = referenceIndex;
					    result.Add(masterPosition);
					    break;
				    }
			    }
		    } while (masterIndex < lastIndex); // if we're on the last point -> terminate

		    // returns the optimized path
		    optimizedPath = result;
		    optimizedPivotPoints = result;
	    }

	    protected static void RecursiveDivisionOptimization(List<Vector2> inputPath,
		    StopFunction<Vector2> stopFunction,
		    out List<Vector2> optimizedPath,
		    out List<Vector2> optimizedPivotPoints)
	    {
		    // creates result path
		    List<Vector2> prunedSectors = new List<Vector2>();

		    // perfroms subdivision optimization (start -> end)
		    OptimizeSegment(0, inputPath.Count - 1, stopFunction, inputPath, prunedSectors);

		    // returns the optimized path
		    optimizedPath = prunedSectors;
		    optimizedPivotPoints = prunedSectors;
	    }

	    private static void OptimizeSegment(Int32 startIndex, Int32 endIndex,
		    StopFunction<Vector2> stopFunction,
		    List<Vector2> inputPath,
		    ICollection<Vector2> result)
	    {
		    Vector2 startPosition = inputPath.ElementAt(startIndex);
		    Vector2 endPosition = inputPath.ElementAt(endIndex);

		    // if this segment is unblocked, return start + end points
		    if (LineRasterizer.IsUnblocked(startPosition, endPosition, stopFunction))
		    {
			    result.Add(inputPath.ElementAt(startIndex));
			    result.Add(inputPath.ElementAt(endIndex));
		    }
		    else // otherwise subdivide segment in two, and process them
		    {
			    Int32 halfIndex = startIndex + (endIndex - startIndex) / 2 + 1;
			    OptimizeSegment(startIndex, halfIndex - 1, stopFunction, inputPath, result);
			    OptimizeSegment(halfIndex, endIndex, stopFunction, inputPath, result);
		    }
	    }

		#endregion
	}
}