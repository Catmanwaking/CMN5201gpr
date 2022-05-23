//Author: Dominik Dohmeier
using UnityEngine;

public class AudioHolder : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    public void PlaySound(int ID)
    {
        AudioManager.PlayAudio(clips[ID]);
    }
}