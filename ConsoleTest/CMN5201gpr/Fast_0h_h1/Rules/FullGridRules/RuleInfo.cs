//Author: Dominik Dohmeier
namespace Fast_0h_h1
{
    public struct RuleInfo
    {
        public Rule brokenRule;
        public int direction;
        public int axis1;
        public int axis2;

        public RuleInfo(Rule brokenRule, int direction, int axis1, int axis2)
        {
            this.brokenRule = brokenRule;
            this.direction = direction;
            this.axis1 = axis1;
            this.axis2 = axis2;
        }
    }
}