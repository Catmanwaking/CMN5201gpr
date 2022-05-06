//Author: Dominik Dohmeier
namespace Fast_0h_h1;

public class RuleChecker : GenericRuleChecker<int[,,]>
{
    public RuleChecker()
    {
        AddRule(Rules.AdjacencyRule);
        AddRule(Rules.EqualCountRule);
        AddRule(Rules.SameLineRule);
    }
}