//Author: Dominik Dohmeier
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    [SerializeField] private InfoTextManager info;
    [SerializeField] private GameObject gimbal;

    [SerializeField] private CubeVisualizer visualizer;
    [SerializeField] private CubeInteractor interactor;

    private void Start()
    {
        SetupTutorialLevel();
        visualizer.Initialize(level);
        interactor.Initialize(level);
    }

    public void SetupTutorialLevel()
    {
        int[,,] tutorialGrid = new int[,,]
        {
            {
                { 1,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,1 }
            },
            {
                { 1,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,1 }
            },
            {
                { 1,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,1 }
            },
            {
                { 1,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,0 },
                { 0,0,0,1 }
            }
        };

        level.grid = new CubeGrid(tutorialGrid);
    }

    public void OnSwipeInput(SwipeDirection direction) => throw new NotImplementedException();

    public void OnTapInput(Vector2 position) => throw new NotImplementedException();

    public void IncreaseZAxis() => throw new NotImplementedException();

    public void DecreaseZAxis() => throw new NotImplementedException();
}