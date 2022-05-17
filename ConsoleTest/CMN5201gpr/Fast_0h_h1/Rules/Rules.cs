﻿//Author: Dominik Dohmeier
namespace Fast_0h_h1
{
    internal static class Rules
    {
        public const int DIMENSIONS = 3;
        public const int MAX_REPETITIONS = 2;
        private static int[][][,] cache;

        private static int maxColorPerLine;
        private static int colorAmount;

        private static int sideLength;
        public static int ColorAmount { get => colorAmount; }

        public static void Initialize(int size, int colorAmount)
        {
            Rules.colorAmount = colorAmount;
            Rules.sideLength = size * colorAmount;

            maxColorPerLine = size;
            cache = new int[DIMENSIONS][][,];
            for (int d = 0; d < DIMENSIONS; d++)
            {
                cache[d] = new int[colorAmount][,];
                for (int c = 0; c < colorAmount; c++)
                    cache[d][c] = new int[sideLength, sideLength];
            }
        }

        public static int[][][,] GetCache() => cache;

        public static int AdjacencyRule(Grid grid)
        {
            V3Int pos = grid.LastEditPos;

            int currentPos;
            int lastColor;
            int color;
            int repetitions;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];
                repetitions = 0;
                lastColor = default;

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = grid[pos];

                    if (color != lastColor)
                        repetitions = 0;
                    if (color != 0)
                        repetitions++;

                    if (repetitions > MAX_REPETITIONS)
                        return -1;

