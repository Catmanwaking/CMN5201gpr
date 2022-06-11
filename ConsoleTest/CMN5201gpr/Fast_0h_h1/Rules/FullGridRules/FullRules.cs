//Author: Dominik Dohmeier
namespace Fast_0h_h1
{
    internal static class FullRules
    {
        private static int[][][,] lineCache;
        private static int maxColorPerLine;
        private static int sideLength;

        public static void Initialize(int size)
        {
            sideLength = size * Rules.COLOR_AMOUNT;

            maxColorPerLine = size;
            lineCache = new int[Rules.DIMENSIONS][][,];
            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                lineCache[d] = new int[Rules.COLOR_AMOUNT][,];
                for (int c = 0; c < Rules.COLOR_AMOUNT; c++)
                    lineCache[d][c] = new int[sideLength, sideLength];
            }
        }

        public static RuleInfo AdjacencyRule(int[,,] grid)
        {
            V3Int pos = new V3Int();

            int lastColor;
            int color;
            int repetitions;

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    pos[(d + 1) % Rules.DIMENSIONS] = y;
                    for (int z = 0; z < sideLength; z++)
                    {
                        pos[(d + 2) % Rules.DIMENSIONS] = z;
                        repetitions = 0;
                        lastColor = default;

                        for (int x = 0; x < sideLength; x++)
                        {
                            pos[d] = x;
                            color = grid[pos.X, pos.Y, pos.Z];

                            if (color != lastColor)
                                repetitions = 0;
                            if (color != 0)
                                repetitions++;

                            if (repetitions > Rules.MAX_REPETITIONS)
                                return new RuleInfo(Rule.Adjacency, d, y, z);

                            lastColor = color;
                        }
                    }
                }
            }

            return new RuleInfo();
        }

        public static RuleInfo EqualCountRule(int[,,] grid)
        {
            int sideLength = grid.GetLength(0);
            V3Int pos = new V3Int();
            int[] colorCount = new int[Rules.COLOR_AMOUNT];

            int color;

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
                            color = grid[pos.X, pos.Y, pos.Z];

                            if (color != 0)
                                colorCount[color - 1]++;
                        }

                        for (int i = 0; i < Rules.COLOR_AMOUNT; i++)
                        {
                            if (colorCount[i] > maxColorPerLine)
                                return new RuleInfo(Rule.EqualCount, d, y, z);
                            colorCount[i] = 0;
                        }
                    }
                }
            }

            return new RuleInfo();
        }

        public static RuleInfo SameLineRule(int[,,] grid)
        {
            RebuildCache(grid);

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                for (int c = 0; c < Rules.COLOR_AMOUNT; c++)
                {
                    for (int xOuter = 0; xOuter < sideLength; xOuter++)
                    {
                        for (int yOuter = 0; yOuter < sideLength; yOuter++)
                        {
                            if (lineCache[d][c][xOuter, yOuter] == 0)
                                continue;
                            for (int xInner = xOuter + 1; xInner < sideLength; xInner++)
                            {
                                if (lineCache[d][c][xOuter, yOuter] == lineCache[d][c][xInner, yOuter])
                                    return new RuleInfo(Rule.SameLine, d, xInner, yOuter);
                            }
                            for (int yInner = yOuter + 1; yInner < sideLength; yInner++)
                            {
                                if (lineCache[d][c][xOuter, yOuter] == lineCache[d][c][xOuter, yInner])
                                    return new RuleInfo(Rule.SameLine, d, xOuter, yInner);
                            }
                        }
                    }
                }
            }

            return new RuleInfo();
        }

        public static void RebuildCache(int[,,] grid)
        {
            V3Int pos = new V3Int();
            maxColorPerLine = grid.GetLength(0) >> 1;

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
                            color = grid[pos.X, pos.Y, pos.Z];

                            if (color != 0)
                            {
                                colorCount[color - 1]++;
                                currentColorCache[color - 1] += 1 << x;
                            }
                        }

                        for (int i = 0; i < Rules.COLOR_AMOUNT; i++)
                        {
                            int currentCache = currentColorCache[i];
                            lineCache[d][i][pos[(d + 1) % Rules.DIMENSIONS],pos[(d + 2) % Rules.DIMENSIONS]] = 
                                (colorCount[i] == maxColorPerLine) ? currentCache : 0;
                            colorCount[i] = 0;
                            currentColorCache[i] = 0;
                        }
                    }
                }
            }
        }
    }
}