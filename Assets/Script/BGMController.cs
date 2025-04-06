using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        // Pastikan objek ini tidak hancur saat pindah scene
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Set volume awal sesuai settings
        if (GameSettings.instance != null)
        {
            audioSource.volume = GameSettings.instance.bgmVolume;
        }
    }

    void Update()
    {
        // Update volume secara real-time sesuai settings
        if (GameSettings.instance != null)
        {
            audioSource.volume = GameSettings.instance.bgmVolume;
        }
    }
}
