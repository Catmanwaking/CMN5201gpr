//Author: Dominik Dohmeier
namespace _0h_h1_3D_Console
{
    public static class Rules3D
    {
        private const int COLOR_COUNT = 2;
        private const int DIM = 3; //Dimensions
        private const int MAX_REPITITIONS = 2;

        /// <summary>
        /// The adjecency rule states that a color in a line cannot repeat 3 times.
        /// </summary>
        /// <param name="board"> The two-dimensional array to check. </param>
        /// <returns> False if the rule is broken. </returns>
        public static bool AdjacencyRule(int[,,] board)
        {
            int sideLength = board.GetLength(0);
            int color;

            int[] idx;
            int[] repititions = new int[DIM];
            int[] lastColors = new int[DIM];

            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    for (int z = 0; z < sideLength; z++)
                    {
                        idx = new int[] { x, y, z };
                        for (int d = 0; d < DIM; d++)
                        {
                            color = board[idx[d], idx[(d + 1) % DIM], idx[(d + 2) % DIM]];

                            if (color != lastColors[d])
                                repititions[d] = 0;
                            if (color != 0)
                                repititions[d]++;

                            if (repititions[d] > MAX_REPITITIONS)
                                return false;

                            lastColors[d] = color;
                        }
                    }
                    for (int i = 0; i < DIM; i++)
                    {
                        repititions[i] = 0;
                        lastColors[i] = 0;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// The same line rule states that no two lines 
        /// in the same direction on the same plane may be the same.
        /// </summary>
        /// <param name="board"> The two-dimensional array to check. </param>
        /// <returns></returns>
        public static bool SameLineRule(int[,,] board)
        {
            int sideLength = board.GetLength(0);
            int color;

            int[] idx;
            int[,] cache = new int[sideLength, DIM];

            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    for (int z = 0; z < sideLength; z++)
                    {
                        idx = new int[] { x, y };
                        for (int d = 0; d < DIM; d++)
                        {
                            color = board[idx[d], idx[(d + 1) % DIM], idx[(d + 1) % DIM]];

                            cache[y, d] <<= 4;
                            cache[y, d] += color;
                        }
                    }

                    for (int i = 0; i < x; i++)
                    {
                        for (int d = 0; d < DIM; d++)
                        {
                            if (cache[i, d] == cache[y, d])
                                return false;
                        }
                    } 
                }
                cache = new int[sideLength, DIM];
            }

            return true;
        }

        /// <summary>
        /// The equal count rule states that a line must contain an equal amount of colors.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static bool EqualCountRule(int[,,] board)
        {
            int sideLength = board.GetLength(0);
            int maxColorCount = sideLength / COLOR_COUNT;
            int color;

            int[] idx;
            int[,] colorCounts = new int[COLOR_COUNT, DIM];

            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    for (int z = 0; z < sideLength; z++)
                    {
                        idx = new int[] { x, y, z };
                        for (int d = 0; d < DIM; d++)
                        {
                            color = board[idx[d], idx[(d + 1) % DIM], idx[(d + 2) % DIM]];
                            if (color != 0)
                                colorCounts[color - 1, d]++;
                        }
                    }

                    for (int i = 0; i < COLOR_COUNT; i++)
                    {
                        for (int j = 0; j < DIM; j++)
                        {
                            if (colorCounts[i, j] > maxColorCount)
                                return false;
                            colorCounts[i, j] = 0;
                        }
                    }
                }
            }

            return true;
        }
    }
}