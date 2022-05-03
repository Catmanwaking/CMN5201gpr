//Author: Dominik Dohmeier
using Optimized_0h_h1_3D;
using System;

public class PlayerGrid : GameGrid
{
    public event Action TileChanged;
    public event Action BoardChanged;

    private int[,,] bufferGrid;

    public override int this[int posX, int posY, int posZ]
    {
        get => internalGrid[posX, posY, posZ];
        set
        {
            internalGrid[posX, posY, posZ] = value;
            LastEditPos.SetValues(posX, posY, posZ);
            TileChanged?.Invoke();
        }
    }

    public override int this[V3Int pos]
    {
        get => internalGrid[pos.X, pos.Y, pos.Z];
        set
        {
            internalGrid[pos.X, pos.Y, pos.Z] = value;
            LastEditPos.SetValues(pos.X, pos.Y, pos.Z);
            TileChanged?.Invoke();
        }
    }

    public PlayerGrid(int size, int colorCount) : base(size, colorCount) { }

    public void SetFromGrid(GameGrid grid)
    {
        if (grid.SideLength != this.SideLength)
            return;

        internalGrid = grid.ExportGrid();
    }

    #region Rotations

    public void Rotate(SwipeDirection direction)
    {
        if (bufferGrid == null)
            bufferGrid = new int[SideLength, SideLength, SideLength];


        //TODO Order seems to be messed up
        switch (direction)
        {
            case SwipeDirection.Up:
                TripleNestedForLoop(RotateYCCW);
                break;
            case SwipeDirection.Down:
                TripleNestedForLoop(RotateYCW);
                break;
            case SwipeDirection.Left:
                TripleNestedForLoop(RotateXCW);
                break;
            case SwipeDirection.Right:
                TripleNestedForLoop(RotateXCCW);
                break;
            default:
                break;
        }

        (bufferGrid, internalGrid) = (internalGrid, bufferGrid); //swap values
        BoardChanged?.Invoke();
    }

    public void RotateXCW(int x, int y, int z)
    {
        bufferGrid[x, y, z] = internalGrid[x, SideLength - z - 1, y];
    }

    public void RotateXCCW(int x, int y, int z)
    {
        bufferGrid[x, y, z] = internalGrid[x, z, SideLength - y - 1];    
    }

    public void RotateYCW(int x, int y, int z)
    {
        bufferGrid[x, y, z] = internalGrid[z, y, SideLength - x - 1];
    }

    public void RotateYCCW(int x, int y, int z)
    {
        bufferGrid[x, y, z] = internalGrid[SideLength - z - 1, y, x];
    }

    public void RotateZCW(int x, int y, int z)
    {
        bufferGrid[x, y, z] = internalGrid[SideLength - y - 1, x, z];
    }

    public void RotateZCCW(int x, int y, int z)
    {
        bufferGrid[x, y, z] = internalGrid[y, SideLength - x - 1, z];
    }

    /// <summary>
    /// Executes an <see cref="Action{T1, T2, T3}"/> within a 3 times nested for loop.
    /// </summary>
    /// <param name="Rotate"></param>
    private void TripleNestedForLoop(Action<int, int, int> Rotate)
    {
        for (int x = 0; x < SideLength; x++)
        {
            for (int y = 0; y < SideLength; y++)
            {
                for (int z = 0; z < SideLength; z++)
                    Rotate.Invoke(x, y, z);
            }
        }
    }

    #endregion
}
