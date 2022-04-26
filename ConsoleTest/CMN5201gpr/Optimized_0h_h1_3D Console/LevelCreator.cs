//Author: Dominik Dohmeier
namespace Optimized_0h_h1_3D_Console;

internal class LevelCreator
{
    private Random rnd;
    private RuleChecker ruleChecker;

    public LevelCreator()
    {
        rnd = new Random();
        ruleChecker = RuleChecker.CreateStandardRuleChecker();
    }

    private Board GenerateUniqueLevel(int size)
    {
        Board level = new(size);
        V3Int pos = new();

        int[] placementOrder = new int[level.Length];
        for (int i = 0; i < placementOrder.Length; i++)
            placementOrder[i] = i;
        placementOrder = placementOrder.OrderBy(x => rnd.Next()).ToArray();
        int[] placementAttempts = new int[placementOrder.Length];

        int indexer = 0;
        int posInt = placementOrder[indexer];
        pos.SetFromIndexer(level.SideLength, posInt);

        while (true)
        {
            //TODO both can be do while
            do
            {
                level[pos.X, pos.Y, pos.Z] = placementAttempts[posInt] + 1;
                placementAttempts[posInt]++;

                if(!ruleChecker.CheckRules(level))
                {
                    level[pos.X, pos.Y, pos.Z] = 0; //TODO small optimization here
                    continue;
                }

                indexer++;
                if (indexer == placementOrder.Length)
                    return level;

                posInt = placementOrder[indexer];
                pos.SetFromIndexer(level.SideLength, posInt);
            } 
            while (placementAttempts[posInt] < Rules.COLOR_COUNT) ;

            do
            {
                placementAttempts[posInt] = 0;
                indexer--;
                posInt = placementOrder[indexer];

                pos.SetFromIndexer(level.SideLength, posInt);
                level[pos.X, pos.Y, pos.Z] = 0;             
            }
            while (placementAttempts[posInt] >= Rules.COLOR_COUNT);
        }
    }

    public Board CreateLevel(int size)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        Board level = GenerateUniqueLevel(size);
        return level;
    }

    public Board CreateLevel(int size, int seed)
    {
        rnd = new Random(seed);
        return CreateLevel(size);
    }
}