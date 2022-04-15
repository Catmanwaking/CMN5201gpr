using System.Collections;
using System.Text;

namespace _0h_h1_3D_Console
{
    class BruteForceSolver
    {
        RuleChecker<int[,]> ruleChecker;
        int sideLength;
        int posX, posY;
        int solutionCount;
        int[,] board;

        public BruteForceSolver()
        {
            CreateRuleChecker();
        }

        private void CreateRuleChecker()
        {
            ruleChecker = new RuleChecker<int[,]>();
            ruleChecker.AddRule(Rules2D.AdjacencyRule);
            ruleChecker.AddRule(Rules2D.EqualCountRule);
            ruleChecker.AddRule(Rules2D.SameLineRule);
        }

        public int CountSolutions(int sideLength)
        {
            this.sideLength = sideLength;
            board = new int[sideLength, sideLength];
            posX = 0;
            posY = 0;
            solutionCount = 0;

            while (true)
            {
                FindForward();
                if (!FindBackward())
                    break;
            }

            return solutionCount;
        }

        private bool IterateForward()
        {
            posY++;
            if (posY < sideLength)
                return true;
            posY = 0;

            posX++;
            if (posX < sideLength)
                return true;
            posX = sideLength - 1;
            posY = sideLength - 1;

            return false;
        }

        private bool IterateBackward()
        {
            posY--;
            if (posY >= 0)
                return true;
            posY = sideLength -1;

            posX--;
            if (posX >= 0)
                return true;
            posX = 0;
            posY = 0;

            return false;
        }

        private void FindForward()
        {
            while(FindFittingColor())
            {
                if (!IterateForward())
                {
                    if (solutionCount == 0)
                        PrintSolution();
                    solutionCount++;
                }
            }
        }

        private bool FindBackward()
        {
            while (IterateBackward())
            {
                if (FindFittingColor())
                {
                    IterateForward();
                    return true;
                }
            }
            return false;
        }

        private bool FindFittingColor()
        {
            int start = board[posX, posY] + 1;
            for (int i = start; i <= Rules2D.COLOR_COUNT; i++)
            {
                board[posX, posY] = i;
                if (ruleChecker.CheckRules(board))
                    return true;
            }
            board[posX, posY] = 0;
            return false;
        }

        private void PrintSolution()
        {
            StringBuilder builder = new StringBuilder();
            for (int x = 0; x < sideLength; x++)
            {
                for (int y = 0; y < sideLength; y++)
                {
                    builder.Append(board[x, y].ToString()) ;
                }
                builder.AppendLine();
            }
            System.Console.WriteLine(builder.ToString());
        }
    }
}
