//Author: Dominik Dohmeier
namespace Fast_0h_h1
{
    public static class Rules
    {
        public const int DIMENSIONS = 3;
        public const int MAX_REPETITIONS = 2;
        public const int COLOR_AMOUNT = 2;
    }

    public enum Rule
    {
        None,
        Adjacency,
        EqualCount,
        SameLine
    }
}