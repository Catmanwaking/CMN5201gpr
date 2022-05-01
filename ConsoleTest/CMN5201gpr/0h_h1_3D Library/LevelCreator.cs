namespace _0h_h1_3D_Library;

public class LevelCreator
{
    private Random rnd;
    private RuleChecker ruleChecker;

    public LevelCreator()
    {
        rnd = new Random();
        ruleChecker = RuleChecker.CreateStandardRuleChecker();
    }

    private Board GenerateUniqueLevel(int size, int colorCount)
    {
        Rules.Initialize(size, colorCount);

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
            do
            {
                level[pos] = placementAttempts[posInt] + 1;
                placementAttempts[posInt]++;

                if (!ruleChecker.CheckRules(level))
                {
                    level[pos] = 0; //TODO small optimization here
                    continue;
                }

                indexer++;
                if (indexer == placementOrder.Length)
                    return level;

                posInt = placementOrder[indexer];
                pos.SetFromIndexer(level.SideLength, posInt);
            }
            while (placementAttempts[posInt] < Rules.ColorCount);

            do
            {
                placementAttempts[posInt] = 0;
                indexer--;
                posInt = placementOrder[indexer];

                pos.SetFromIndexer(level.SideLength, posInt);
                level[pos] = 0;
            }
            while (placementAttempts[posInt] >= Rules.ColorCount);
        }
    }

    public int[,,] CreateLevel(int size, int colorCount)
    {
        Board level = GenerateUniqueLevel(size, colorCount);

        return level.ExportGrid();
    }

    public int[,,] CreateLevel(int size, int seed, int colorCount)
    {
        rnd = new Random(seed);
        return CreateLevel(size, colorCount);
    }
}