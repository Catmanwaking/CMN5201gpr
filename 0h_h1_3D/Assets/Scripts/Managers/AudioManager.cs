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
            LoadOptions();
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
            Destroy(this.gameObject);
    }

    public static void PlayAudio(AudioClip clip, float volume, bool changePitch)
    {
        float pitch = changePitch ? Random.Range(0.9f, 1.1f) : 1.0f;
        instance.SetPitch(pitch);
        instance.audioSource.PlayOneShot(clip, volume);
    }

    public void SetPitch(float pitch)
    {
        audioSource.pitch = pitch;
    }

    public static void SetMute(bool mute)
    {
        instance.audioSource.mute = mute;
    }

    private void LoadOptions()
    {
        Settings settings = SettingsLoader.LoadSettings();
        SetMute(settings.muteSound == 1);
    }
}
