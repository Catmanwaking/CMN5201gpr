//Author: Dominik Dohmeier
using System.Collections.Generic;
using UnityEngine;

public class CubeUndoer
{
    private LevelSO level;
    private Stack<(Vector3Int, int)> invertedActions;

    public CubeUndoer(LevelSO level)
    {
        this.level = level;
        level.grid.OnTileChanged += OnTileChanged;
        invertedActions = new Stack<(Vector3Int pos, int color)>();
    }

    public void Undo()
    {
        if (invertedActions.Count == 0)
            return;
        (Vector3Int pos, int color) = invertedActions.Pop();
        level.grid.OnTileChanged -= OnTileChanged;
        level.grid[pos] = color;
        level.grid.OnTileChanged += OnTileChanged;
    }

    private void OnTileChanged(Vector3Int tile)
    {
        int previousColor = level.grid[tile] -1;
        if (previousColor < 0)
            previousColor = ColorIndex.ColorCount - 1;

        if (invertedActions.Count == 0)
            invertedActions.Push((tile, previousColor));
        else
        {
            (Vector3Int pos, int color) = invertedActions.Peek();
            if (pos != tile)
                invertedActions.Push((tile, previousColor));
        }
    }
}