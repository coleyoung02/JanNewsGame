using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] private RawImage timerFill;
    [SerializeField] private TextMeshProUGUI timerClock;
    [SerializeField] private Color green;
    [SerializeField] private Color red;
    [SerializeField] private JanController jan;
    //[SerializeField] private TextMeshProUGUI streamClock;
    [SerializeField] private Slider streamTimer;
    bool currentReal;
    bool articlesRemain = true;
    int viewers = 10000;
    private float maxTime = 45f;
    private float currentTime = 0f;
    private float streamMinutes = 0f;
    private float streamMaxMinutes = 5f;
    private int strikes = 0;
    private bool doUpdate = false;

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

    public void SetDoUpdate(bool updates)
    {
        doUpdate = updates;
    }

    // Update is called once per frame
    void Update()
    {
        if (doUpdate)
        {
            if (currentTime <= 0f)
            {
                Guess(!currentReal, true);
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
                streamMinutes += Time.deltaTime;
                UpdateStreamTimer();
            }
        }
    }

    private void UpdateTimer()
    {
        timer.value = currentTime;
        timerFill.color = Color.Lerp(red, green, Mathf.Max(0, (currentTime - 10f))/ (maxTime - 10f));
        int seconds = Mathf.RoundToInt(currentTime);
        if (seconds < 10)
        {
            timerClock.text = "<mspace=.6em>0:0" + seconds.ToString() + "</mspace>";
        }
        else
        {
            timerClock.text = "<mspace=.6em>0:" + seconds.ToString() + "</mspace>";
        }
    }

    private void UpdateStreamTimer()
    {
        //streamTimer.value = streamMinutes;
        //int hours = (int)streamMinutes / 60;
        //int minutes = (int)streamMinutes % 60;
        //if (minutes < 10)
        //{
        //    streamClock.text = hours + ":0" + minutes.ToString();
        //}
        //else
        //{
        //    streamClock.text = hours + ":" + minutes.ToString();
        //}
    }

    private void LoadNext()
    {
        viewerUI.text = viewers.ToString("N0");
        currentReal = loader.PopulateArticle();
        currentTime = maxTime;
        UpdateTimer();
    }

    public void Guess(bool real, bool timeout)
    {
        if (!doUpdate)
        {
            return;
        }
        if (!timeout)
        {
            jan.PlayReaction(real, real == currentReal);
        }
        if (real == currentReal)
        {
            Correct();
        }
        else
        {
            Incorrect(timeout);
        }
    }

    public void Guess(bool real)
    {
        Guess(real, false);
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

    public void Incorrect(bool timeout)
    {
        Debug.Log(currentReal);
        strikes += 1;
        if (strikes == 3)
        {
            doUpdate = false;
            StartCoroutine(LoseStream());
        }
        Instantiate(x, lives.transform);
        viewers *= 3;
        viewers /= 4;
        chatManager.SpamError();
        LoadNext();
    }

    private IEnumerator LoseStream()
    {
        yield return new WaitForSeconds(3f);
        PersistantManager.Instance.Lose();
    }

    public void FinishedLast()
    {
        return;
    }

    
}
