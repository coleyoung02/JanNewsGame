using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Slider streamTimer;
    [SerializeField] private TextMeshProUGUI streamClock;
    [SerializeField] private Color green;
    [SerializeField] private Color red;
    [SerializeField] private JanController jan;
    [SerializeField] private GameObject plus;
    [SerializeField] private GameObject minus;
    [SerializeField] private RectTransform plusSpot;
    [SerializeField] private RectTransform minusSpot;
    //[SerializeField] private TextMeshProUGUI streamClock;
    bool currentReal;
    bool articlesRemain = true;
    int viewers = 10000;
    private float maxTime = 45f;
    private float currentTime = 0f;
    private float streamMinutes = 0f;
    private float streamMaxMinutes = 5f;
    private int strikes = 0;
    private bool doUpdate = false;
    private float multiplier = 1f;
    private float multiplierFactor = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = maxTime;
        loader.Initialize();
        streamMinutes = streamMaxMinutes * 60f;
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
            if (streamMinutes <= 0)
            {
                SceneManager.LoadScene("Win");
            }
            else
            {
                streamMinutes -= Time.deltaTime;
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
        AudioManager.Instance.PlaySuccess();
        if (currentReal)
        {
            chatManager.SpamReal();
        }
        else
        {
            chatManager.SpamFake();
        }
        AddViewers();
        LoadNext();
    }

    private void AddViewers()
    {
        int toAdd = Mathf.RoundToInt(100 * multiplier);
        viewers += toAdd;
        multiplier *= multiplierFactor;
        ShowChange(toAdd);
        PersistantManager.Instance.SetMaxViewers(viewers);

    }

    public void Incorrect(bool timeout)
    {
        AudioManager.Instance.PlayError();
        strikes += 1;
        int sub;
        if (strikes == 3)
        {
            doUpdate = false;
            StartCoroutine(LoseStream());
            Instantiate(x, lives.transform);
            multiplier = 1;
            sub = Mathf.RoundToInt(viewers * .25f);
            viewers -= sub;
            multiplier = 1;
            ShowChange(-sub);
            chatManager.SpamError();
            return;
        }
        Instantiate(x, lives.transform);
        multiplier = 1;
        sub = Mathf.RoundToInt(viewers * .25f);
        viewers -= sub;
        multiplier = 1;
        ShowChange(-sub);
        chatManager.SpamError();
        LoadNext();
    }

    private void ShowChange(int n)
    {
        if (n > 0)
        {
            Instantiate(plus, plusSpot).GetComponent<ViewIncrement>().SetNumber(n);
        }
        else
        {
            Instantiate(minus, minusSpot).GetComponent<ViewIncrement>().SetNumber(n);
        }
    }

    private IEnumerator LoseStream()
    {
        yield return new WaitForSeconds(7f);
        PersistantManager.Instance.Lose();
    }

    public void FinishedLast()
    {
        doUpdate = false;
    }
}
