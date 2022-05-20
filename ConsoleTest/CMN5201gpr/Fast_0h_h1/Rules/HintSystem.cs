//Author: Dominik Dohmeier
using System;

namespace Fast_0h_h1
{
    public class HintSystem
    {
        private RuleChecker checker = new RuleChecker();

        public int GetHint(int[,,] input,out int ruleInfo)
        {
            int rule = checker.CheckRules(input, out ruleInfo);
            if(rule != -1)
                return rule;

            rule = SingleCheck(input, out ruleInfo);
            if (rule != -1)
                return rule;

            rule = RowCheck(input, out ruleInfo);
            if (rule != -1)
                return rule;

            return -1;
        }

        private int SingleCheck(int[,,] grid, out int ruleInfo)
        {
            int sideLength = grid.GetLength(0);
            ruleInfo = -1;
            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    for (int z = 0; z < sideLength; z++)
                    {
                        if (grid[x, y, z] != 0)
                            continue;

                        int brokenRule = CheckAllColors(grid, x, y, z, out ruleInfo);

                        if (brokenRule != -1)
                            return brokenRule;
                    }
                }
            }
            return -1;
        }

        private int RowCheck(int[,,] grid, out int ruleInfo)
        {
            int sideLength = grid.GetLength(0);
            ruleInfo = -1;
            int checkCount = (sideLength >> 1) - 1;
            if (checkCount == 1)
                return -1;

            V3Int pos = new V3Int();

            int color;
            int[] colorCount = new int[Rules.ColorAmount];
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

                        for (int c = 0; c < Rules.ColorAmount; c++)
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
                                {
                                    ruleInfo = (d << 6) + (y << 3) + z;
                                    return 0;
                                }
                            }
                            colorCount[c] = 0;
                        }
                    }
                }
            }

            return -1;
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

        private int CheckAllColors(int[,,] grid, int x, int y, int z, out int ruleInfo)
        {
            ruleInfo = -1;
            int brokenRule;
            for (int i = 1; i <= Rules.ColorAmount; i++)
            {
                grid[x, y, z] = i;
                brokenRule = checker.CheckRules(grid, out ruleInfo);
                if (brokenRule != -1)
                {
                    grid[x, y, z] = 0;
                    return brokenRule;
                }
            }
            grid[x, y, z] = 0;
            return -1;
        }
    }    
}