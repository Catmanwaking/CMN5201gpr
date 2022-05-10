//Author: Dominik Dohmeier
namespace Fast_0h_h1
{
    internal class FastRuleChecker : GenericRuleChecker<Grid>
    {
        public FastRuleChecker()
        {
            AddRule(Rules.AdjacencyRule);
            AddRule(Rules.EqualCountRule);
            AddRule(Rules.SameLineRule);
        }
    }
}