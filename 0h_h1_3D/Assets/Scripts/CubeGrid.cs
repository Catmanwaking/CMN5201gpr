//Author: Dominik Dohmeier
using UnityEngine;
using Fast_0h_h1;

public class CubeGrid
{
    public event System.Action OnGridChanged;
    public event System.Action OnTileChanged;

    private int[,,] internalGrid;
    private int[,,] bufferGrid;
    private int sideLength;

    public Vector3Int LastEditPos;

    public int SideLength { get => sideLength; }

    public int this[int x, int y, int z]
    {
        get => internalGrid[x, y, z];
        set
        {
            internalGrid[x, y, z] = value;
            LastEditPos = new Vector3Int(x, y, z);
            OnTileChanged?.Invoke();
        }
    }

    public int this[Vector3Int pos]
    {
        get => internalGrid[pos.x, pos.y, pos.z];
        set 
        {
            internalGrid[pos.x, pos.y, pos.z] = value;
            LastEditPos = pos;
            OnTileChanged?.Invoke();
        }
    }

    public CubeGrid(int size)
    {
        LevelCreator creator = new(size);
        internalGrid = creator.GenerateLevel();
        sideLength = internalGrid.GetLength(0);
        LastEditPos = new Vector3Int();
    }

    #region Tmp Rotations

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
        OnGridChanged?.Invoke();
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
    private void TripleNestedForLoop(System.Action<int, int, int> Rotate)
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