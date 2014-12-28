using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JigsawPuzzleSolver
{
    class Program
    {
        const int P = 16;
        const int R = 4, C = 4;
        const int DIRS = 4;

        // W, N, E, S
        //
        // 0: bulb
        // 1: cross
        // 2: out arrow (big)
        // 3: in arrow (small)
        int[,] pieces =
            new int[P, DIRS] {
                { 0, 2, 2, 0 }, // 1
                { 0, 3, 3, 2 }, // 2
                { 1, 2, 1, 3 }, // 3
                { 3, 0, 1, 0 }, // 4

                { 2, 0, 0, 1 }, // 5
                { 3, 2, 2, 0 }, // 6
                { 3, 3, 1, 0 }, // 7
                { 0, 3, 1, 3 }, // 8

                { 3, 3, 2, 1 }, // 9
                { 2, 2, 0, 3 }, // 10
                { 0, 1, 2, 2 }, // 11
                { 0, 0, 0, 2 }, // 12

                { 1, 2, 2, 0 }, // 13
                { 1, 3, 3, 2 }, // 14
                { 1, 0, 0, 3 }, // 15
                { 1, 0, 0, 1 }, // 16
            };
        bool[] used = new bool[P];

        // original:  1,  2, ...,  16
        // flipped:  -1, -2, ..., -16
        int[,] grid = new int[R, C];

        int numSolutions = 0;

        private bool connects(int dir, int p1, int p2)
        {
            Debug.Assert(2 <= dir && dir < DIRS);
            Debug.Assert(-16 <= p1 && p1 <= -1 || 1 <= p1 && p1 <= 16);
            Debug.Assert(-16 <= p2 && p2 <= -1 || 1 <= p2 && p2 <= 16);

            int d1 = dir;
            if (p1 < 0)
            {
                p1 *= -1;
                if (d1 == 2) d1 = 3;
                else
                {
                    Debug.Assert(d1 == 3);
                    d1 = 2;
                }
            }

            int d2 = dir - 2;
            if (p2 < 0)
            {
                p2 *= -1;
                if (d2 == 0) d2 = 1;
                else
                {
                    Debug.Assert(d2 == 1);
                    d2 = 0;
                }
            }

            return pieces[p1 - 1, d1] == pieces[p2 - 1, d2];
        }

        private void rec(int r, int c)
        {
            if (r == R) {
                rec(0, c+1);
                return;
            }

            if (c == C) {
                ++numSolutions;
                printgrid();
                return;
            }

            for (int p = 0; p < P; ++p)
            {
                if (used[p]) continue;
                used[p] = true;

                for (int flip = -1; flip <= 1; flip += 2)
                {
                    grid[r, c] = flip * (p + 1);

                    if (r > 0 && !connects(3, grid[r - 1, c], grid[r, c])) continue;
                    if (c > 0 && !connects(2, grid[r, c - 1], grid[r, c])) continue;

                    rec(r + 1, c);
                }

                used[p] = false;
            }
            grid[r, c] = 0;
        }

        private void printgrid()
        {
            for (int r = 0; r < R; ++r)
            {
                for (int c = 0; c < C; ++c)
                {
                    Console.Write(grid[r, c] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void run()
        {
            // TODO: This solver is currently restricted to solutions where the left and top edges
            // are fully positive and the right and bottom edges are fully negative.
            rec(0, 0);

            Console.WriteLine("Total solutions found:  " + numSolutions);
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            new Program().run();
        }
    }
}
