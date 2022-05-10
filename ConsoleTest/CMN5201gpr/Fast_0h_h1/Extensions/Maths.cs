//Author: Dominik Dohmeier
namespace Fast_0h_h1.Extensions
{
    internal static class Maths
    {
        public static int IntPow(int numBase, int intExponent)
        {
            if (intExponent <= 0)
                return 1;
            int result = 1;

            for (int i = 0; i < intExponent; i++)
                result *= numBase;

            return result;
        }
    }
}