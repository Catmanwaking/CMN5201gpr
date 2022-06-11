//Author: Dominik Dohmeier
namespace Fast_0h_h1
{
    internal static class FastRules
    {
        private static int[][][,] lineCache;
        private static int[][][,] equalCache;

        private static int maxColorPerLine;

        private static int sideLength;

        public static void Initialize(int size)
        {
            FastRules.sideLength = size * Rules.COLOR_AMOUNT;

            maxColorPerLine = size;
            lineCache = new int[Rules.DIMENSIONS][][,];
            equalCache = new int[Rules.DIMENSIONS][][,];
            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                lineCache[d] = new int[Rules.COLOR_AMOUNT][,];
                equalCache[d] = new int[Rules.COLOR_AMOUNT][,];
                for (int c = 0; c < Rules.COLOR_AMOUNT; c++)
                {
                    lineCache[d][c] = new int[sideLength, sideLength];
                    equalCache[d][c] = new int[sideLength, sideLength];
                }
            }
        }

        public static bool AdjacencyRule(Grid grid)
        {
            if (grid.SideLength == 4 || grid.LastEditColor == 0)
                return false;

            V3Int pos = grid.LastEditPos;

            int currentPos;
            int lastColor;
            int color;
            int repetitions;

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                currentPos = pos[d];
                repetitions = 0;
                lastColor = 0;

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = grid[pos];

                    if (color != lastColor)
                        repetitions = 0;
                    if (color != 0)
                        repetitions++;

                    if (repetitions > Rules.MAX_REPETITIONS)
                        return true;

                    lastColor = color;
                }
                pos[d] = currentPos;
            }
            return false;
        }

        public static bool EqualCountRule(Grid grid)
        {
            if (grid.LastEditColor <= 0)
                return false;

            V3Int pos = grid.LastEditPos;
            int color = grid.LastEditColor - 1;

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                if (equalCache[d][color][pos[(d + 1) % Rules.DIMENSIONS], pos[(d + 2) % Rules.DIMENSIONS]] >= maxColorPerLine)
                    return true;
            }

            return false;
        }

        public static bool SameLineRule(Grid grid)
        {
            if (grid.LastEditColor <= 0)
                return false;

            V3Int pos = grid.LastEditPos;
            int color = grid.LastEditColor;

            int currentPos;
            int colorCount = 0;
            int currentColorCache = 0;

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                currentPos = pos[d];

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;

                    if (grid[pos] == color)
                    {
                        colorCount++;
                        currentColorCache += 1 << i;
                    }
                }
                pos[d] = currentPos;

                if (colorCount == maxColorPerLine)
                {
                    if (ContainsInLayer(pos, d, color, currentColorCache))
                        return true;
                }

                colorCount = 0;
                currentColorCache = 0;
            }
            return false;
        }

        public static void CacheChanged(Grid grid)
        {
            V3Int pos = grid.LastEditPos;
            int color = grid.LastEditColor;

            int currentPos;
            int colorCount = 0;
            int currentColorCache = 0;

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                currentPos = pos[d];

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;

                    if (grid[pos] == color)
                    {
                        colorCount++;
                        currentColorCache += 1 << i;
                    }
                }
                pos[d] = currentPos;

                lineCache[d][color - 1][pos[(d + 1) % Rules.DIMENSIONS], pos[(d + 2) % Rules.DIMENSIONS]] =
                    (colorCount == maxColorPerLine) ? currentColorCache : 0;

                equalCache[d][color - 1][pos[(d + 1) % Rules.DIMENSIONS], pos[(d + 2) % Rules.DIMENSIONS]] =
                    colorCount;

                colorCount = 0;
                currentColorCache = 0;
            }
        }

        public static void RebuildCache(Grid grid)
        {
            V3Int pos = new V3Int();

            int color;
            int[] currentColorCache = new int[Rules.COLOR_AMOUNT];
            int[] colorCount = new int[Rules.COLOR_AMOUNT];

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    pos[(d + 1) % Rules.DIMENSIONS] = y;
                    for (int z = 0; z < sideLength; z++)
                    {
                        pos[(d + 2) % Rules.DIMENSIONS] = z;

                        for (int x = 0; x < sideLength; x++)
                        {
                            pos[d] = x;
                            color = grid[pos];

                            if (color != 0)
                            {
                                colorCount[color - 1]++;
                                currentColorCache[color - 1] += 1 << x;
                            }
                        }

                        for (int i = 0; i < Rules.COLOR_AMOUNT; i++)
                        {
                            int currentCache = currentColorCache[i];

                            lineCache[d][i][pos[(d + 1) % Rules.DIMENSIONS], pos[(d + 2) % Rules.DIMENSIONS]] =
                                (colorCount[i] == maxColorPerLine) ? currentCache : 0;

                            equalCache[d][i][pos[(d + 1) % Rules.DIMENSIONS], pos[(d + 2) % Rules.DIMENSIONS]] =
                                colorCount[i];

                            colorCount[i] = 0;
                            currentColorCache[i] = 0;
                        }
                    }
                }
            }
        }

        private static bool ContainsInLayer(V3Int pos, int direction, int color, int value)
        {
            int cacheValue;
            for (int i = 0; i < sideLength; i++)
            {
                if (i != pos[(direction + 2) % Rules.DIMENSIONS])
                {
                    cacheValue = lineCache[direction][color - 1][pos[(direction + 1) % Rules.DIMENSIONS], i];
                    if (cacheValue == value)
                        return true;
                }

                if (i != pos[(direction + 1) % Rules.DIMENSIONS])
                {
                    cacheValue = lineCache[direction][color - 1][i, pos[(direction + 2) % Rules.DIMENSIONS]];
                    if (cacheValue == value)
                        return true;
                }
            }
            return false;
        }
    }
}