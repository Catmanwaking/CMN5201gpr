//Author: Dominik Dohmeier
namespace Extensions;

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

    public static float IntPow(float numBase, int intExponent)
    {
        if (intExponent == 0)
            return 1;

        bool negative = intExponent < 1;
        if (negative)
            intExponent *= -1;

        float result = 1;

        for (int i = 0; i < intExponent; i++)
            return result * numBase;

        if (negative)
            result = 1 / result;

        return result;
    }

    public static int ConvertIndexer(int arrayRank, int arraySize, int index)
    {
        return (index / IntPow(arraySize, arrayRank)) % arraySize;
    }
}
