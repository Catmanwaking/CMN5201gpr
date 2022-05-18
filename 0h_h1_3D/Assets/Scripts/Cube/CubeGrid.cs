//Author: Dominik Dohmeier
using UnityEngine;
using System;
using Fast_0h_h1;
using System.Collections;

public class CubeGrid : IEnumerable
{
    public event Action<Vector3Int> OnTileChanged;

    private readonly int[,,] internalGrid;
    private readonly int[,,] lockedGrid;
    public readonly int SideLength;
    public readonly int ColorCount = 2;

    private Vector3Int LastEditPos;

    private RuleChecker ruleChecker;

    public int Tiles { get => internalGrid.Length; }

    public int this[int x, int y, int z]
    {
        get => internalGrid[x, y, z];
        set
        {
            if(lockedGrid[x,y,z] != 0)
                return;
            internalGrid[x, y, z] = value;
            LastEditPos = new Vector3Int(x, y, z);
            OnTileChanged?.Invoke(LastEditPos);
        }
    }

    public int this[Vector3Int pos]
    {
        get => this[pos.x, pos.y, pos.z];
        set => this[pos.x, pos.y, pos.z] = value;       
    }

    public CubeGrid(int size)
    {
        LevelCreator creator = new(size);
        internalGrid = creator.GenerateLevel();
        SideLength = internalGrid.GetLength(0);

        lockedGrid = new int[SideLength, SideLength, SideLength];
        Array.Copy(internalGrid, lockedGrid, internalGrid.Length);

        LastEditPos = new Vector3Int();
        ruleChecker = new RuleChecker();
    }

    public int CheckRules(out int info)
    {
        return ruleChecker.CheckRules(internalGrid, out info);
    }

    public int GetHint(out int info)
    {
        throw new NotImplementedException();
    }

    public IEnumerator GetEnumerator()
    {
        return internalGrid.GetEnumerator();
    }
}