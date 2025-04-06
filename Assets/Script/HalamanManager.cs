using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class HalamanManager : MonoBehaviour
{
    public bool isEscapeToExit;
    private SerialPort serialPort;

    void Start()
    {
        // Sesuaikan port COM dengan Arduino Anda
        serialPort = new SerialPort("COM6", 9600);
        serialPort.Open();
        StartCoroutine(ReadSerialData());
    }

    IEnumerator ReadSerialData()
    {
        while (true)
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    string data = serialPort.ReadLine();
                    if (data.Contains("START"))
                    {
                        MulaiPermainan();
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
            yield return new WaitForSeconds(0.05f);
        }
    }

    void OnDestroy()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isEscapeToExit)
            {
                Application.Quit();
            }
            else
            {
                KembaliKeMenu();
            }
        }
    }

    public void MulaiPermainan()
    {
        SceneManager.LoadScene("Main");
    }

    public void KembaliKeMenu()
    {
        SceneManager.LoadScene("Intro");
    }

    public void tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void option()
    {
        SceneManager.LoadScene("Option");
    }
}
