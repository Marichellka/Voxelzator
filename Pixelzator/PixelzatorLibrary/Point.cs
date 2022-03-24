namespace PixelzatorLibrary
{
    public struct Point
    {
        public double X{ get;}
        public double Y { get;}

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj.GetType() != typeof(Point))
                return false;
            
            if (((Point)obj).X == this.X && ((Point)obj).Y == this.Y)
                return true;
            
            return false;
        }

        public override string ToString()
        {
            return $"Point x: {X} y: {Y}";
        }
    }
}