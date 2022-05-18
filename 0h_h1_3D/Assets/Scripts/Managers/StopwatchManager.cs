using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class StopwatchManager : MonoBehaviour
{
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
        //save best time
    }
}