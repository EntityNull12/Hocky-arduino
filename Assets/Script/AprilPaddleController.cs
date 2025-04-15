using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class AprilPaddleController : MonoBehaviour
{
    public string paddleIdentifier;
    public string portName = "COM6";
    private static SerialPort serialPort;
    private float currentPosition = 0f;
    private float paddleSpeed;
    private float smoothSpeed;
    private bool isReversed; // Untuk mengontrol arah paddle

    private float currentSpeed;
    private Vector3 lastPosition;

    public float upperBoundary = 3f;
    public float lowerBoundary = -3f;
    public float defaultPaddleSpeed = 10f;
    public float maxPaddleSpeed = 50f;

    void Start()
    {
        // Menentukan apakah paddle akan terbalik (70% kemungkinan)
        isReversed = Random.value <= 0.7f;
        if (isReversed)
        {
            Debug.Log($"{paddleIdentifier} Paddle: Movement Reversed!");
        }

        paddleSpeed = Mathf.Min(PlayerPrefs.GetFloat("PaddleSpeed", defaultPaddleSpeed), maxPaddleSpeed);
        smoothSpeed = paddleSpeed;
        lastPosition = transform.position;

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

        float keyboardInput = 0f;
        if (paddleIdentifier == "Left")
        {
            keyboardInput = Input.GetAxis("vertical1") * paddleSpeed * Time.deltaTime *2;
        }
        else if (paddleIdentifier == "Right")
        {
            keyboardInput = Input.GetAxis("vertical2") * paddleSpeed * Time.deltaTime *2;
        }
        currentPosition += keyboardInput;

        // Reverse keyboard input jika isReversed true
        float movement = Input.GetAxis("Horizontal") * paddleSpeed * Time.deltaTime;
        if (isReversed) movement *= -1;
        transform.Translate(movement, 0, 0);

        currentSpeed = ((transform.position - lastPosition).magnitude) / Time.deltaTime;
        lastPosition = transform.position;
        Debug.Log($"Paddle Info - Set Speed: {paddleSpeed}, Smooth Speed: {smoothSpeed}, Actual Speed: {currentSpeed:F2}");

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

                    float targetPosition;
                    if (isReversed)
                    {
                        // Membalik input dari potentiometer
                        targetPosition = ((1023f - potValue) / 1023f) * 10f - 5f;
                    }
                    else
                    {
                        targetPosition = (potValue / 1023f) * 10f - 5f;
                    }

                    currentPosition = Mathf.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothSpeed);
                    
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
        currentPosition = Mathf.Clamp(currentPosition, lowerBoundary, upperBoundary);
        transform.position = new Vector3(transform.position.x, currentPosition, transform.position.z);
    }

    // Metode lainnya tetap sama
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDestroy()
    {
        ClosePort();
    }

    public void SetPaddleSpeed(float speed)
    {
        paddleSpeed = speed;
        smoothSpeed = speed;
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
