//Author: Dominik Dohmeier
using System.Collections;

namespace _0h_h1_2D_Console;

public class Board : IEnumerable
{
    private const int DIMENSION = 2;

    private readonly int[,] internalBoard;

    public int SideLength { get; }
    public int Size { get; }
    public int Length { get => internalBoard.Length; }

    public int this[int posX, int posY]
    {
        get => internalBoard[posX, posY];
        set => internalBoard[posX, posY] = value;
    }

    public Board(int size)
    {
        if(size < 0)
            throw new ArgumentOutOfRangeException(nameof(size));

        Size = size;
        SideLength = Size * DIMENSION;
        internalBoard = new int[SideLength, SideLength];
    }

    public Board CreateCopy()
    {
        Board copy = new(Size);

        for (int x = 0; x < SideLength; x++)
        {
            for (int y = 0; y < SideLength; y++)
                copy[x, y] = internalBoard[x, y];
        }

        return copy;
    }

    public IEnumerator GetEnumerator()
    {
        return internalBoard.GetEnumerator();
    }
}
