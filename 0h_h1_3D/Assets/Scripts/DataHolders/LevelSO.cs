//Author: Dominik Dohmeier
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Level", fileName = "Level")]
public class LevelSO : ScriptableObject
{
    public CubeGrid grid;
}