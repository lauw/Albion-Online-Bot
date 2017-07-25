using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace YinYang.CodeProject.Projects.SimplePathfinding.Helpers
{
    public class EllipseRasterizer
    {
        private const Double Step = 0.001;
        private const Double HalfPi = Math.PI / 2.0;

        protected static IEnumerable<Vector2> CreateYield(Int32 x, Int32 y)
        {
            yield return new Vector2(x, y);
        }

        public static IEnumerable<Vector2> Enumerate(Int32 centerX, Int32 centerY, Int32 radiusX, Int32 radiusY, Boolean filled = false)
        {
            IEnumerable<Vector2> points = Enumerate(radiusX, radiusY, filled);
            return points.Select(point => new Vector2(point.x + centerX - radiusX, point.y + centerY - radiusY));
        }

        public static IEnumerable<Vector2> Enumerate(Int32 radiusX, Int32 radiusY, Boolean filled = false)
        {
            // preliminary check - zero point
            if (radiusX == 0 || radiusY == 0) return Enumerable.Empty<Vector2>();

            // preliminary check - horizontal line
            if (radiusY == 1 || radiusY == -1) return LineRasterizer.EnumerateHorizontalLine(radiusX);

            // preliminary check - vertical line
            if (radiusX == 1 || radiusX == -1) return LineRasterizer.EnumerateVerticalLine(radiusY);

            // potencial problem - negative radius X
            if (radiusX < 0) radiusX = -radiusX;

            // potencial problem - negative radius Y
            if (radiusY < 0) radiusY = -radiusY;

            // enumerates ellipse
            return EnumerateEllipse(radiusX, radiusY, filled);
        }

        /// <summary>
        /// If you wonder why I didn't use some faster algorithm (bresenham, mid-point..) it's because it wouldn't match
        /// the outline of drawn ellipse via Graphics.DrawEllipse. So the obstacle looks elsewhere then the drawn ellipse.
        /// </summary>
        private static IEnumerable<Vector2> EnumerateEllipse(Int32 radiusX, Int32 radiusY, Boolean filled = false)
        {
            Double anomaly = HalfPi;
            Vector2 lastPosition = Vector2.zero;

            List<Vector2> result = new List<Vector2>
            {
                new Vector2(-radiusX, radiusY), 
                new Vector2(radiusX, radiusY)
            };

            if (filled) result.Add(Vector2.zero);

            while (anomaly >= 0.0)
            {
                Int32 shiftX = Convert.ToInt32(radiusX*Math.Cos(anomaly));
                Int32 shiftY = Convert.ToInt32(radiusY*Math.Sin(anomaly));

                Int32 x = radiusX + shiftX;
                Int32 y = radiusY + shiftY;

                if (x != lastPosition.x || y != lastPosition.y)
                {
                    Vector2 topLeft = new Vector2(radiusX - shiftX, radiusY - shiftY);
                    Vector2 topRight = new Vector2(radiusX + shiftX, radiusY - shiftY);
                    Vector2 bottomLeft = new Vector2(radiusX - shiftX, radiusY + shiftY);
                    Vector2 bottomRight = new Vector2(radiusX + shiftX, radiusY + shiftY);

                    result.Add(topLeft);
                    if (filled) result.AddRange(LineRasterizer.EnumerateHorizontalLine(radiusX - shiftX + 1, radiusX + shiftX - 1, radiusY - shiftY));
                    result.Add(topRight);

                    result.Add(bottomLeft);
                    if (filled) result.AddRange(LineRasterizer.EnumerateHorizontalLine(radiusX - shiftX + 1, radiusX + shiftX - 1, radiusY + shiftY));
                    result.Add(bottomRight);

                    lastPosition = bottomRight;
                }

                anomaly -= Step;
            }

            return result;
        }
    }
}
