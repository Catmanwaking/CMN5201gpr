//Author: Dominik Dohmeier
using System;
using System.Collections.Generic;

namespace Fast_0h_h1
{
    internal class FastRuleChecker
    {
        private readonly Func<Grid, bool>[] rules;

        public FastRuleChecker()
        {
            rules = new Func<Grid, bool>[]
            {
                FastRules.AdjacencyRule,
                FastRules.EqualCountRule,
                FastRules.SameLineRule
            };
        }

        public bool CheckRules(Grid grid)
        {
            if (rules.Length == 0)
                return false;

            for (int i = 0; i < rules.Length; i++)
            {
                if (rules[i].Invoke(grid))
                    return true;
            }

            return false;
        }
    }
}