//Author: Dominik Dohmeier
using System;

namespace Fast_0h_h1
{
    public class HintSystem
    {
        private FullRuleChecker checker = new FullRuleChecker();

        public RuleInfo GetHint(int[,,] input)
        {
            FullRules.Initialize(input.GetLength(0) / Rules.COLOR_AMOUNT);

            RuleInfo info = checker.CheckRules(input);
            if(info.brokenRule != Rule.None)
                return info;

            info = SingleCheck(input);
            if (info.brokenRule != Rule.None)
                return info;

            info = RowCheck(input);
            if (info.brokenRule != Rule.None)
                return info;

            return new RuleInfo();
        }

        private RuleInfo SingleCheck(int[,,] grid)
        {
            int sideLength = grid.GetLength(0);
            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    for (int z = 0; z < sideLength; z++)
                    {
                        if (grid[x, y, z] != 0)
                            continue;

                        RuleInfo info = CheckAllColors(grid, x, y, z);

                        if (info.brokenRule != Rule.None)
                            return info;
                    }
                }
            }
            return new RuleInfo();
        }

        private RuleInfo RowCheck(int[,,] grid)
        {
            int sideLength = grid.GetLength(0);
            int checkCount = (sideLength >> 1) - 1;
            if (checkCount == 1)
                return new RuleInfo();

            V3Int pos = new V3Int();

            int color;
            int[] colorCount = new int[Rules.COLOR_AMOUNT];
            int[] line = new int[sideLength];

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

                            if (color == 0)
                                continue;

                            colorCount[color - 1]++;
                        }

                        for (int c = 0; c < Rules.COLOR_AMOUNT; c++)
                        {
                            if (colorCount[c] == checkCount)
                            {
                                for (int x = 0; x < sideLength; x++)
                                {
                                    pos[d] = x;
                                    line[x] = grid[pos.X, pos.Y, pos.Z];
                                }
                                int col = SingleLineBruteForce(line, c + 1);
                                if (col != -1)
                                    return new RuleInfo(Rule.Adjacency, d, y, z);
                            }
                            colorCount[c] = 0;
                        }
                    }
                }
            }

            return new RuleInfo();
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

        private RuleInfo CheckAllColors(int[,,] grid, int x, int y, int z)
        {
            RuleInfo info;

            for (int i = 1; i <= Rules.COLOR_AMOUNT; i++)
            {
                grid[x, y, z] = i;
                info = checker.CheckRules(grid);
                if (info.brokenRule != Rule.None)
                {
                    grid[x, y, z] = 0;
                    return info;
                }
            }
            grid[x, y, z] = 0;
            return new RuleInfo();
        }
    }    
}