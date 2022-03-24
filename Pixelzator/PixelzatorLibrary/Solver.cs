using System;
using System.Collections.Generic;
using System.Linq;
using Library;

namespace PixelzatorLibrary
{
    public class Solver
    {
        public const double Eps = 1e-7;
        private Point[] triangle;

        public Solver(Point[] triangle)
        {
            this.triangle = triangle;
        }

        private Point ChangeCoordinateSystem(Point point, double step)
        {
            return new Point(point.X / step, point.Y / step);
        }
        
        public List<Point> CreateListOfRectangles(double step)
        {
            // change coordinate system for triangle
            for (int i = 0; i < triangle.Length; i++)
            {
                triangle[i] = ChangeCoordinateSystem(triangle[i], step);
            }
            
            List<Point> sideSquares = new List<Point>();
            List<Point> middleSquares = new List<Point>();

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
                        sideSquares.Add((Point) slaveCube);
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
                    if (slaveCube != null && Math.Abs(((Point)slaveCube).Y-((Point)mainCube).Y)!=0)
                    {
                        sideSquares.Add((Point) slaveCube);
                        double currentStep = ((Point) mainCube).Y < ((Point) slaveCube).Y ? 1 : -1;
                        for (double y = ((Point) mainCube).Y + currentStep; 
                             Math.Abs(y - ((Point) slaveCube).Y) > Eps ; 
                             y += currentStep) 
                            middleSquares.Add(new Point(((Point) mainCube).X, y));
                    }
                }

                sideSquares.Add((Point) mainCube);
            }

            List<Point> result = new List<Point>();
            List<Point> removedSquares = new List<Point>();
            foreach (var square in sideSquares.Distinct())
            {
                if (BelongsToTriangle(new Point(square.X + 0.5, square.Y + 0.5)))
                {
                    result.Add(square);
                }
                else
                {
                    removedSquares.Add(square);
                }
            }
            
            foreach (var square in middleSquares.Distinct())
            {
                if (!removedSquares.Contains(square))
                {
                    result.Add(square);
                }
            }

            // change coordinate system back (up to now, that is not needed)
            // step = 1 / step;
            // for (int i = 0; i < result.Count; i++)
            // {
            //     result[i] = ChangeCoordinateSystem(result[i], step);
            // }

            return result.Distinct().ToList();
        }

        private bool BelongsToTriangle(Point point)
        {
            double a = (triangle[0].X - point.X) * (triangle[1].Y - triangle[0].Y) - 
                       (triangle[1].X - triangle[0].X) * (triangle[0].Y - point.Y);
            double b = (triangle[1].X - point.X) * (triangle[2].Y - triangle[1].Y) - 
                       (triangle[2].X - triangle[1].X) * (triangle[1].Y - point.Y);
            double c = (triangle[2].X - point.X) * (triangle[0].Y - triangle[2].Y) - 
                       (triangle[0].X - triangle[2].X) * (triangle[2].Y - point.Y);

            if ((a >= 0 && b >= 0 && c >= 0) || (a <= 0 && b <= 0 && c <= 0))
                return true;
            
            return false;
        }
    }
}