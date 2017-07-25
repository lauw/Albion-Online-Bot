using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YinYang.CodeProject.Projects.SimplePathfinding.Helpers
{
    public class LineRasterizer
    {
        #region | Common |

        protected static Int32 GetDirection(Int32 value, out Boolean positive)
        {
            positive = value > 0;
            return positive ? 1 : -1;
        }

        protected static Int32 GetDirection(Int32 lowValue, Int32 highValue)
        {
            return lowValue < highValue ? 1 : -1;
        }

        protected static IEnumerable<Vector2> CreateYield(Int32 x, Int32 y)
        {
            yield return new Vector2(x, y);
        }

        #endregion

        #region | Horizontal line |

        public static IEnumerable<Vector2> EnumerateHorizontalLine(Int32 x1, Int32 x2, Int32 y)
        {
            IEnumerable<Vector2> points = EnumerateHorizontalLine(x2 - x1 + GetDirection(x1, x2));
            return points.Select(point => new Vector2(point.x + x1, point.y + y));
        }

        public static IEnumerable<Vector2> EnumerateHorizontalLine(Int32 width)
        {
            // preliminary check - zero point
            if (width == 0) return CreateYield(0, 0);

            // enumerates horizontal line
            return EnumerateHorizontalLineInternal(width);
        }

        private static IEnumerable<Vector2> EnumerateHorizontalLineInternal(Int32 width)
        {
            // preliminary check - zero point
            if (width == 0) return CreateYield(0, 0);

            // possible problem - reversed order
            Boolean positive;
            Int32 direction = GetDirection(width, out positive);

            // prepares buffer
            List<Vector2> result = new List<Vector2>();

            // enumerates units in a reverse direction
            for (Int32 x = 0; (!positive && x > width) || (positive && x < width); x += direction)
            {
                Vector2 position = new Vector2(x, 0);
                result.Add(position);
            }

            // returns the points
            return result;
        }

        #endregion

        #region | Vertical line |

        public static IEnumerable<Vector2> EnumerateVerticalLine(Int32 x, Int32 y1, Int32 y2)
        {
            IEnumerable<Vector2> points = EnumerateVerticalLine(y2 - y1 + GetDirection(y1, y2));
            return points.Select(point => new Vector2(point.x + x, point.y + y1));
        }

        public static IEnumerable<Vector2> EnumerateVerticalLine(Int32 height, Func<Vector2, Boolean> stopFunction = null)
        {
            // preliminary check - zero point
            if (height == 0) return CreateYield(0, 0);

            // enumerates vertical line
            return EnumerateVerticalLineInternal(height);
        }

        private static IEnumerable<Vector2> EnumerateVerticalLineInternal(Int32 height)
        {
            // possible problem - reversed order
            Boolean positive;
            Int32 direction = GetDirection(height, out positive);

            // prepares buffer
            List<Vector2> result = new List<Vector2>();

            // enumerates units in a given direction
            for (Int32 y = 0; (!positive && y > height) || (positive && y < height); y += direction)
            {
                Vector2 position = new Vector2(0, y);
                result.Add(position);
            }

            // returns the points
            return result;
        }

        #endregion

        #region | Line |

        public static IEnumerable<Vector2> EnumerateLine(Int32 x1, Int32 y1, Int32 x2, Int32 y2, Boolean allowDiagonals = false)
        {
            IEnumerable<Vector2> points = EnumerateLine(x2 - x1, y2 - y1, allowDiagonals);
            return points.Select(point => new Vector2(point.x + x1, point.y + y1));
        }

        public static IEnumerable<Vector2> EnumerateLine(Int32 targetX, Int32 targetY, Boolean allowDiagonals = false)
        {
            // preliminary check - zero point
            if (targetX == 0 && targetY == 0) return CreateYield(0, 0);

            // preliminary check - vertical line
            if (targetX == 0) return EnumerateVerticalLine(0, 0, targetY);

            // preliminary check - horizontal line
            if (targetY == 0) return EnumerateHorizontalLine(0, targetX, 0);

            // enumerates line
            return EnumerateLineInternal(targetX, targetY, allowDiagonals);
        }

        private static IEnumerable<Vector2> EnumerateLineInternal(Int32 targetX, Int32 targetY, Boolean allowDiagonals = false)
        {
            // initializes the variables of the line
            Int32 count, error;
            Int32 x = 0, y = 0;
            Int32 deltaX = Math.Abs(targetX);
            Int32 deltaY = Math.Abs(targetY);
            Int32 stepLeftX, stepLeftY;
            Int32 stepRightX, stepRightY;
            Int32 stepErrorLeft, stepErrorRight;

            // gradual elevation (angle <= 45°)
            if (deltaX >= deltaY)
            {
                count = deltaX + 1;
                error = (deltaY << 1) - deltaX;
                stepErrorLeft = deltaY << 1;
                stepErrorRight = (deltaY - deltaX) << 1;
                stepLeftX = 1; stepRightX = 1;
                stepLeftY = 0; stepRightY = 1;
            }
            else // steep elevation (angle > 45°)
            {
                count = deltaY + 1;
                error = (deltaX << 1) - deltaY;
                stepErrorLeft = deltaX << 1;
                stepErrorRight = (deltaX - deltaY) << 1;
                stepLeftX = 0; stepRightX = 1;
                stepLeftY = 1; stepRightY = 1;
            }

            // possible problem - reversed horizontal alignment (← instead of →)
            if (targetX < 0)
            {
                stepLeftX = -stepLeftX;
                stepRightX = -stepRightX;
            }

            // possible problem - reversed vertical alignment (↓ instead of ↑)
            if (targetY < 0)
            {
                stepLeftY = -stepLeftY;
                stepRightY = -stepRightY;
            }

            Boolean stepLeftDiagonal = false;
            Boolean stepRightDiagonal = false;
            Boolean isLeftDominant = false;

            if (allowDiagonals)
            {
                stepLeftDiagonal = Math.Abs(stepLeftX + stepLeftY) != 1;
                stepRightDiagonal = Math.Abs(stepRightX + stepRightY) != 1;
                isLeftDominant = Math.Abs(stepErrorLeft) > Math.Abs(stepErrorRight);
            }

            // enumerates a line using a specific unit
            for (Int32 a = 0; a < count; a++)
            {
                // enqueues the new unit
                yield return new Vector2(x, y);

                // moves position one step near to end
                if (error < 0)
                {
                    if (allowDiagonals && stepLeftDiagonal)
                    {
                        if (isLeftDominant)
                        {
                            yield return new Vector2(x, y + stepLeftY);
                        }
                        else
                        {
                            yield return new Vector2(x + stepLeftX, y);
                        }
                    }

                    error += stepErrorLeft;
                    x += stepLeftX;
                    y += stepLeftY;
                }
                else
                {
                    if (allowDiagonals && stepRightDiagonal)
                    {
                        if (isLeftDominant)
                        {
                            yield return new Vector2(x, y + stepRightY);
                        }
                        else
                        {
                            yield return new Vector2(x + stepRightX, y);
                        }
                    }

                    error += stepErrorRight;
                    x += stepRightX;
                    y += stepRightY;
                }
            }
        }

        #endregion

        #region | Pathfinding |

        public static Boolean IsUnblocked(Vector2 start, Vector2 end, StopFunction<Vector2> stopFunction)
        {
            return EnumerateLine((int)start.x, (int)start.y, (int)end.x, (int)end.y).All(point => !stopFunction(point));
        }

        #endregion
    }
}
