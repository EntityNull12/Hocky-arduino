using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    public float paddleSpeed = 10f; // Default speed
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    public float ballSpeed = 5f; // Nilai default

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("PaddleSpeed", paddleSpeed);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("BallSpeed", ballSpeed);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        paddleSpeed = PlayerPrefs.GetFloat("PaddleSpeed", 10f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
}
