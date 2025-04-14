using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class HPPaddleController : MonoBehaviour
{
    public string paddleIdentifier;
    public string portName = "COM6";
    private static SerialPort serialPort;
    private float currentPosition = 0f;
    private float paddleSpeed;
    private float smoothSpeed;

    public int maxHP = 500;
    private int currentHP;
    public Text hpText;

    public float upperBoundary = 2.8f;
    public float lowerBoundary = -2.8f;
    public float defaultPaddleSpeed = 10f;
    public float maxPaddleSpeed = 50f;

    public GameObject panelSelesai;
    public Text txPemenang;

    void Start()
    {
        currentHP = maxHP;
        UpdateHPDisplay();

        paddleSpeed = Mathf.Min(PlayerPrefs.GetFloat("PaddleSpeed", defaultPaddleSpeed), maxPaddleSpeed);
        smoothSpeed = paddleSpeed;

        if (serialPort == null || !serialPort.IsOpen)
        {
            try
            {
                serialPort = new SerialPort(portName, 9600);
                serialPort.Open();
                serialPort.ReadTimeout = 50;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error membuka port: " + e.Message);
            }
        }

        if (panelSelesai != null)
        {
            panelSelesai.SetActive(false);
        }
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                if (data.Contains("RESTART"))
                {
                    RestartGame();
                }

                string[] values = data.Split(',');
                if (values.Length >= 2)
                {
                    float potValue = 0f;
                    if (paddleIdentifier == "Left")
                    {
                        float.TryParse(values[0].Trim(), out potValue);
                    }
                    else if (paddleIdentifier == "Right")
                    {
                        float.TryParse(values[1].Trim(), out potValue);
                    }

                    float targetPosition = (potValue / 1023f) * 10f - 5f;
                    currentPosition = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothSpeed);
                    currentPosition = Mathf.Clamp(currentPosition, lowerBoundary, upperBoundary);
                    transform.position = new Vector3(transform.position.x, currentPosition, transform.position.z);
                }
            }
            catch (System.Exception e)
            {
                if (!(e is System.TimeoutException))
                {
                    Debug.LogError("Error membaca data: " + e.Message);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP += damage; // damage is negative
        if (currentHP <= 0)
        {
            EndGame();
        }
        UpdateHPDisplay();
    }

    public void HealDamage(int heal)
    {
        currentHP = Mathf.Min(currentHP + heal, maxHP);
        UpdateHPDisplay();
    }

    void UpdateHPDisplay()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + currentHP.ToString();
        }
    }

    void EndGame()
    {
        string winner = (paddleIdentifier == "Left") ? "Player Kanan" : "Player Kiri";
        panelSelesai.SetActive(true);
        txPemenang.text = winner + " Menang!";
        Time.timeScale = 0f;
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        ClosePort();
    }

    void OnDisable()
    {
        ClosePort();
    }

    void ClosePort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}
