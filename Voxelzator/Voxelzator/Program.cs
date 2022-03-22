using System;

namespace Voxelzator
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver solver = new Solver();
            Point[] triangle = new Point[] { new Point(1, 1), new Point(1, 6), new Point(4, 1)};
            var list = solver.CreateListOfRectangles(triangle, 0.5);
            Console.WriteLine(list.Count);
            
            foreach (Point cube in list)
            {
                Console.WriteLine(cube);
            }
        }
    }
}