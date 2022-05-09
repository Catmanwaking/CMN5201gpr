//Author: Dominik Dohmeier
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
            zIndex = Mathf.Clamp(value,0,level.grid.SideLength - 1);
            OnZIndexChanged();
        }
    }

    private void Start()
    {
        grid = GetComponent<GridLayoutGroup>();
    }

    private void OnBoardChanged()
    {
        CubeGrid board = level.grid;
        Vector3Int pos = board.LastEditPos;

        buttons[pos.x, pos.y].SetColor(board[pos.x, pos.y, ZIndex]);
    }

    private void OnZIndexChanged()
    {
        CubeGrid board = level.grid;
        for (int x = 0; x < board.SideLength; x++)
        {
            for (int y = 0; y < board.SideLength; y++)
                buttons[x, y].SetColor(board[x, y, ZIndex]);
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

        level.grid.OnTileChanged += OnBoardChanged;
        level.grid.OnGridChanged += OnZIndexChanged;

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