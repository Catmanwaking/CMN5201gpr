//Author: Dominik Dohmeier
using System;
using System.Linq;

namespace Fast_0h_h1
{
    public class LevelCreator
    {
        private const int DEFAULT_COLOR_COUNT = 2;
        private const int DEFAULT_SIZE = 2;
        private readonly FastRuleChecker ruleChecker;
        private Random rand = new Random();

        private int size;

        private bool restartGeneration;
        private int[,,] lastGeneratedGrid;

        public LevelCreator() : this(DEFAULT_SIZE)
        {
        }

        public LevelCreator(int size, int colorCount = DEFAULT_COLOR_COUNT)
        {
            ChangeSettings(size, colorCount);

            rand = new Random();
            ruleChecker = new FastRuleChecker();
        }

        public void ChangeSettings(int size, int colorCount = DEFAULT_COLOR_COUNT)
        {
            this.size = size;
            Rules.Initialize(size, colorCount);
        }

        public int[,,] GenerateLevel(int seed)
        {
            rand = new Random(seed);
            return GenerateLevel();
        }

        public int[,,] GenerateLevel()
        {
            Grid grid;
            do
            {
                grid = GenerateFull();
            } while (restartGeneration);
            
            lastGeneratedGrid = Reduce(grid);

            return lastGeneratedGrid;
        }

        public int[,,] RegenerateLastLevel()
        {
            if (lastGeneratedGrid == null)
                lastGeneratedGrid = GenerateLevel();
            return lastGeneratedGrid;
        }

        private Grid GenerateFull()
        {
            restartGeneration = false;

            Grid grid = new Grid(size);
            V3Int pos = new V3Int();

            Rules.RebuildCache(grid);

            int[] placementOrder = new int[grid.Length];
            for (int i = 0; i < placementOrder.Length; i++)
                placementOrder[i] = i;
            placementOrder = placementOrder.OrderBy(x => rand.Next()).ToArray();

            int indexer = 0;

            //place the first two, ignoring rules
            int posInt = placementOrder[indexer++];
            pos.SetFromIndexer(grid.SideLength, posInt);
            grid[pos] = 1;
            posInt = placementOrder[indexer++];
            pos.SetFromIndexer(grid.SideLength, posInt);
            grid[pos] = 1;

            do
            {
                //check each tile that has not been set with a mock move.
                //if all but one color break the rules, place the one color
                if (SingleCheck(grid))
                    continue;

                //stop if unsolvable grid emerged (rejection sampling)
                if (restartGeneration)
                    return null;

                //if no color is placable, check by finishing the entire row along with the placed tile
                //if all but one color break the rules, place the one color
                if (RowCheck(grid))
                    continue;

                while (indexer < placementOrder.Length)
                {
                    posInt = placementOrder[indexer++];
                    pos.SetFromIndexer(grid.SideLength, posInt);
                    if (grid[pos] == 0)
                    {
                        grid[pos] = 1;
                        Rules.CacheChanged(grid);
                        break;
                    }
                }
            } while (indexer < placementOrder.Length);

            return grid;
        }

        private int[,,] Reduce(Grid grid)
        {
            int[,,] reducedGrid = new int[grid.SideLength, grid.SideLength, grid.SideLength];
            V3Int pos = new V3Int();

            int[] removalOrder = new int[grid.Length];
            for (int i = 0; i < removalOrder.Length; i++)
                removalOrder[i] = i;
            removalOrder = removalOrder.OrderBy(x => rand.Next()).ToArray();           

            foreach (int item in removalOrder)
            {
                restartGeneration = false;
                pos.SetFromIndexer(grid.SideLength, item);
                int currentColor = grid[pos];

                Grid testGrid = new Grid(grid);
                testGrid[pos] = (currentColor % 2) + 1;
                Rules.RebuildCache(testGrid);

                if(ruleChecker.CheckRules(testGrid, out _) != -1)
                    restartGeneration = true;

                while (!restartGeneration)
                {
                    if (SingleCheck(testGrid))
                        continue;
                    if (restartGeneration)
                        continue;
                    if (RowCheck(testGrid))
                        continue;

                    reducedGrid[pos.X, pos.Y, pos.Z] = currentColor;
                    break;
                } 
                if (restartGeneration)
                    grid[pos] = 0;
            }
            return reducedGrid;
        }

