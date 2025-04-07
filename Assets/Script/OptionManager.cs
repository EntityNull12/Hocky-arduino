using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider paddleSpeedSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    [SerializeField] private Slider ballSpeedSlider;
    [SerializeField] private BallController ballController;

    void Start()
    {

        bgmVolumeSlider.onValueChanged.AddListener((value) => {
            GameSettings.instance.bgmVolume = value;
            // Langsung simpan perubahan
            GameSettings.instance.SaveSettings();
        });
        // Inisialisasi nilai slider dari GameSettings
        ballSpeedSlider.value = GameSettings.instance.ballSpeed;
        ballSpeedSlider.onValueChanged.AddListener((value) => {
            GameSettings.instance.ballSpeed = value;

            // Update force di BallController
            if (ballController != null)
            {
                ballController.UpdateForce(value);
            }
        });
        //paddleSpeedSlider.value = GameSettings.instance.paddleSpeed;
        bgmVolumeSlider.value = GameSettings.instance.bgmVolume;
        sfxVolumeSlider.onValueChanged.AddListener((value) => {
            GameSettings.instance.sfxVolume = value;
            GameSettings.instance.SaveSettings();
        });
        paddleSpeedSlider.onValueChanged.AddListener((value) => {
            GameSettings.instance.paddleSpeed = value;
        });
    }

    public void SaveSettings()
    {
        // Update nilai di GameSettings
        GameSettings.instance.paddleSpeed = paddleSpeedSlider.value;
        GameSettings.instance.bgmVolume = bgmVolumeSlider.value;
        GameSettings.instance.sfxVolume = sfxVolumeSlider.value;
        GameSettings.instance.ballSpeed = ballSpeedSlider.value;

        // Simpan ke PlayerPrefs
        GameSettings.instance.SaveSettings();
    }
}
