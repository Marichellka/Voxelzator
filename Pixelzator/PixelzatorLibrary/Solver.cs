using System;
using System.Collections.Generic;
using System.Linq;
using Library;

namespace PixelzatorLibrary
{
    public class Solver
    {
        public const double Eps = 1e-7;

        private Point ChangeCoordinateSystem(Point point, double step)
        {
            return new Point(point.X / step, point.Y / step);
        }
        
        public List<Point> CreateListOfRectangles(Point[] triangle, double step)
        {
            // change coordinate system for triangle
            for (int i = 0; i < triangle.Length; i++)
            {
                triangle[i] = ChangeCoordinateSystem(triangle[i], step);
            }
            
            List<Point> result = new List<Point>();

            //order triangle by x coordinate
            Array.Sort(triangle, new Comparison<Point>((lhs, rhs) => lhs.X.CompareTo(rhs.X)));

            var mainLinePasser = new LinePasser(triangle[0], triangle[2]);
            var slaveLinePasser = new LinePasser(triangle[0], triangle[1]);

            bool secondEdge = false;
            for (Point? mainCube = mainLinePasser.NextStep(), slaveCube = slaveLinePasser.NextStep();
                 mainCube != null;
                 mainCube = mainLinePasser.NextStep())
            {
                if (slaveCube != null && ((Point) slaveCube).X < ((Point) mainCube).X)
                {
                    //iterete over shorter(by X axis) line
                    while (slaveCube != null && ((Point) slaveCube).X < ((Point) mainCube).X)
                    {
                        result.Add((Point) slaveCube);
                        slaveCube = slaveLinePasser.NextStep();

                        if (slaveCube == null)
                        {
                            if (secondEdge)
                                break;
                            else
                                secondEdge = true;

                            slaveLinePasser = new LinePasser(triangle[1], triangle[2]);
                            slaveCube = slaveLinePasser.NextStep();
                        }
                    }
                    
                    //fill cubes between lines
                    if (slaveCube != null)
                    {
                        result.Add((Point) slaveCube);
                        double currentStep = ((Point) mainCube).Y < ((Point) slaveCube).Y ? 1 : -1;
                        for (double y = ((Point) mainCube).Y;
                             Math.Abs(y - ((Point) slaveCube).Y) > Eps;
                             y += currentStep)
                            result.Add(new Point(((Point) mainCube).X, y));
                    }
                }

                result.Add((Point) mainCube);
            }
            
            // change coordinate system back (up to now, that is not needed)
            // step = 1 / step;
            // for (int i = 0; i < result.Count; i++)
            // {
            //     result[i] = ChangeCoordinateSystem(result[i], step);
            // }
            
            return result.Distinct().ToList();
        }
    }
}