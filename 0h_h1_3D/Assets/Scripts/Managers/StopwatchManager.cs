using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class StopwatchManager : MonoBehaviour
{
    [SerializeField] private LevelSO level;
    private TMP_Text stopwatch_Text;
    private TimeSpan timeSpan;
    private float startTime;
    private float pausedTime;
    private float pausedStartTime;

    private bool timerActive;

    public bool TimerActive
    {
        get => timerActive;
        set 
        {
            timerActive = value;
            if (!timerActive)
                pausedStartTime = Time.time;
            else
                pausedTime = Time.time - pausedStartTime;
        }
    }

    private void Awake()
    {
        stopwatch_Text = GetComponent<TMP_Text>();
        timerActive = true;
        startTime = Time.time;
    }

    private void Update()
    {
        if (timerActive)
        {
            timeSpan = TimeSpan.FromSeconds(Time.time - startTime - pausedTime);
            stopwatch_Text.text = timeSpan.ToString(@"m\:ss"); 
        }
    }

    public void StopTimer()
    {
        timerActive = false;
        int size = level.grid.SideLength;
        Records records = RecordsLoader.LoadRecords();

        switch (size)
        {
            case 4:
                if (!records.recordSize4Exists || records.recordSize4 > timeSpan.TotalSeconds)
                {
                    records.recordSize4 = timeSpan.TotalSeconds;
                    records.recordSize4Exists = true;
                }
                break;
            case 6:
                if (!records.recordSize6Exists || records.recordSize6 > timeSpan.TotalSeconds)
                {
                    records.recordSize6 = timeSpan.TotalSeconds;
                    records.recordSize6Exists = true;
                }
                break;
        }

        RecordsLoader.SaveRecords(records);
    }
}