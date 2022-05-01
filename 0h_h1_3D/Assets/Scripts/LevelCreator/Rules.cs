//Author: Dominik Dohmeier
using System;

namespace Optimized_0h_h1_3D
{
    public static class Rules
    {
        public const int DIMENSIONS = 3;

        private const int MAX_REPITITIONS = 2;

        private static int cacheShiftCount;
        private static ulong[][,] cache;
        private static int colorCount;
        private static int maxColorPerLine;

        public static int ColorCount { get => colorCount;}

        private static bool ManualContains(V3Int pos, int sideLength, int direction, ulong num)
        {
            ulong cacheVal;
            for (int i = 0; i < sideLength; i++)
            {
                cacheVal = cache[direction][pos[(direction + 1) % DIMENSIONS], i];
                if (cacheVal == num)
                    return true;
                cacheVal = cache[direction][i, pos[(direction + 2) % DIMENSIONS]];
                if (cacheVal == num)
                    return true;
            }
            return false;
        }

        public static bool AdjacencyRule(Board board)
        {
            int sideLength = board.SideLength;
            V3Int pos = board.LastEditPos;

            int currentPos;
            int lastColor;
            int color;
            int repititions;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];
                repititions = 0;
                lastColor = default;

                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = board[pos];

                    if (color != lastColor)
                        repititions = 0;
                    if (color != 0)
                        repititions++;

                    if (repititions > MAX_REPITITIONS)
                        return false;

                    lastColor = color;
                }
                pos[d] = currentPos;
            }
            return true;
        }

        public static bool EqualCountRule(Board board)
        {
            int sideLength = board.SideLength;
            V3Int pos = board.LastEditPos;
            int[] colorCounts = new int[colorCount];

            int currentPos;
            int color;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];
                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = board[pos];

                    if (color != 0)
                        colorCounts[color - 1]++;
                }
                pos[d] = currentPos;

                for (int i = 0; i < colorCount; i++)
                {
                    if (colorCounts[i] > maxColorPerLine)
                        return false;
                    colorCounts[i] = 0;
                }
            }
            return true;
        }

        /// <summary>
        /// THIS FUNCTION ASSUMES THAT THE CACHE IS MANAGED CORRECTLY ELSEWHERE IF FULL LINES ARE REMOVED!
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static bool SameLineRule(Board board)
        {
            int sideLength = board.SideLength;
            V3Int pos = board.LastEditPos;

            int currentPos;
            int color;
            ulong currentCache;

            for (int d = 0; d < DIMENSIONS; d++)
            {
                currentPos = pos[d];
                currentCache = 0;
                for (int i = 0; i < sideLength; i++)
                {
                    pos[d] = i;
                    color = board[pos];

                    if (color == 0)
                    {
                        currentCache = 0;
                        break;
                    }

                    currentCache <<= cacheShiftCount;
                    currentCache += (ulong)color;
                }
                pos[d] = currentPos;

                if (currentCache == 0)
                    continue;

                if (ManualContains(pos, sideLength, d, currentCache))
                    return false;

                //add to cache now, since it is new and full?
                cache[d][pos[(d + 1) % DIMENSIONS], pos[(d + 2) % DIMENSIONS]] = currentCache;
            }
            return true;
        }

        public static void SetupRules(int size, int colorCount = 2)
        {
            int sideLength = size * colorCount;
            Rules.colorCount = colorCount;
            cacheShiftCount = (int)Math.Ceiling(Math.Log((colorCount + 1), 2));
            maxColorPerLine = sideLength / Rules.colorCount;

            cache = new ulong[DIMENSIONS][,];
            for (int i = 0; i < DIMENSIONS; i++)
                cache[i] = new ulong[sideLength, sideLength];
        }

        public static ulong[][,] GetCache()
        {
            return cache;
        }
    }
}