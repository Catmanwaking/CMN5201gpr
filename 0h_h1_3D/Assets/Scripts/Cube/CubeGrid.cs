//Author: Dominik Dohmeier
using UnityEngine;
using Fast_0h_h1;

public class CubeGrid
{
    public event System.Action<Vector3Int> OnTileChanged;

    private readonly int[,,] internalGrid;
    public readonly int SideLength;
    public readonly int ColorCount = 2;

    private Vector3Int LastEditPos;

    public int this[int x, int y, int z]
    {
        get => internalGrid[x, y, z];
        set
        {
            internalGrid[x, y, z] = value;
            LastEditPos = new Vector3Int(x, y, z);
            OnTileChanged?.Invoke(LastEditPos);
        }
    }

    public int this[Vector3Int pos]
    {
        get => internalGrid[pos.x, pos.y, pos.z];
        set 
        {
            internalGrid[pos.x, pos.y, pos.z] = value;
            LastEditPos = pos;
            OnTileChanged?.Invoke(pos);
        }
    }

    public CubeGrid(int size)
    {
        LevelCreator creator = new(size);
        internalGrid = creator.GenerateLevel();
        SideLength = internalGrid.GetLength(0);
        LastEditPos = new Vector3Int();
    }
}