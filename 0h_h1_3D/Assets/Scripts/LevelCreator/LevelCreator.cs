//Author: Dominik Dohmeier
using System;
using System.Linq;

namespace Optimized_0h_h1_3D
{
    internal class LevelCreator
    {
        private Random rnd;
        private RuleChecker ruleChecker;

        public LevelCreator()
        {
            rnd = new Random();
            ruleChecker = RuleChecker.CreateStandardRuleChecker();
        }

        private GameGrid GenerateUniqueLevel(int size, int colorCount)
        {
            BruteForceGrid level = new(size, colorCount);
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

        public GameGrid CreateLevel(int size, int colorCount)
        {
            GameGrid level = GenerateUniqueLevel(size, colorCount);

            return level;
        }

        public GameGrid CreateLevel(int size, int seed, int colorCount)
        {
            rnd = new Random(seed);
            return CreateLevel(size, colorCount);
        }
    }
}