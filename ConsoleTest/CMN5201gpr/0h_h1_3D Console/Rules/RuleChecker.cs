//Author: Dominik Dohmeier
using System;
using System.Collections;
using System.Collections.Generic;

namespace _0h_h1_3D_Console
{
    public class RuleChecker<T> where T : IList
    {
        private readonly List<Func<T, bool>> rules;

        public RuleChecker()
        {
            rules = new List<Func<T, bool>>();
        }

        public bool CheckRules(T board)
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

        public void AddRule(Func<T, bool> rule)
        {
            if (!rules.Contains(rule))
                rules.Add(rule);
        }

        public void RemoveRule(Func<T, bool> rule)
        {
            if (rules.Contains(rule))
                rules.Remove(rule);
        }
    }
}