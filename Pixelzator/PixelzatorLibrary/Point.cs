namespace PixelzatorLibrary
{
    public struct Point
    {
        public double X{ get; set; }
        public double Y { get; set;}

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        
        public static bool operator < (Point vertex1, Point vertex2)
        {
            return vertex1.X < vertex2.X && vertex1.Y < vertex2.Y;
        }

        public static bool operator >(Point vertex1, Point vertex2)
        {
            return vertex1.X > vertex2.X && vertex1.Y > vertex2.Y;
        }
        
        public override string ToString()
        {
            return $"Point x: {X} y: {Y}";
        }
    }
}