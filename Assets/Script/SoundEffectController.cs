using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Set volume awal dari settings
        if (GameSettings.instance != null)
        {
            audioSource.volume = GameSettings.instance.sfxVolume;
        }
    }

    void Update()
    {
        // Update volume secara real-time
        if (GameSettings.instance != null)
        {
            audioSource.volume = GameSettings.instance.sfxVolume;
        }
    }
}
