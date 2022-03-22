using System;
using PixelzatorLibrary;

namespace Library
{
    public class LinePasser
        {
            private Point _currentPoint;
            private double _count;
            private readonly Vector2 _vector;
            private readonly double _step;

            public LinePasser(Point start, Point finish, double step)
            {
                _step = step*0.9999;
                _count = Math.Max(Math.Abs(Math.Floor(start.X) - Math.Ceiling(finish.X)),
                    Math.Abs(Math.Floor(start.Y) - Math.Ceiling(finish.Y)))/_step + 1;

                _vector = new Vector2(start, finish);
                _vector.Normalize();
                
                _currentPoint = new Point(start.X - _vector.X*_step, 
                    start.Y - _vector.Y*_step);
            }

            public Point? NextStep()
            {
                if (_count <= Solver.Eps)
                    return null;

                _count--;
                _currentPoint = new Point(_currentPoint.X + _vector.X*_step, 
                    _currentPoint.Y + _vector.Y*_step);
                double current_stepx = Math.Floor(_currentPoint.X / _step)*_step;
                double current_stepy = Math.Floor(_currentPoint.Y / _step)*_step;
                if (_currentPoint.X - current_stepx >= _step / 2)
                {
                    current_stepx += _step;
                }
                if (_currentPoint.Y - current_stepy >= _step / 2)
                {
                    current_stepy += _step;
                }
                return new Point(current_stepx, current_stepy);
            }
        }
}