//Author: Dominik Dohmeier
using Extensions;

namespace _0h_h1_3D_Console;

internal class LevelCreator
{
    private readonly BruteForceSolver solver;
    private Random rnd;

    public LevelCreator()
    {
        rnd = new Random();
        solver = new BruteForceSolver();
    }

    private void RemoveUnecessaryTiles(Board level)
    {
        int setTileCount = 0;
        foreach (int tile in level)
        {
            if (tile != 0)
                setTileCount++;
        }

        int[] order = new int[level.Length];
        for (int i = 0; i < order.Length; i++)
            order[i] = i;
        order = order.OrderBy(x => rnd.Next()).ToArray();

        for (int i = 0; i < order.Length; i++)
        {
            int pos = order[i];
            int posX = Maths.ConvertIndexer(0, level.SideLength, pos);
            int posY = Maths.ConvertIndexer(1, level.SideLength, pos);
            int posZ = Maths.ConvertIndexer(2, level.SideLength, pos);
            if (level[posX, posY, posZ] == 0)
                continue;

            int currentColor = level[posX, posY, posZ];
            level[posX, posY, posZ] = 0;
            if (solver.CountSolutions(level) != 1)
                level[posX, posY, posZ] = currentColor;
        }
    }

    private Board GenerateUniqueLevel(int size)
    {
        Board level = new(size);

        int[] order = new int[level.Length];
        for (int i = 0; i < order.Length; i++)
            order[i] = i;
        order = order.OrderBy(x => rnd.Next()).ToArray();

        for (int i = 0; i < order.Length; i++)
        {
            int pos = order[i];
            int posX = Maths.ConvertIndexer(0, level.SideLength, pos);
            int posY = Maths.ConvertIndexer(1, level.SideLength, pos);
            int posZ = Maths.ConvertIndexer(2, level.SideLength, pos);
            int nextColor = rnd.Next(Rules.COLOR_COUNT) + 1;

            for (int j = 0; j < Rules.COLOR_COUNT; j++)
            {
                level[posX, posY, posZ] = nextColor;
                int solutionCount = solver.CountSolutions(level);

                if (solutionCount == 0)
                    nextColor = (nextColor % Rules.COLOR_COUNT) + 1;
                else if (solutionCount == 1)
                    return level;
                else
                    break;
            }
        }
        return level;
    }

    public Board CreateLevel(int size)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        Board level = GenerateUniqueLevel(size);
        RemoveUnecessaryTiles(level);

        return level;
    }

    public Board CreateLevel(int size, int seed)
    {
        rnd = new Random(seed);
        return CreateLevel(size);
    }
}