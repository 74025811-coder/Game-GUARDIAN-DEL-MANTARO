using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

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
    }

    public void SetMusicVolume(float value01)
    {
        PlayerPrefs.SetFloat(MusicKey, value01);
        // Cuando tengamos el AudioMixer (Bloque 8-9), aquí se conecta.
    }

    public void SetSFXVolume(float value01)
    {
        PlayerPrefs.SetFloat(SFXKey, value01);
        // Cuando tengamos el AudioMixer (Bloque 8-9), aquí se conecta.
    }

    public float GetMusicVolume() => PlayerPrefs.GetFloat(MusicKey, 0.75f);
    public float GetSFXVolume() => PlayerPrefs.GetFloat(SFXKey, 0.75f);
}