using System;

namespace PixelzatorLibrary
{
    public class Vector2
    {
        public double X { get; private set; }
        public double Y { get; private set;}
        public double Length { get; }

        public Vector2(Point v1, Point v2)
        {
            X = v2.X - v1.X;
            Y = v2.Y - v1.Y;
            Length = Math.Sqrt(X * X + Y * Y);
        }

        public void Normalize()
        {
            X /= Length;
            Y /= Length;
        }
    }
}