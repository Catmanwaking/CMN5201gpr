//Author: Dominik Dohmeier
namespace Optimized_0h_h1_3D_Console;

public static class Rules
{
    private const int MAX_REPITITIONS = 2;
    private static int[][,] cache;
    private static int maxColorPerLine;
    public const int COLOR_COUNT = 2;
    public const int DIMENSIONS = 3;
    private const int CACHE_SHIFT_COUNT = 2; //TODO

    private static bool ManualContains(V3Int pos, int sideLength, int direction, int num)
    {
        int cacheVal;
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

    public static int[][,] SetupCache(int sideLength)
    {
        maxColorPerLine = sideLength / COLOR_COUNT;
        cache = new int[DIMENSIONS][,];
        for (int i = 0; i < DIMENSIONS; i++)
            cache[i] = new int[sideLength, sideLength];
        return cache;
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
        int[] colorCounts = new int[COLOR_COUNT];

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

            for (int i = 0; i < COLOR_COUNT; i++)
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
        int currentCache;

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

                currentCache <<= CACHE_SHIFT_COUNT;
                currentCache += color;
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
}