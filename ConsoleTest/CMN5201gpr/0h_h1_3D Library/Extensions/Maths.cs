//Author: Dominik Dohmeier
namespace _0h_h1_3D_Library.Extensions;

internal class Maths
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