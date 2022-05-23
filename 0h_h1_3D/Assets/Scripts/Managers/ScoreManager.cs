//Author: Dominik Dohmeier
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text score_Text;

    [Header("Score Ticker")]
    [SerializeField, Range(0.01f, 0.2f)] private float scoreTickInterval;
    [SerializeField] private float scoreTicks;

    private Records record;

    private void Start()
    {
        record = RecordsLoader.LoadRecords();
        UpdateScore();
        RecordsLoader.SaveRecords(record);
    }

    private IEnumerator TickScoreUp()
    {
        float start = record.oldScore;
        float target = record.score;
        float step = (target - record.oldScore) / scoreTicks;
        record.oldScore = record.score;

        yield return new WaitForSeconds(1.0f);

        for (float i = start; i < target; i += step)
        {
            score_Text.text = $"Score {(int)i}";
            yield return new WaitForSeconds(scoreTickInterval);
        }
        score_Text.text = $"Score {record.score}";
    }

    private void UpdateScore()
    {
        score_Text.text = $"Score {record.oldScore}";
        if (record.score != record.oldScore)
            StartCoroutine(TickScoreUp());
    }
}