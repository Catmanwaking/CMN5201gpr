//Author: Dominik Dohmeier
using System;
using System.Collections.Generic;

namespace Fast_0h_h1
{
    public class FullRuleChecker
    {
        private readonly Func<int[,,], RuleInfo>[] rules;

        internal FullRuleChecker()
        {
            rules = new Func<int[,,], RuleInfo>[]
            {
                FullRules.AdjacencyRule,
                FullRules.EqualCountRule,
                FullRules.SameLineRule
            };
        }

        /// <summary>
        /// Checks if all <see cref="FullRules"/> are observed.
        /// returns which rule is broken, -1 if none.
        /// </summary>
        /// <param name="grid">The Grid to check.</param>
        /// <returns></returns>
        public RuleInfo CheckRules(int[,,] grid)
        {
            if (rules.Length == 0)
                return new RuleInfo(Rule.None, 0, 0, 0);

            RuleInfo info;

            for (int i = 0; i < rules.Length; i++)
            {
                info = rules[i].Invoke(grid);
                if (info.brokenRule != Rule.None)
                    return info;
            }

            return new RuleInfo(Rule.None, 0,0,0);
        }
    }
}