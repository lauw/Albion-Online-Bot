using System;
using System.Collections.Generic;
using UnityEngine;

namespace YinYang.CodeProject.Projects.SimplePathfinding.Helpers
{
    public class DirectionHelper
    {
        public static Vector2 GetNextStep(Vector2 startPosition, DirectionType direction, Int32 steps = 1)
        {
            Vector2 nextPosition = GetNextStep(direction, steps);
            return new Vector2(startPosition.x + nextPosition.x, startPosition.y + nextPosition.y);
        }

        public static Vector2 GetNextStep(DirectionType direction, Int32 steps = 1)
        {
            Int32 x = 0, y = 0;

            if ((direction & DirectionType.West) != 0) x = -steps;
            if ((direction & DirectionType.East) != 0) x = +steps;
            if ((direction & DirectionType.North) != 0) y = +steps;
            if ((direction & DirectionType.South) != 0) y = -steps;

            return new Vector2(x, y);
        }
		
        public static IEnumerable<DirectionType> GetValues(Boolean allowDiagonals = true)
        {
            yield return DirectionType.North;
            if (allowDiagonals) yield return DirectionType.NorthEast;
            yield return DirectionType.East;
            if (allowDiagonals) yield return DirectionType.SouthEast;
            yield return DirectionType.South;
            if (allowDiagonals) yield return DirectionType.SouthWest;
            yield return DirectionType.West;
            if (allowDiagonals) yield return DirectionType.NorthWest;
        }
    }
}
