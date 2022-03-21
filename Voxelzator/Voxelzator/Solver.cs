using System;
using System.Collections.Generic;
using System.Linq;
using Voxelzator;
using Microsoft.CodeAnalysis;

namespace Voxelzator
{
    public class Solver
    {
        private const double Eps = 1e-7;

        class LinePasser
        {
            private Point _currentPoint;
            private readonly Point _lastPoint;
            private readonly double _xGridStep;
            private readonly double _yGridStep;
            private double _xNextBorder;
            private double _yNextBorder;
            private readonly double _dx;
            private readonly double _dy;

            public LinePasser(Point start, Point finish, double step)
            {
                _currentPoint = start;
                _lastPoint = finish;
                _dx = finish.x - start.x;
                _dy = finish.y - start.y;

                _xGridStep = finish.x > start.x ? step : -step;
                _yGridStep = finish.y > start.y ? step : -step;

                //Should I improve this formula?
                _xNextBorder = (Math.Round((start.x - _xGridStep / 2) / _xGridStep) + 0.5) * _xGridStep +
                               _xGridStep / 2;
                _yNextBorder = (Math.Round((start.y - _yGridStep / 2) / _yGridStep) + 0.5) * _yGridStep +
                               _yGridStep / 2;
            }

            static double LengthTillBorder(double current, double border, double speed)
            {
                return (border - current) / speed;
            }

            public Point? NextStep()
            {
                if ((_lastPoint.x - _currentPoint.x) * _dx <= Eps || (_lastPoint.y - _currentPoint.y) * _dy <= Eps)
                    return null;

                var returnPoint = new Point(_xNextBorder - _xGridStep / 2, _yNextBorder - _yGridStep / 2);

                double xOffset = LengthTillBorder(_currentPoint.x, _xNextBorder, _dx);
                double yOffset = LengthTillBorder(_currentPoint.y, _yNextBorder, _dy);

                if (xOffset < yOffset)
                {
                    _currentPoint.x = _xNextBorder;
                    _xNextBorder += _xGridStep;
                    _currentPoint.y += xOffset * _dy;
                }
                else
                {
                    _currentPoint.y = _yNextBorder;
                    _yNextBorder += _yGridStep;
                    _currentPoint.x += yOffset * _dx;
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