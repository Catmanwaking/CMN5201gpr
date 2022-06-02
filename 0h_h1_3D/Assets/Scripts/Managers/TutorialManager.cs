//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private InfoTextManager info;

    [Header("UI BUttons")]
    [SerializeField] private GameObject[] ZIndexButtons;
    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject undoButton;

    [Header("Tutorial order")]
    [SerializeField] private Vector3Int[] inputOrder;

    [Header("special indices")]
    [SerializeField] private int fullLineHighlightIndex;
    [SerializeField] private int adjustCamIndex;
    [SerializeField] private int swipeIndex;
    [SerializeField] private int ZAxisIndex;
    [SerializeField] private int showUndoIndex;
    [SerializeField] private int showHintIndex;

    [SerializeField] private CubeVisualizer visualizer;
    [SerializeField] private CubeInteractor interactor;

    [SerializeField] private UnityEvent OnTutorialEnd;

    private bool awaitCubeInput = true;
    private bool awaitSwipeInput = false;
    private bool awaitZAxisInput = false;

    private int nextCubeInput;
    private int nextInfoText;

    private Vector3Int expectedCubeInput;
    private int[,,] lockedGrid;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        SetupTutorialLevel();

        visualizer.Initialize(level);
        interactor.Initialize(level);
        level.grid.OnTileChanged += OnCubeInput;
        info.SetDefaultText();

        ShowCurrent();
    }

    public void SetupTutorialLevel()
    {
        int[,,] tutorialGrid = new int[,,]
        {
            {
                { 0,0,0,2 },
                { 0,1,2,1 },
                { 1,2,1,2 },
                { 2,2,1,1 }
            },
            {
                { 0,1,2,1 },
                { 0,2,1,2 },
                { 1,0,0,0 },
                { 2,1,1,2 }
            },
            {
                { 0,2,1,2 },
                { 0,1,1,2 },
                { 0,1,2,1 },
                { 0,2,2,1 }
            },
            {
                { 0,2,1,1 },
                { 1,2,2,1 },
                { 2,1,1,2 },
                { 1,1,2,2 }
            }
        };

        CubeGrid grid = new(tutorialGrid);
        lockedGrid = grid.GetLockedGrid();
        for (int x = 0; x < grid.SideLength; x++)
        {
            for (int y = 0; y < grid.SideLength; y++)
            {
                for (int z = 0; z < grid.SideLength; z++)
                    lockedGrid[x, y, z] = 1;
            }
        }

        level.grid = grid;
    }

    public void OnSwipeInput(SwipeDirection direction)
    {
        if (!awaitSwipeInput)
            return;

        Vector3Int input = direction switch
        {
            SwipeDirection.Up => Vector3Int.up,
            SwipeDirection.Down => Vector3Int.down,
            SwipeDirection.Left => Vector3Int.left,
            SwipeDirection.Right => Vector3Int.right,
            _ => Vector3Int.zero
        };
        if (expectedCubeInput == input)
        {
            visualizer.OnSwipeInput(direction, this);
            CycleNextInput();
        }
    }

    public void OnTapInput(Vector2 position)
    {
        if (!awaitCubeInput)
            return;

        interactor.OnTapInput(position);
    }

    public void IncreaseZAxis()
    {
        if (!awaitZAxisInput)
            return;

        if(expectedCubeInput == Vector3Int.forward)
        {
            visualizer.CurrentZAxis++;
            CycleNextInput();
        }
    }

    public void DecreaseZAxis()
    {
        if (!awaitZAxisInput)
            return;

        if (expectedCubeInput == Vector3Int.back)
        {
            visualizer.CurrentZAxis--;
            CycleNextInput();
        }
    }

    private void ShowCurrent()
    {
        if(awaitCubeInput)
        {
            if (fullLineHighlightIndex == nextInfoText)
                visualizer.HighlightLine((1 << 6) + (0 << 3) + 3); //Hardcoded tutorial stuff

            visualizer.HighlightSingle(expectedCubeInput);
            lockedGrid[expectedCubeInput.x, expectedCubeInput.y, expectedCubeInput.z] = 0;
        }
        info.SetTutorialText(nextInfoText);
    }

    private void CycleNextInput()
    {
        lockedGrid[expectedCubeInput.x, expectedCubeInput.y, expectedCubeInput.z] = 1;
        nextCubeInput++;

        if(nextCubeInput >= inputOrder.Length)
        {
            nextInfoText++;
            info.SetTutorialText(nextInfoText);
            OnTutorialEnd?.Invoke();
            return;
        }

        if (expectedCubeInput != inputOrder[nextCubeInput])
            nextInfoText++;

        expectedCubeInput = inputOrder[nextCubeInput];

        if (adjustCamIndex == nextCubeInput)
            visualizer.ResetCameraAngle(this);
        if(showUndoIndex == nextCubeInput)
            undoButton.SetActive(true);
        if(showHintIndex == nextCubeInput)
            hintButton.SetActive(true);

        if(swipeIndex == nextCubeInput)
        {
            awaitCubeInput = false;
            awaitSwipeInput = true;
        } 
        else if(ZAxisIndex == nextCubeInput)
        {
            awaitCubeInput = false;
            awaitZAxisInput = true;
            foreach (var item in ZIndexButtons)
                item.SetActive(true);
        } 
        else
        {
            awaitCubeInput = true;
            awaitSwipeInput = false;
            awaitZAxisInput = false;
        }
        ShowCurrent();
    }

    private void OnCubeInput(Vector3Int pos)
    {
        CycleNextInput();
    }
}