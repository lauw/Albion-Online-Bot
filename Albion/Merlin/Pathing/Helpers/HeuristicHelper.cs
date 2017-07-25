using System;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.Helpers
{
    public class HeuristicHelper
    {
        /// <summary>
        /// Calculates fast (without square root) euclidean distance between two points.
        /// </summary>
        /// <param name="start">The start point.</param>
        /// <param name="end">The end point.</param>
        /// <returns></returns>
        public static Int32 FastEuclideanDistance(Vector2 start, Vector2 end)
        {
            return (int)(end - start).sqrMagnitude;
        }
    }
}
