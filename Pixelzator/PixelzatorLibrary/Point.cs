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
        
        public override string ToString()
        {
            return $"Point x: {X} y: {Y}";
        }
    }
}