//Author: Dominik Dohmeier
using Fast_0h_h1.Extensions;

namespace Fast_0h_h1;

internal class V3Int
{
    private readonly int[] values = new int[Rules.DIMENSIONS];

    public int this[int d]
    {
        get => values[d];
        set => values[d] = value;
    }

    public int X
    {
        get => values[0];
    }

    public int Y
    {
        get => values[1];
    }

    public int Z
    {
        get => values[2];
    }

    public V3Int()
    {
        values = new int[Rules.DIMENSIONS];
    }

    public V3Int(int x, int y, int z)
    {
        values = new int[] { x, y, z };
    }

    public void SetValues(int x, int y, int z)
    {
        values[0] = x;
        values[1] = y;
        values[2] = z;
    }

    public int[] GetValues()
    {
        return values;
    }

    public void SetFromIndexer(int arraySideLength, int index)
    {
        for (int d = 0; d < values.Length; d++)
            values[d] = (index / Maths.IntPow(arraySideLength, d)) % arraySideLength;
    }
}