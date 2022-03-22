using System;
using System.Collections.Generic;
using System.Linq;
using Library;
using Voxelzator;
using Microsoft.CodeAnalysis;

namespace Voxelzator
{
    public class Solver
    {
        public const double Eps = 1e-7;

        public List<Point> CreateListOfRectangles(Point[] triangle, double step = 1)
        {
            List<Point> result = new List<Point>();

            //order triangle by x coordinate
            Array.Sort(triangle, new Comparison<Point>((lhs, rhs) => lhs.x.CompareTo(rhs.x)));

            var mainLinePasser = new LinePasser(triangle[0], triangle[2], step);
            var slaveLinePasser = new LinePasser(triangle[0], triangle[1], step);

            bool secondEdge = false;
            for (Point? mainCube = mainLinePasser.NextStep(), slaveCube = slaveLinePasser.NextStep();
                 mainCube != null;
                 mainCube = mainLinePasser.NextStep())
            {
                if (slaveCube != null && ((Point) slaveCube).x < ((Point) mainCube).x)
                {
                    //iterete over shorter(by X axis) line
                    while (slaveCube != null && ((Point) slaveCube).x < ((Point) mainCube).x)
                    {
                        result.Add((Point) slaveCube);
                        slaveCube = slaveLinePasser.NextStep();

                        if (slaveCube == null)
                        {
                            if (secondEdge)
                                break;
                            else
                                secondEdge = true;

                            slaveLinePasser = new LinePasser(triangle[1], triangle[2], step);
                            slaveCube = slaveLinePasser.NextStep();
                        }
                    }
                    
                    //fill cubes between lines
                    if (slaveCube != null)
                    {
                        result.Add((Point) slaveCube);
                        double currentStep = ((Point) mainCube).y < ((Point) slaveCube).y ? step : -step;
                        for (double y = ((Point) mainCube).y;
                             Math.Abs(y - ((Point) slaveCube).y) > Eps;
                             y += currentStep)
                            result.Add(new Point(((Point) mainCube).x, y));
                    }
                }

                result.Add((Point) mainCube);
            }

            return result.Distinct().ToList();
        }
    }
}