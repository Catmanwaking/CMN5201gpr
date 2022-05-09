//Author: Dominik Dohmeier
using System.Collections;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevelSO level;

    private const int colorCount = 2;

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();
        //wait for UI to initialize first
        gridManager.InitializeGrid(4, OnTileClicked);
    }

    private void OnTileClicked(Vector2Int tilePos)
    {
        Vector3Int pos = new(tilePos.x, tilePos.y, gridManager.ZIndex);
        int color = level.grid[pos.x, pos.y, pos.z];
        color = (color + 1) % (colorCount + 1);
        level.grid[pos.x, pos.y, pos.z] = color;
    }

    public void RotateCube(SwipeDirection direction)
    {
        level.grid.Rotate(direction);
    }

    public void RotateUp()
    {
        RotateCube(SwipeDirection.Up);
    }

    public void RotateDown()
    {
        RotateCube(SwipeDirection.Down);
    }

    public void RotateLeft()
    {
        RotateCube(SwipeDirection.Left);
    }

    public void RotateRight()
    {
        RotateCube(SwipeDirection.Right);
    }
}