//Author: Dominik Dohmeier
using System;
using System.Collections.Generic;

namespace Optimized_0h_h1_3D
{
    public class RuleChecker
    {
        private readonly List<Func<Board, bool>> rules;

        public RuleChecker()
        {
            rules = new List<Func<Board, bool>>();
        }

        public bool CheckRules(Board board)
        {
            if (rules.Count == 0)
                return true;

            foreach (var rule in rules)
            {
                if (!rule.Invoke(board))
                    return false;
            }

            return true;
        }

        public void AddRule(Func<Board, bool> rule)
        {
            if (!rules.Contains(rule))
                rules.Add(rule);
        }

        public void RemoveRule(Func<Board, bool> rule)
        {
            if (rules.Contains(rule))
                rules.Remove(rule);
        }

        public static RuleChecker CreateStandardRuleChecker()
        {
            RuleChecker checker = new();
            checker.AddRule(Rules.AdjacencyRule);
            checker.AddRule(Rules.EqualCountRule);
            checker.AddRule(Rules.SameLineRule);
            return checker;
        }
    }
}
