//Author: Dominik Dohmeier
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Score", fileName = "Score")]
public class ScoreSO : ScriptableObject
{
    public int score;
    public int oldScore;
}