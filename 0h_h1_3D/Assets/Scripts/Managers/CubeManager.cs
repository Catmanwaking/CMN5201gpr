//Author: Dominik Dohmeier
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private InfoTextManager info;
    [SerializeField] private CubeVisualizer visualizer;
    [SerializeField] private CubeInteractor interactor;

    [SerializeField] private UnityEvent OnCubeSolved;

    private CubeUndoer undoer;
    private Coroutine fullGridRoutine;

    private void Start()
    {
        if (level.grid == null)
            level.grid = new CubeGrid(2);

        visualizer.Initialize(level);
        interactor.Initialize(level);
        undoer = new CubeUndoer(level);
        level.grid.OnTileChanged += CheckSolved;
        info.SetDefaultText();
    }

    #region SwipeInput

    public void OnSwipeInput(SwipeDirection direction) => visualizer.OnSwipeInput(direction, this);

    #endregion

    public void OnTapInput(Vector2 position) => interactor.OnTapInput(position);

    public void IncreaseZAxis() => visualizer.CurrentZAxis++;

    public void DecreaseZAxis() => visualizer.CurrentZAxis--;

    public void Undo() => undoer.Undo();

    public void GetHint()
    {
        int rule = level.grid.GetHint(out int ruleInfo);
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
            CubeSolved();
        else
        {
            info.SetHintInfoText(rule);
            visualizer.HighlightLine(ruleInfo);
        }
    }

    private void CubeSolved()
    {
        interactor.AllowInput = false;
        UpdateScore();
        info.SetWinInfoText();
        OnCubeSolved?.Invoke();       
    }

    private void UpdateScore()
    {
        Records recrods = RecordsLoader.LoadRecords();
        recrods.score += level.grid.Tiles;
        RecordsLoader.SaveRecords(recrods);
    }   
}