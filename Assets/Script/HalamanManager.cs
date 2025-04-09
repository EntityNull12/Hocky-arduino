using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO.Ports;

public class HalamanManager : MonoBehaviour
{
    public bool isEscapeToExit;
    private SerialPort serialPort;

    public GameObject panelPause;
    public GameObject tombolPause;
    private bool isPaused = false;

    void Start()
    {
        // Sesuaikan port COM dengan Arduino Anda
        serialPort = new SerialPort("COM6", 9600);
        serialPort.Open();
        StartCoroutine(ReadSerialData());

        if (panelPause != null)
        {
            panelPause.SetActive(false); 
        }

        Time.timeScale = 1f;
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
            else if (SceneManager.GetActiveScene().name == "Main")
            {
                TogglePause(); // Panggil fungsi pause jika di scene Main
            }
            else
            {
                KembaliKeMenu();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Hentikan waktu game
        panelPause.SetActive(true); // Tampilkan panel pause
        tombolPause.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Jalankan waktu game
        panelPause.SetActive(false); // Sembunyikan panel pause
        isPaused = false;
        tombolPause.SetActive(true);
    }

    public void MulaiPermainan()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void MulaiPermainanApril()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("AprilMain");
    }

    public void MulaiPermainanHP()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("HPMain");
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
