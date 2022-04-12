//Author: Dominik Dohmeier
namespace _0h_h1_3D_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RuleChecker<int[,,]> ruleChecker = new RuleChecker<int[,,]>();
            ruleChecker.AddRule(Rules3D.AdjacencyRule);
        }
    }
}