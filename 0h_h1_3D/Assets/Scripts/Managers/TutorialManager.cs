//Author: Dominik Dohmeier
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private InfoTextManager info;
    [SerializeField] private GameObject gimbal;

    [SerializeField] private Vector3Int[] inputOrder;
    [SerializeField] private string[] infoTexts;
    [SerializeField] private int adustCamIndex;
    [SerializeField] private int nextCubeInput = 0;

    [SerializeField] private CubeVisualizer visualizer;
    [SerializeField] private CubeInteractor interactor;

    private CubeUndoer undoer;

    private bool awaitCubeInput = true;
    private bool awaitSwipeInput = false;
    private bool awaitZAxisInput = false;

    private Vector3Int expectedCubeInput;

    private Dictionary<int, SwipeDirection> expectedDirection; //TODO property drawer to avoid hardcoded
    private Dictionary<int, bool> expectedZAxis; //TODO property drawer to avoid hardcoded

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        SetupTutorialLevel();
        SetupHardcodedDictionaries();

        visualizer.Initialize(level);
        interactor.Initialize(level);
        undoer = new CubeUndoer(level);
        level.grid.OnTileChanged += OnAttemptedInput;

        ShowCurrent();
    }

    public void SetupTutorialLevel()
    {
        int[,,] tutorialGrid = new int[,,]
        {
            {
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 1,0,0,0 },
                { 2,0,0,0 }
            },
            {
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 1,0,0,0 },
                { 2,0,0,0 }
            },
            {
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 }
            },
            {
                { 0,0,0,0 },
                { 1,0,0,0 },
                { 2,0,0,0 },
                { 1,0,0,0 }
            }
        };

        level.grid = new CubeGrid(tutorialGrid);
    }

    public void SetupHardcodedDictionaries()
    {
        expectedDirection = new Dictionary<int, SwipeDirection>
        {
            { 10, SwipeDirection.Left }
        };

        expectedZAxis = new Dictionary<int, bool>
        {
            { 20, true }
        };
    }

    public void OnSwipeInput(SwipeDirection direction)
    {
        if (!awaitSwipeInput)
            return;

        if (expectedDirection[nextCubeInput] == direction)
            visualizer.OnSwipeInput(direction, this);
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

        if(expectedZAxis[nextCubeInput] == true)
        {
            visualizer.CurrentZAxis++;
        }
    }

    public void DecreaseZAxis()
    {
        if (!awaitZAxisInput)
            return;

        if (expectedZAxis[nextCubeInput] == false)
        {
            visualizer.CurrentZAxis--;
        }
    }

    private void ShowCurrent()
    {
        if(awaitCubeInput)
        {
            expectedCubeInput = inputOrder[nextCubeInput];
            visualizer.HighlightSingle(expectedCubeInput);
        }
        info.SetText(infoTexts[nextCubeInput], false);
    }

    private void CycleNextInput()
    {
        nextCubeInput++;
        if (adustCamIndex == nextCubeInput)
            visualizer.ResetCameraAngle();
        if(expectedDirection.ContainsKey(nextCubeInput))
        {
            awaitCubeInput = false;
            awaitSwipeInput = true;
        } 
        else if(expectedZAxis.ContainsKey(nextCubeInput))
        {
            awaitCubeInput = false;
            awaitZAxisInput = true;
        } 
        else
        {
            awaitCubeInput = true;
            awaitSwipeInput = false;
            awaitZAxisInput = false;
        }
        ShowCurrent();
    }

    private void OnAttemptedInput(Vector3Int pos)
    {
        level.grid.OnTileChanged -= OnAttemptedInput;

        if (pos != expectedCubeInput)
        {
            undoer.Undo();
            ShowCurrent();
        }
        else
            CycleNextInput();

        level.grid.OnTileChanged += OnAttemptedInput;
    }
}