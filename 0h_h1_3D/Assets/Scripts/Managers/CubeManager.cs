//Author: Dominik Dohmeier
using Optimized_0h_h1_3D;
using System.Collections;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private LevelSO level;

    private void Start()
    {
        LevelCreator levelCreator = new();
        level.board = levelCreator.CreateLevel(2, 12,2);
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForEndOfFrame();

        gridManager.InitializeGrid(4, OnTileClicked);
    }

    private void OnTileClicked(Vector2Int tilePos)
    {
        Vector3Int pos = new(tilePos.x, tilePos.y, gridManager.ZIndex);
        int color = level.board[pos.x, pos.y, pos.z];
        color = (color + 1) % (Rules.ColorCount + 1);
        level.board[pos.x, pos.y, pos.z] = color;
    }
}