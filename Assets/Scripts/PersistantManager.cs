using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistantManager : MonoBehaviour
{
    public static PersistantManager Instance;
    private static int defaultMaxViewers = 1000;
    private static int maxViewers;
    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Application.runInBackground = true;
    }

    public int GetDefaultMaxViewers()
    {
        return defaultMaxViewers;
    }
    public void SetMaxViewers(int viewers)
    {
        if (viewers > maxViewers)
        {
            maxViewers = viewers;
        }
    }

    public void StartGame()
    {
        maxViewers = defaultMaxViewers;
        SceneManager.LoadScene("JanScene");
    }
    public void MainMenu()
    {
        maxViewers = defaultMaxViewers;
        Debug.Log("load main");
        SceneManager.LoadScene("StartMenu");
    }

    public int GetPeakViewers()
    {
        return maxViewers;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }

    public void Lose()
    {
        SceneManager.LoadScene("Lose");
    }
}
