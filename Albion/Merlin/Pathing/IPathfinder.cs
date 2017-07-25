using System;

using System.Collections.Generic;

using YinYang.CodeProject.Projects.SimplePathfinding.Helpers;

namespace YinYang.CodeProject.Projects.SimplePathfinding.PathFinders
{
    public interface IPathfinder<TValue>
    {
		/// <summary>
		/// Performs the search for a path from <see cref="startValue" /> to <see cref="endValue" />.
		/// Returning status whether the <see cref="endValue" /> is accessible or not, and it is,
		/// also returns the list of the points leading to a <see cref="endValue" />.
		/// </summary>
		/// <param name="startValue"></param>
		/// <param name="endValue"></param>
		/// <param name="stopFunction">The stop function.</param>
		/// <param name="path">Returns the list of all the points of found path.</param>
		/// <param name="pivotPoints">Returns the list of the pivot points (sector points and corners).</param>
		/// <param name="ignoreStartEnd"></param>
		/// <param name="optimize">Determines whether the optimization is turned on.</param>
		/// <returns>
		/// If set to <c>True</c> the path was found, otherwise the target point is inaccessible.
		/// </returns>
		bool TryFindPath(TValue startValue, TValue endValue, 
					StopFunction<TValue> stopFunction, 
					out List<TValue> path, 
					out List<TValue> pivotPoints, 
					bool ignoreStartEnd = false, bool optimize = true);
    }
}
