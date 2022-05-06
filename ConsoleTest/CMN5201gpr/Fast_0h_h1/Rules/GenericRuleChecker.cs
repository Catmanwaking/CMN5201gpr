//Author: Dominik Dohmeier
namespace Fast_0h_h1;

public abstract class GenericRuleChecker<T>
{
    private readonly List<Func<T, bool>> rules;

    internal GenericRuleChecker()
    {
        rules = new List<Func<T, bool>>();
    }

    internal bool CheckRules(T grid)
    {
        if (rules.Count == 0)
            return true;

        foreach (var rule in rules)
        {
            if (!rule.Invoke(grid))
                return false;
        }

        return true;
    }

    internal void AddRule(Func<T, bool> rule)
    {
        if (!rules.Contains(rule))
            rules.Add(rule);
    }

    internal void RemoveRule(Func<T, bool> rule)
    {
        if (rules.Contains(rule))
            rules.Remove(rule);
    }
}