        private bool SingleCheck(Grid grid)
        {
            bool change = false;
            for (int x = 0; x < grid.SideLength; x++)
            {
                for (int y = 0; y < grid.SideLength; y++)
                {
                    for (int z = 0; z < grid.SideLength; z++)
                    {
                        if (grid[x, y, z] != 0)
                            continue;

                        int color = CheckAllColors(grid, x, y, z);

                        if (color == 0)
                        {
                            restartGeneration = true;
                            return false;
                        }

                        if (color == -1)
                            continue;

                        grid[x, y, z] = color;
                        Rules.CacheChanged(grid);
                        change = true;
                    }
                }
            }

            return change;
        }

        private bool RowCheck(Grid grid)
        {
            int checkCount = grid.Size - 1;
            if (checkCount == 1)
                return false;

            bool change = false;

            V3Int pos = new V3Int();

            int color;
            int[] colorCount = new int[Rules.ColorAmount];
            int[] line = new int[grid.SideLength];

            for (int d = 0; d < Rules.DIMENSIONS; d++)
            {
                for (int y = 0; y < grid.SideLength; y++)
                {
                    pos[(d + 1) % Rules.DIMENSIONS] = y;
                    for (int z = 0; z < grid.SideLength; z++)
                    {
                        pos[(d + 2) % Rules.DIMENSIONS] = z;
                        for (int x = 0; x < grid.SideLength; x++)
                        {
                            pos[d] = x;
                            color = grid[pos];

                            if (color == 0)
                                continue;

                            colorCount[color - 1]++;
                        }

                        for (int c = 0; c < Rules.ColorAmount; c++)
                        {
                            if (colorCount[c] == checkCount)
                            {
                                for (int x = 0; x < grid.SideLength; x++)
                                {
                                    pos[d] = x;
                                    line[x] = grid[pos];
                                }
                                int col = SingleLineBruteForce(line, c + 1);
                                if (col != -1)
                                {
                                    pos[d] = col;
                                    grid[pos] = ((c + 1) % 2) + 1;
                                    change = true;
                                    Rules.CacheChanged(grid);
                                }
                            }
                            colorCount[c] = 0;
                        }
                    }
                }
            }

            return change;
        }

        private int SingleLineBruteForce(int[] line, int color)
        {
            int[] copy = new int[line.Length];
            for (int outer = 0; outer < line.Length; outer++)
            {
                if (line[outer] != 0)
                    continue;

                line[outer] = color;
                Array.Copy(line, copy, line.Length);
                for (int inner = 0; inner < line.Length; inner++)
                {
                    if (copy[inner] != 0)
                        continue;

                    copy[inner] = (color % 2) + 1;
                }

                if (!SingleLineAdjacencyCheck(copy))
                    return outer;

                line[outer] = 0;
            }
            return -1;
        }

        private bool SingleLineAdjacencyCheck(int[] line)
        {
            int lastColor = default;
            int repetition = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] != lastColor)
                    repetition = 0;
                if (line[i] != 0)
                    repetition++;

                if (repetition > Rules.MAX_REPETITIONS)
                    return false;

                lastColor = line[i];
            }
            return true;
        }

        private int CheckAllColors(Grid grid, int x, int y, int z)
        {
            int possibleColors = 0;
            int lastPossibleColor = 0;
            for (int i = 1; i <= Rules.ColorAmount; i++)
            {
                grid[x, y, z] = i;
                if (ruleChecker.CheckRules(grid, out _) == -1)
                {
                    possibleColors++;
                    lastPossibleColor = i;
                }
            }
            grid[x, y, z] = 0;

            switch (possibleColors)
            {
                case 0:
                    return 0;
                case 1:
                    return lastPossibleColor;
                default:
                    return -1;
            };
        }
    }
}