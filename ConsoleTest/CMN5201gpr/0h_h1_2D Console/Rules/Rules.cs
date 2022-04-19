//Author: Dominik Dohmeier
namespace _0h_h1_2D_Console;

public static class Rules
{
    public const int COLOR_COUNT = 2;
    private const int DIM = 2;
    private const int MAX_REPITITIONS = 2;

    /// <summary>
    /// The adjecency rule states that a color in a line cannot repeat 3 times.
    /// </summary>
    /// <param name="board"> The two-dimensional array to check. </param>
    /// <returns> False if the rule is broken. </returns>
    public static bool AdjacencyRule(Board board)
    {
        int color;

        int[] idx;
        int[] repititions = new int[DIM];
        int[] lastColors = new int[DIM];

        for (int x = 0; x < board.SideLength; x++)
        {
            for (int y = 0; y < board.SideLength; y++)
            {
                idx = new int[] { x, y };
                for (int d = 0; d < DIM; d++)
                {
                    color = board[idx[d], idx[(d + 1) % DIM]];
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
        return true;
    }

    /// <summary>
    /// The same line rule states that no two lines in the same direction may be the same.
    /// </summary>
    /// <param name="board"> The two-dimensional array to check. </param>
    /// <returns></returns>
    public static bool SameLineRule(Board board)
    {
        int color;

        int[] idx;
        int[,] cache = new int[board.SideLength, DIM];

        for (int x = 0; x < board.SideLength; x++)
        {
            for (int y = 0; y < board.SideLength; y++)
            {
                idx = new int[] { x, y };
                for (int d = 0; d < DIM; d++)
                {
                    color = board[idx[d], idx[(d + 1) % DIM]];
                    if (color == 0)
                        cache[x, d] = 0;
                    if (y != 0 && cache[x, d] == 0)
                        continue;
                        
                    cache[x, d] <<= 4;
                    cache[x, d] += color;
                }
            }

            for (int i = 0; i < x; i++)
            {
                for (int d = 0; d < DIM; d++)
                {
                    if (cache[i, d] != 0 && cache[i, d] == cache[x, d])
                        return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// The equal count rule states that a line must contain an equal amount of colors.
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public static bool EqualCountRule(Board board)
    {
        int maxColorCount = board.SideLength / COLOR_COUNT;
        int color;

        int[] idx;
        int[,] colorCounts = new int[COLOR_COUNT, DIM];

        for (int x = 0; x < board.SideLength; x++)
        {
            for (int y = 0; y < board.SideLength; y++)
            {
                idx = new int[] { x, y };
                for (int d = 0; d < DIM; d++)
                {
                    color = board[idx[d], idx[(d + 1) % DIM]];
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
        return true;
    }
}
