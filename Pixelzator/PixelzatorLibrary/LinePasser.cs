using System;
using PixelzatorLibrary;

namespace Library
{
    public class LinePasser
        {
            private Point _currentPoint;
            private int _count;
            private Vector2 _vector;

            public LinePasser(Point start, Point finish, double step)
            {
                _count = (int)Math.Ceiling(Math.Max(Math.Abs(start.X - finish.X),
                    Math.Abs(start.Y - finish.Y))) + 1;

                _vector = new Vector2(start, finish);
                _vector.Normalize();
                
                _currentPoint = new Point(start.X - _vector.X, 
                    start.Y - _vector.Y);
            }

            public Point? NextStep()
            {
                if (_count == 0)
                    return null;

                _count--;
                _currentPoint = new Point(_currentPoint.X + _vector.X, 
                    _currentPoint.Y + _vector.Y);
                return new Point(Math.Round(_currentPoint.X), 
                    Math.Round(_currentPoint.Y));
            }
        }
}