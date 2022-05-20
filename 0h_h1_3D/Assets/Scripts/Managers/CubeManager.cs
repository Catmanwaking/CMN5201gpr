//Author: Dominik Dohmeier
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private ScoreSO score;
    [SerializeField] private InfoTextManager info;
    [SerializeField] private CubeVisualizer visualizer;
    [SerializeField] private CubeInteractor interactor;
    [SerializeField] private GameObject[] objectsToDisable;

    [SerializeField] private UnityEvent OnCubeSolved;

    private CubeUndoer undoer;
    private Coroutine fullGridRoutine;

    private void Start()
    {
        visualizer.Initialize(level);
        interactor.Initialize(level);
        undoer = new CubeUndoer(level);
        info.SetDefaultText();
        level.grid.OnTileChanged += CheckSolved;
    }

    #region SwipeInput

    public void OnSwipeInput(SwipeDirection direction) => visualizer.OnSwipeInput(direction, this);

    public void SwipeUp() => OnSwipeInput(SwipeDirection.Up);

    public void SwipeDown() => OnSwipeInput(SwipeDirection.Down);

    public void SwipeLeft() => OnSwipeInput(SwipeDirection.Left);

    public void SwipeRight() => OnSwipeInput(SwipeDirection.Right);

    #endregion

    public void OnTapInput(Vector2 position) => interactor.OnTapInput(position);

    public void IncreaseZAxis() => visualizer.CurrentZAxis++;

    public void DecreaseZAxis() => visualizer.CurrentZAxis--;

    public void Undo() => undoer.Undo();

    public void GetHint()
    {
        int rule = level.grid.GetHint(out int ruleInfo);
        Debug.Log($"r:{rule} i:{ruleInfo}");
        if (rule == -1)
            return;
        info.SetHintInfoText(rule);
        visualizer.HighlightLine(ruleInfo);
    }

    private void CheckSolved(Vector3Int pos)
    {
        if (fullGridRoutine != null)
            StopCoroutine(fullGridRoutine);
        foreach (int item in level.grid)
        {
            if (item == 0)
                return;
        }
        
        fullGridRoutine = StartCoroutine(FullGridRoutine());                  
    }

    private IEnumerator FullGridRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        int rule = level.grid.GetHint(out int ruleInfo);
        if (rule == -1)
        {
            interactor.AllowInput = false;
            score.score += level.grid.Tiles;
            info.SetWinInfoText();
            foreach (GameObject GO in objectsToDisable)
                GO.SetActive(false);
            OnCubeSolved?.Invoke();
            yield return new WaitForSeconds(2.0f);
            ReturnToMainMenu();
        }
        else
        {
            info.SetHintInfoText(rule);
            visualizer.HighlightLine(ruleInfo);
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(2);
    }
}