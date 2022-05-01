//Author: Dominik Dohmeier
using Optimized_0h_h1_3D;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridManager : MonoBehaviour
{
    [SerializeField] private ButtonScript buttonPrefab;
    [SerializeField] private LevelSO level;

    private GridLayoutGroup grid;
    private float availableSize;
    private int zIndex = 0;

    private ButtonScript[,] buttons;

    public int ZIndex
    {
        get => zIndex;
        private set
        {
            zIndex = Mathf.Clamp(value,0,level.board.SideLength - 1);
            OnZIndexChanged();
        }
    }

    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    private void OnBoardChanged()
    {
        Board board = level.board;
        V3Int pos = board.LastEditPos;

        buttons[pos.X, pos.Y].SetColor(board[pos.X, pos.Y, ZIndex]);
    }

    private void OnZIndexChanged()
    {
        Board board = level.board;
        for (int x = 0; x < board.SideLength; x++)
        {
            for (int y = 0; y < board.SideLength; y++)
            {
                buttons[x, y].SetColor(board[x, y, ZIndex]);
            }
        }
    }

    private void GenerateButtons(int cellSideCount, System.Action<Vector2Int> callBack)
    {
        buttons = new ButtonScript[cellSideCount, cellSideCount];

        for (int x = 0; x < cellSideCount; x++)
        {
            for (int y = 0; y < cellSideCount; y++)
            {
                ButtonScript button = Instantiate(buttonPrefab, grid.transform);
                button.SetupButton(x, y, callBack);
                buttons[x, y] = button;
            }
        }
    }

    public void InitializeGrid(int cellSideCount, System.Action<Vector2Int> callBack)
    {
        availableSize = GetComponent<RectTransform>().rect.width / cellSideCount;
        grid.cellSize = new Vector2(availableSize, availableSize);
        GenerateButtons(cellSideCount, callBack);

        level.board.BoardChanged += OnBoardChanged;

        OnZIndexChanged();
    }

    public void IncreaseZIndex()
    {
        ZIndex++;
    }

    public void DecreaseZIndex()
    {
        ZIndex--;
    }
}