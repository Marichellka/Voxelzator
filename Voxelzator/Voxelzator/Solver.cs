using System;
using System.Collections.Generic;
using System.Linq;
using Voxelzator;
using Microsoft.CodeAnalysis;

namespace Voxelzator
{
    public class Solver
    {
        private const double eps = 1e-7;
        class LinePasser
        {
            private Point currentPoint;
            private Point lastPoint;
            private double xGridStep;
            private double yGridStep;
            private double xNextBorder;
            private double yNextBorder;
            private double dx;
            private double dy;
            public LinePasser(Point start, Point finish, double step)
            {
                currentPoint = start;
                lastPoint = finish;
                dx = finish.x - start.x;
                dy = finish.y - start.y;
                
                xGridStep = finish.x > start.x ? step : -step;
                yGridStep = finish.y > start.y ? step : -step;

                //TODO: figure out wether step shouldn't be replaced with xGridStep
                xNextBorder = (Math.Round((start.x - xGridStep / 2) / xGridStep) + 0.5) * xGridStep + xGridStep / 2;
                yNextBorder = (Math.Round((start.y - yGridStep / 2) / yGridStep) + 0.5) * yGridStep + yGridStep / 2;
            }

            static double LengthTillBorder(double current, double border, double speed)
            {
                return (border - current) / speed;
            }
            public Point NextStep()
            {
                
                if ((lastPoint.x - currentPoint.x) * dx <= eps || (lastPoint.y - currentPoint.y) * dy <= eps)
                    return null;

                var returnPoint = new Point(xNextBorder - xGridStep / 2, yNextBorder - yGridStep / 2);
                
                double xOffset = LengthTillBorder(currentPoint.x, xNextBorder, dx);
                double yOffset = LengthTillBorder(currentPoint.y, yNextBorder, dy);

                if (xOffset < yOffset)
                {
                    currentPoint.x = xNextBorder;
                    xNextBorder += xGridStep;
                    currentPoint.y += xOffset * dy;
                }
                else
                {
                    currentPoint.y = yNextBorder;
                    yNextBorder += yGridStep;
                    currentPoint.x += yOffset * dx;
                }
                
                return returnPoint;
            }
        }

        public List<Point> CreateListOfRectangles(Point[] triangle, double step = 1)
        {
            List<Point> result = new List<Point>();

            //order triangle by x coordinate
            Array.Sort(triangle, new Comparison<Point>((lhs, rhs) => lhs.x.CompareTo(rhs.x)));

            var mainLinePasser = new LinePasser(triangle[0], triangle[2], step);
            
            for(Point cube = mainLinePasser.NextStep(); cube != null; cube = mainLinePasser.NextStep())
                result.Add(cube);
            
            
            
            return result;
        }
    }
}