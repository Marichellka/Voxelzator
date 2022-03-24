using System;
using System.Text;
using PixelzatorLibrary;

namespace Pixelzator
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Default;
            Point[] triangle = new Point[] { new Point(1, 1), new Point(3, 7), new Point(5, 2)};
            Solver solver = new Solver(triangle);
            var list = solver.CreateListOfRectangles(1);
            Console.WriteLine(list.Count);
            
            int[,] grid = new int[10, 10];
            
            foreach (Point cube in list)
            {
                grid[(int) cube.Y, (int) cube.X] = 1;
                Console.WriteLine(cube);
            }
            
            for (int i = 9; i >= 0; i--)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write(grid[i,j]==0?"⬜":"⬛");
                }
                Console.WriteLine();
            }
        }
    }
}