                    lastColor = color;
                }
                pos[d] = currentPos;
            }
            return -1;
        }

        public static int AdjacencyRule(int[,,] grid)
        {
            V3Int pos = new V3Int();

            int lastColor;
            int color;
            int repetitions;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    pos[(d + 1) % DIMENSIONS] = y;
                    for (int z = 0; z < sideLength; z++)
                    {
                        pos[(d + 2) % DIMENSIONS] = z;
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

                            if (repetitions > MAX_REPETITIONS)
                                return (d << 6) + (y << 3) + z;

                            lastColor = color;
                        }
                    }
                }
            }

            return -1;
        }

        public static int EqualCountRule(Grid grid)
        {
            V3Int pos = grid.LastEditPos;
            int[] colorCount = new int[ColorAmount];

            int currentPos;
            int color;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];
                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = grid[pos];

                    if (color != 0)
                        colorCount[color - 1]++;
                }
                pos[d] = currentPos;

                for (int i = 0; i < ColorAmount; i++)
                {
                    if (colorCount[i] > maxColorPerLine)
                        return 0;
                    colorCount[i] = 0;
                }
            }

            return -1;
        }

        public static int EqualCountRule(int[,,] grid)
        {
            int sideLength = grid.GetLength(0);
            V3Int pos = new V3Int();
            int[] colorCount = new int[ColorAmount];

            int color;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    pos[(d + 1) % DIMENSIONS] = y;
                    for (int z = 0; z < sideLength; z++)
                    {
                        pos[(d + 2) % DIMENSIONS] = z;

                        for (int x = 0; x < sideLength; x++)
                        {
                            pos[d] = x;
                            color = grid[pos.X, pos.Y, pos.Z];

                            if (color != 0)
                                colorCount[color - 1]++;
                        }

                        for (int i = 0; i < ColorAmount; i++)
                        {
                            if (colorCount[i] > maxColorPerLine)
                                return (d << 6) + (y << 3) + z;
                            colorCount[i] = 0;
                        }
                    }
                }
            }

            return -1;
        }

        public static int SameLineRule(Grid grid)
        {
            V3Int pos = grid.LastEditPos;

            int currentPos;
            int color;
            int[] currentColorCache = new int[ColorAmount];
            int[] colorCount = new int[ColorAmount];

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = grid[pos];

                    if (color != 0)
                    {
                        colorCount[color - 1]++;
                        currentColorCache[color - 1] += 1 << i;
                    }
                }
                pos[d] = currentPos;

                for (int i = 0; i < ColorAmount; i++)
                {
                    if (colorCount[i] == maxColorPerLine)
                    {
                        if (ContainsInLayer(pos, d, i, currentColorCache[i]))
                            return 0;
                    }

                    colorCount[i] = 0;
                    currentColorCache[i] = 0;
                }
            }
            return -1;
        }

        public static int SameLineRule(int[,,] grid)
        {
            RebuildCache(grid);

            for (int d = 0; d < DIMENSIONS; d++)
            {
                for (int c = 0; c < ColorAmount; c++)
                {
                    for (int xOuter = 0; xOuter < sideLength; xOuter++)
                    {
                        for (int yOuter = 0; yOuter < sideLength; yOuter++)
                        {
                            for (int xInner = xOuter + 1; xInner < sideLength; xInner++)
                            {
                                if (cache[d][c][xOuter, yOuter] == cache[d][c][xInner, yOuter])
                                    return (d << 6) + (xOuter << 3) + yOuter;
                            }
                            for (int yInner = yOuter + 1; yInner < sideLength; yInner++)
                            {
                                if (cache[d][c][xOuter, yOuter] == cache[d][c][xOuter, yInner])
                                    return (d << 6) + (xOuter << 3) + yOuter;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        public static void CacheChanged(Grid grid)
        {
            V3Int pos = grid.LastEditPos;

            int currentPos;
            int color;
            int[] currentColorCache = new int[ColorAmount];
            int[] colorCount = new int[ColorAmount];

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = grid[pos];

                    if (color != 0)
                    {
                        colorCount[color - 1]++;
                        currentColorCache[color - 1] += 1 << i;
                    }
                }
                pos[d] = currentPos;

                for (int i = 0; i < ColorAmount; i++)
                {
                    if (colorCount[i] == maxColorPerLine)
                        cache[d][i][pos[(d + 1) % DIMENSIONS], pos[(d + 2) % DIMENSIONS]] = currentColorCache[i];

                    colorCount[i] = 0;
                    currentColorCache[i] = 0;
                }
            }
        }

        public static void RebuildCache(Grid grid)
        {
            V3Int pos = new V3Int();

            int color;
            int[] currentColorCache = new int[ColorAmount];
            int[] colorCount = new int[ColorAmount];

            for (int d = 0; d < DIMENSIONS; d++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    pos[(d + 1) % DIMENSIONS] = y;
                    for (int z = 0; z < sideLength; z++)
                    {
                        pos[(d + 2) % DIMENSIONS] = z;

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

                        for (int i = 0; i < ColorAmount; i++)
                        {
                            int currentCache = currentColorCache[i];
                            if (colorCount[i] == maxColorPerLine)
                                cache[d][i][pos[(d + 1) % DIMENSIONS], pos[(d + 2) % DIMENSIONS]] = currentCache;
                            else
                                cache[d][i][pos[(d + 1) % DIMENSIONS], pos[(d + 2) % DIMENSIONS]] = 0;
                            colorCount[i] = 0;
                            currentColorCache[i] = 0;
                        }
                    }
                }
            }
        }

        public static void RebuildCache(int[,,] grid)
        {
            V3Int pos = new V3Int();

            int color;
            int[] currentColorCache = new int[ColorAmount];
            int[] colorCount = new int[ColorAmount];

            for (int d = 0; d < DIMENSIONS; d++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    pos[(d + 1) % DIMENSIONS] = y;
                    for (int z = 0; z < sideLength; z++)
                    {
                        pos[(d + 2) % DIMENSIONS] = z;

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

                        for (int i = 0; i < ColorAmount; i++)
                        {
                            int currentCache = currentColorCache[i];
                            if (colorCount[i] == maxColorPerLine)
                                cache[d][i][pos[(d + 1) % DIMENSIONS], pos[(d + 2) % DIMENSIONS]] = currentCache;
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
                if (i != pos[(direction + 2) % DIMENSIONS])
                {
                    cacheValue = cache[direction][color][pos[(direction + 1) % DIMENSIONS], i];
                    if (cacheValue == value)
                        return true;
                }

                if (i != pos[(direction + 1) % DIMENSIONS])
                {
                    cacheValue = cache[direction][color][i, pos[(direction + 2) % DIMENSIONS]];
                    if (cacheValue == value)
                        return true;
                }
            }
            return false;
        }
    }
}