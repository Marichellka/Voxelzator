using System;
using System.Text;

namespace Voxelzator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Default;
            Solver solver = new Solver();
            Point[] triangle = new Point[] { new Point(0, 0), new Point(2, 6), new Point(4, 1)};
            var list = solver.CreateListOfRectangles(triangle, 1);
            Console.WriteLine(list.Count);
            
            int[,] grid = new int[10, 10];
            
            foreach (Point cube in list)
            {
                grid[(int) cube.y, (int) cube.x] = 1;
                Console.WriteLine(cube);
            }
            
            for (int i = 9; i >= 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(grid[i,j]==0?"⬜️":"⬛️");
                }
                Console.WriteLine();
            }
        }
    }
}