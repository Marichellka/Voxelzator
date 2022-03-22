using System;
using PixelzatorLibrary;

namespace Library
{
    public class LinePasser
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
                _dx = finish.X - start.X;
                _dy = finish.Y - start.Y;

                _xGridStep = finish.X > start.X ? step : -step;
                _yGridStep = finish.Y > start.Y ? step : -step;

                //Should I improve this formula?
                _xNextBorder = (Math.Round((start.X - _xGridStep / 2) / _xGridStep) + 0.5) * _xGridStep +
                               _xGridStep / 2;
                _yNextBorder = (Math.Round((start.Y - _yGridStep / 2) / _yGridStep) + 0.5) * _yGridStep +
                               _yGridStep / 2;
            }

            static double LengthTillBorder(double current, double border, double speed)
            {
                return (border - current) / speed;
            }

            public Point? NextStep()
            {
                if ((_lastPoint.X - _currentPoint.X) * _dx <= Solver.Eps && (_lastPoint.Y - _currentPoint.Y) * _dy <= Solver.Eps)
                    return null;

                var returnPoint = new Point(_xNextBorder - _xGridStep / 2, _yNextBorder - _yGridStep / 2);

                if (Math.Abs(_dx) < Solver.Eps)
                {
                    _currentPoint.Y = _yNextBorder;
                    _yNextBorder += _yGridStep;
                    return returnPoint;
                }
                if (Math.Abs(_dy) < Solver.Eps)
                {
                    _currentPoint.X = _xNextBorder;
                    _xNextBorder += _xGridStep;
                    return returnPoint;
                }
                
                double xOffset = LengthTillBorder(_currentPoint.X, _xNextBorder, _dx);
                double yOffset = LengthTillBorder(_currentPoint.Y, _yNextBorder, _dy);

                if (xOffset < yOffset)
                {
                    _currentPoint.X = _xNextBorder;
                    _xNextBorder += _xGridStep;
                    _currentPoint.Y += xOffset * _dy;
                }
                else
                {
                    _currentPoint.Y = _yNextBorder;
                    _yNextBorder += _yGridStep;
                    _currentPoint.X += yOffset * _dx;
                }

                return returnPoint;
            }
        }
}