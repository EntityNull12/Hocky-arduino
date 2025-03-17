using UnityEngine;
using System.IO.Ports;

public class PaddleController : MonoBehaviour
{
    public string paddleIdentifier;
    public string portName = "COM6";
    private static SerialPort serialPort;
    private float currentPosition = 0f;

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
                Debug.Log("Data mentah: " + data);

                string[] values = data.Split(',');
                if (values.Length >= 2)
                {
                    float potValue = 0f;
                    if (paddleIdentifier == "Left")
                    {
                        float.TryParse(values[0].Trim(), out potValue);
                        Debug.Log("Nilai Paddle Kiri: " + potValue);
                    }
                    else if (paddleIdentifier == "Right")
                    {
                        float.TryParse(values[1].Trim(), out potValue);
                        Debug.Log("Nilai Paddle Kanan: " + potValue);
                    }

                    // Mapping nilai potensio (0-1023) ke posisi paddle (-5 sampai 5)
                    currentPosition = (potValue / 1023f) * 10f - 5f;
                    transform.position = new Vector3(transform.position.x, currentPosition, transform.position.z);
                }
            }
            catch (System.Exception e)
            {
                // Ignore timeout exceptions
                if (!(e is System.TimeoutException))
                {
                    Debug.LogError("Error membaca data: " + e.Message);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Port ditutup");
        }
    }
}
