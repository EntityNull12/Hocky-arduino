using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class PaddleController : MonoBehaviour
{
    public string paddleIdentifier;
    public string portName = "COM6";
    private static SerialPort serialPort;
    private float currentPosition = 0f;
    public float smoothSpeed = 10f;

    // Batas atas dan bawah untuk paddle
    public float upperBoundary = 2.8f;
    public float lowerBoundary = -2.8f;

    void Start()
    {
        if (serialPort == null || !serialPort.IsOpen)
        {
            try
            {
                serialPort = new SerialPort(portName, 9600);
                serialPort.Open();
                serialPort.ReadTimeout = 50;
                Debug.Log("Port " + portName + " dibuka");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error membuka port: " + e.Message);
            }
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

                    // Batasi posisi paddle
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
            Debug.Log("Port ditutup");
        }
    }
}
