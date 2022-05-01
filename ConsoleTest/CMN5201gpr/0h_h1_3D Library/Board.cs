//Author: Dominik Dohmeier
using System.Collections;

namespace _0h_h1_3D_Library;

internal class Board : IEnumerable
{
    private readonly int[,,] internalBoard;

    private readonly int[][,] linkedCache;

    public int SideLength { get; }
    public int Size { get; }
    public int Length { get => internalBoard.Length; }

    public V3Int LastEditPos { get; }

    public int this[int posX, int posY, int posZ]
    {
        get => internalBoard[posX, posY, posZ];
        set
        {
            internalBoard[posX, posY, posZ] = value;
            LastEditPos.SetValues(posX, posY, posZ);
            ManageCache();
        }
    }

    public int this[V3Int pos]
    {
        get => internalBoard[pos.X, pos.Y, pos.Z];
        set
        {
            internalBoard[pos.X, pos.Y, pos.Z] = value;
            LastEditPos.SetValues(pos.X, pos.Y, pos.Z);
            ManageCache();
        }
    }

    public Board(int size)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        Size = size;
        SideLength = Size * Rules.ColorCount;
        internalBoard = new int[SideLength, SideLength, SideLength];

        LastEditPos = new V3Int();

        linkedCache = Rules.GetCache();
    }

    private void ManageCache()
    {
        for (int d = 0; d < Rules.DIMENSIONS; d++)
        {
            linkedCache[d]
            [
                LastEditPos[(d + 1) % Rules.DIMENSIONS],
                LastEditPos[(d + 2) % Rules.DIMENSIONS]
            ] = 0;
        }
    }

    public IEnumerator GetEnumerator()
    {
        return internalBoard.GetEnumerator();
    }

    public int[,,] ExportGrid()
    {
        int[,,] grid = new int[SideLength, SideLength, SideLength];
        Array.Copy(internalBoard, grid, internalBoard.Length);
        return grid;
    }
}