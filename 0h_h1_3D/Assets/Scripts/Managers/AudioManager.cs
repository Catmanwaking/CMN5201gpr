using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public static void PlayAudio(AudioClip clip)
    {
        instance.audioSource.PlayOneShot(clip);
    }
}
