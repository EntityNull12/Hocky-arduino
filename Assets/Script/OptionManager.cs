using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public Slider paddleSpeedSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    [SerializeField] private Slider ballSpeedSlider;
    [SerializeField] private BallController ballController;
    [SerializeField] private HPBallController hpBallController;


    private SerialPort serialPort;
    private string portName = "COM6";

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

            if (hpBallController != null)
            {
                hpBallController.UpdateBallSpeed();
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

        serialPort = new SerialPort(portName, 9600);
        try
        {
            serialPort.Open();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                if (data.Contains("RESTART"))
                {
                    // Handle restart if needed
                }
                else
                {
                    string[] values = data.Split(',');
                    if (values.Length == 2)
                    {
                        // Convert potentiometer values (0-1023) to slider ranges
                        float paddleSpeed = Map(float.Parse(values[0]), 0, 1023,
                            paddleSpeedSlider.minValue, paddleSpeedSlider.maxValue);
                        float ballSpeed = Map(float.Parse(values[1]), 0, 1023,
                            ballSpeedSlider.minValue, ballSpeedSlider.maxValue);

                        // Update sliders
                        paddleSpeedSlider.value = paddleSpeed;
                        ballSpeedSlider.value = ballSpeed;

                        // Save settings
                        SaveSettings();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error reading serial: " + e.Message);
            }
        }
    }

    private float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
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

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
