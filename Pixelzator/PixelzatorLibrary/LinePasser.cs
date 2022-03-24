using System;
using PixelzatorLibrary;

namespace Library
{
    public class LinePasser
        {
            private Point _currentPoint;
            private double _count;
            private readonly Vector2 _vector;

            public LinePasser(Point start, Point finish)
            {
                _vector = new Vector2(start, finish);
                _vector.Normalize();
                
                int countX = 0;
                if (_vector.X != 0.0)
                    countX = (int) Math.Ceiling(Math.Abs((start.X - finish.X) / _vector.X));
                int countY = 0;
                if (_vector.Y != 0.0)
                    countY = (int) Math.Ceiling(Math.Abs((start.Y - finish.Y) / _vector.Y));

                _count = Math.Max(countX, countY) + 1;
                
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