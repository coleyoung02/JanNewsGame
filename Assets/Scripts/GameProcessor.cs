using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProcessor : MonoBehaviour
{
    [SerializeField] private LoadArticle loader;
    [SerializeField] private GameObject lives;
    [SerializeField] private GameObject x;
    [SerializeField] private TextMeshProUGUI viewerUI;
    [SerializeField] private ChatManager chatManager;
    [SerializeField] private Slider timer;
    [SerializeField] private TextMeshProUGUI timerClock;
    [SerializeField] private TextMeshProUGUI streamClock;
    [SerializeField] private Slider streamTimer;
    bool currentReal;
    bool articlesRemain = true;
    int viewers = 10000;
    private float maxTime = 45f;
    private float currentTime = 0f;
    private float streamMinutes = 0f;
    private float streamMaxMinutes = 5f;
    private int strikes = 0;
    private bool doUpdate;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
        loader.Initialize();
        currentReal = loader.PopulateArticle();
    }

    public int GetStrikes()
    {
        return strikes;
    }

    // Update is called once per frame
    void Update()
    {
        if (doUpdate)
        {
            if (currentTime <= 0f)
            {
                Guess(!currentReal);
            }
            else
            {
                currentTime -= Time.deltaTime;
                UpdateTimer();
            }
            if (streamMinutes >= streamMaxMinutes * 60f)
            {
                Debug.Log("stream over");
            }
            else
            {
                streamMinutes += Time.deltaTime * 6;
                UpdateStreamTimer();
            }
        }
    }

    private void UpdateTimer()
    {
        timer.value = currentTime;
        int seconds = Mathf.RoundToInt(currentTime);
        if (seconds < 10)
        {
            timerClock.text = "0:0" + seconds.ToString();
        }
        else
        {
            timerClock.text = "0:" + seconds.ToString();
        }
    }

    private void UpdateStreamTimer()
    {
        streamTimer.value = streamMinutes;
        int hours = (int)streamMinutes / 60;
        int minutes = (int)streamMinutes % 60;
        if (minutes < 10)
        {
            streamClock.text = hours + ":0" + minutes.ToString();
        }
        else
        {
            streamClock.text = hours + ":" + minutes.ToString();
        }
    }

    private void LoadNext()
    {
        viewerUI.text = viewers.ToString("N0");
        currentReal = loader.PopulateArticle();
        currentTime = maxTime;
        UpdateTimer();
    }

    public void Guess(bool real)
    {
        if (real == currentReal)
        {
            Correct();
        }
        else
        {
            Incorrect();
        }
    }

    public void Correct()
    {
        if (currentReal)
        {
            chatManager.SpamReal();
        }
        else
        {
            chatManager.SpamFake();
        }
        viewers += 100;
        PersistantManager.Instance.SetMaxViewers(viewers);
        LoadNext();
    }

    public void Incorrect()
    {
        Debug.Log(currentReal);
        strikes += 1;
        if (strikes == 3)
        {

        }
        Instantiate(x, lives.transform);
        viewers *= 3;
        viewers /= 4;
        chatManager.SpamError();
        LoadNext();
    }

    public void FinishedLast()
    {
        return;
    }

    
}
