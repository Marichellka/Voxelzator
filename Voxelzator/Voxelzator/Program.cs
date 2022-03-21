using System;

namespace Voxelzator
{
    class Program
    {
        static void Main(string[] args)
        {
            Solver solver = new Solver();
            Point[] triangle = new Point[] { new Point(0, 0), new Point(1, 5), new Point(3, 4)};
            var list = solver.CreateListOfRectangles(triangle, 1);
            Console.WriteLine(list.Count);
            
            foreach (Point cube in list)
            {
                Console.WriteLine(cube);
            }
        }
    }
}