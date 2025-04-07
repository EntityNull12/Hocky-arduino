using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance;

    public float paddleSpeed = 10f; // Default speed
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public float ballSpeed = 400f; // Nilai default

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
        paddleSpeed = PlayerPrefs.GetFloat("PaddleSpeed", 5f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        ballSpeed = PlayerPrefs.GetFloat("BallSpeed", 300f);
    }
    public void ResetToDefaults()
    {
        paddleSpeed = 5f;
        bgmVolume = 0.5f;
        sfxVolume = 0.5f;
        ballSpeed = 300f;
        SaveSettings();
    }

}
