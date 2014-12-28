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
        const int P = 16; // number of pieces
        const int T = 4; // number of connectors
        const int R = 4, C = 4;
        const int DIRS = 4;

        const int LEFT = 0;
        const int UP = 1;
        const int RIGHT = 2;
        const int DOWN = 3;

        // W, N, E, S
        //
        // 0: bulb
        // 1: cross
        // 2: out arrow (big)
        // 3: in arrow (small)
        //
        // "T+" indicates indent. Otherwise, it's outdent.
        int[,] pieces =
            new int[P, DIRS] {
                { 0, 2, T+2, T+0 }, // 1
                { 0, 3, T+3, T+2 }, // 2
                { 1, 2, T+1, T+3 }, // 3
                { 3, 0, T+1, T+0 }, // 4

                { 2, 0, T+0, T+1 }, // 5
                { 3, 2, T+2, T+0 }, // 6
                { 3, 3, T+1, T+0 }, // 7
                { 0, 3, T+1, T+3 }, // 8

                { 3, 3, T+2, T+1 }, // 9
                { 2, 2, T+0, T+3 }, // 10
                { 0, 1, T+2, T+2 }, // 11
                { 0, 0, T+0, T+2 }, // 12

                { 1, 2, T+2, T+0 }, // 13
                { 1, 3, T+3, T+2 }, // 14
                { 1, 0, T+0, T+3 }, // 15
                { 1, 0, T+0, T+1 }, // 16
            };
        bool[] used = new bool[P];

        // Flip: reverse connector array _before_ rotation. (Same as mirroring along SW-NE line.)
        // Rot: Number of steps to rotate counter-clockwise.
        int[,] gridPiece = new int[R, C];
        bool[,] gridFlip = new bool[R, C];
        int[,] gridRot = new int[R, C];

        int numSolutions = 0;

        // Get the specified connector of the flipped, rotated piece at the grid point.
        private int getConnector(int r, int c, int dir)
        {
            dir = (dir + gridRot[r, c]) % DIRS;
            if (gridFlip[r, c]) dir = 3 - dir;

            return pieces[gridPiece[r, c], dir];
        }

        private bool connects(int t1, int t2)
        {
            return (t1 + T) % (2 * T) == t2;
        }

        private void rec(int r, int c)
        {
            if (r == R) {
                rec(0, c+1);
                return;
            }

            if (c == C) {
                incSolutions();
                printgrid();
                return;
            }

            for (int p = 0; p < P; ++p)
            {
                if (used[p]) continue;
                used[p] = true;

                gridPiece[r, c] = p;
                for (int flip = 0; flip <= 1; ++flip)
                {
                    gridFlip[r, c] = flip == 1;

                    for (int rot = 0; rot < DIRS; ++rot)
                    {
                        gridRot[r, c] = rot;

                        // TODO: Break symmetry here by requiring particular flip/rot of piece 0.

                        if (r > 0 && !connects(getConnector(r - 1, c, DOWN), getConnector(r, c, UP))) continue;
                        if (c > 0 && !connects(getConnector(r, c - 1, RIGHT), getConnector(r, c, LEFT))) continue;

                        rec(r + 1, c);
                    }
                }

                used[p] = false;
            }
        }

        private void incSolutions()
        {
            ++numSolutions;

            if (numSolutions % 1000 == 0)
            {
                Console.WriteLine("Found solution {0}...", numSolutions);
            }
        }

        private void printgrid()
        {
            for (int r = 0; r < R; ++r)
            {
                for (int c = 0; c < C; ++c)
                {
                    Console.Write(gridPiece[r, c] + ", " + gridFlip[r,c] + ", " + gridRot[r,c] + "   ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.ReadLine();
        }

        public void run()
        {
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
