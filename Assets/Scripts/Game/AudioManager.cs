using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField, Range(0f, 1f)] private float maxMusicVolume = 0.35f;

    private const string MusicKey = "MusicVolume";
    private const string SFXKey = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ApplyMusicVolume(GetMusicVolume());
        ApplySFXVolume(GetSFXVolume());

        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float value01)
    {
        PlayerPrefs.SetFloat(MusicKey, value01);
        ApplyMusicVolume(value01);
    }

    public void SetSFXVolume(float value01)
    {
        PlayerPrefs.SetFloat(SFXKey, value01);
        ApplySFXVolume(value01);
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat(MusicKey, 0.75f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFXKey, 0.75f);

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }

    private void ApplyMusicVolume(float value01)
    {
        if (musicSource != null) musicSource.volume = value01 * maxMusicVolume;
    }

    private void ApplySFXVolume(float value01)
    {
        if (sfxSource != null) sfxSource.volume = value01;
    }
}