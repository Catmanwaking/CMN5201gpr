//Author: Dominik Dohmeier
using System;
using System.Collections.Generic;

namespace Fast_0h_h1
{
    public abstract class GenericRuleChecker<T>
    {
        private readonly List<Func<T, int>> rules;

        internal GenericRuleChecker()
        {
            rules = new List<Func<T, int>>();
        }

        /// <summary>
        /// Checks if all <see cref="Rules"/> are observed.
        /// returns which rule is broken, -1 if none.
        /// </summary>
        /// <param name="grid">The Grid to check.</param>
        /// <returns></returns>
        public int CheckRules(T grid, out int info)
        {
            info = 0;
            if (rules.Count == 0)
                return -1;

            for (int i = 0; i < rules.Count; i++)
            {
                if ((info = rules[i].Invoke(grid)) != -1)
                    return i;
            }

            return -1;
        }

        internal void AddRule(Func<T, int> rule)
        {
            if (!rules.Contains(rule))
                rules.Add(rule);
        }

        internal void RemoveRule(Func<T, int> rule)
        {
            if (rules.Contains(rule))
                rules.Remove(rule);
        }
    }
}