//Author: Dominik Dohmeier
using UnityEngine;
using Fast_0h_h1;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private CubeVisualizer visualizer;
    [SerializeField] private CubeInteractor interactor;
    [SerializeField] private InfoTextManager info;

    private readonly RuleChecker ruleChecker = new RuleChecker();

    private void Start()
    {
        visualizer.Initialize(level);
        interactor.Initialize(level);
        level.grid.OnTileChanged += CheckSolved;
    }

    public void OnTapInput(Vector2 position)
    {
        interactor.OnTapInput(position);
    }

    #region SwipeInput

    public void OnSwipeInput(SwipeDirection direction)
    {
        visualizer.OnSwipeInput(direction);
    }

    public void SwipeUp()
    {
        visualizer.OnSwipeInput(SwipeDirection.Up);
    }

    public void SwipeDown()
    {
        visualizer.OnSwipeInput(SwipeDirection.Down);
    }

    public void SwipeLeft()
    {
        visualizer.OnSwipeInput(SwipeDirection.Left);
    }

    public void SwipeRight()
    {
        visualizer.OnSwipeInput(SwipeDirection.Right);
    }

    #endregion

    public void IncreaseZAxis()
    {
        visualizer.CurrentZAxis++;
    }

    public void DecreaseZAxis()
    {
        visualizer.CurrentZAxis--;
    }

    private void CheckSolved(Vector3Int pos)
    {
        foreach (int item in level.grid)
        {
            if (item == 0)
                return;
        }
        int fail = level.grid.CheckRules(out int ruleInfo);
        if (fail == -1)
            Debug.Log("win");
        else
        {
            info.SetInfoText(fail);
            visualizer.GoToView(ruleInfo);
        }            
    }
}