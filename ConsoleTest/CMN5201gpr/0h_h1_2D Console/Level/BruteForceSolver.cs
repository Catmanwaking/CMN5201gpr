//Author: Dominik Dohmeier
namespace _0h_h1_2D_Console;

internal class BruteForceSolver
{
    private RuleChecker ruleChecker;
    private Board testBoard;
    private Board referenceBoard;
    private int posX, posY;
    private int sideLength;
    private int solutionCount;

    public BruteForceSolver()
    {
        CreateRuleChecker();
    }

    private void CreateRuleChecker()
    {
        ruleChecker = new RuleChecker();
        ruleChecker.AddRule(Rules.AdjacencyRule);
        ruleChecker.AddRule(Rules.EqualCountRule);
        ruleChecker.AddRule(Rules.SameLineRule);
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
        posY = sideLength - 1;

        posX--;
        if (posX >= 0)
            return true;
        posX = 0;
        posY = 0;

        return false;
    }

    private void FindForward()
    {
        while (FindFittingColor())
        {
            if (!IterateForward())
            {
                solutionCount++;
                FindFittingColor();
                break;
            }
        }
    }

    private bool FindBackward()
    {
        while (IterateBackward())
        {
            if (FindFittingColor(false))
            {
                IterateForward();
                return true;
            }
        }
        return false;
    }

    private bool FindFittingColor(bool forward = true)
    {
        if (referenceBoard[posX, posY] != 0)
            return forward;
        int start = testBoard[posX, posY] + 1;
        for (int i = start; i <= Rules.COLOR_COUNT; i++)
        {
            testBoard[posX, posY] = i;
            if (ruleChecker.CheckRules(testBoard))
                return true;
        }
        testBoard[posX, posY] = 0;
        return false;
    }

    public int CountSolutions(Board inputBoard)
    {
        if (!ruleChecker.CheckRules(inputBoard))
            return 0;

        referenceBoard = inputBoard;
        testBoard = referenceBoard.CreateCopy();;
        sideLength = testBoard.SideLength;
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
}
