//Author: Dominik Dohmeier
using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text score_Text;
    [SerializeField] private ScoreSO score;

    [Header("Score Ticker")]
    [SerializeField, Range(0.01f, 0.2f)] private float scoreTickInterval;
    [SerializeField] private float scoreTicks;

    private void Start()
    {        
        UpdateScore();
    }

    private IEnumerator TickScoreUp()
    {
        float start = score.oldScore;
        float target = score.score;
        float step = (target - score.oldScore) / scoreTicks;
        score.oldScore = score.score;

        yield return new WaitForSeconds(1.0f);

        for (float i = start; i < target; i += step)
        {
            score_Text.text = $"Score {(int)i}";
            yield return new WaitForSeconds(scoreTickInterval);
        }
        score_Text.text = $"Score {score.score}";
    }

    private void UpdateScore()
    {
        score_Text.text = $"Score {score.oldScore}";
        if (score.score != score.oldScore)
            StartCoroutine(TickScoreUp());
    }
}