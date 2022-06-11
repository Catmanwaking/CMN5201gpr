//Author: Dominik Dohmeier
using Fast_0h_h1;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CubeManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private InfoTextManager infoText;
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
        infoText.SetDefaultText();
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
        RuleInfo info = level.grid.GetHint();
        if (info.brokenRule == Rule.None)
            return;
        infoText.SetHintInfoText(info);
        visualizer.HighlightLine(info);
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

        RuleInfo info = level.grid.GetHint();
        if (info.brokenRule == Rule.None)
            CubeSolved();
        else
        {
            infoText.SetHintInfoText(info);
            visualizer.HighlightLine(info);
        }
    }

    private void CubeSolved()
    {
        interactor.AllowInput = false;
        UpdateScore();
        infoText.SetWinInfoText();
        OnCubeSolved?.Invoke();       
    }

    private void UpdateScore()
    {
        Records recrods = RecordsLoader.LoadRecords();
        recrods.score += level.grid.Tiles;
        RecordsLoader.SaveRecords(recrods);
    }   
}