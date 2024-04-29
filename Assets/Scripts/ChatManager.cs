using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private float minChatDelay;
    [SerializeField] private bool startMenu;
    [SerializeField] private float maxChatDelay;
    [SerializeField] private GameObject chatMessage;
    [SerializeField] private GameObject chatHolder;
    [SerializeField] private TextAsset csvFile;
    private float timer;

    private List<string> general;
    private int g = 0;
    private List<string> real;
    private int r = 0;
    private List<string> fake;
    private int f = 0;
    private List<string> incorrect;
    private int i = 0;
    private List<string> lowKarma;
    private int l = 0;
    private List<string> users;

    void Start()
    {
        timer = maxChatDelay;
        InitializeLists();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            if (!startMenu && FindFirstObjectByType<GameProcessor>().GetStrikes() >= 2)
            {
                if (UnityEngine.Random.Range(0f,1f) > .65f)
                {
                    PostLowKarmaChat();
                }
                else
                {
                    PostGeneralChat();
                }
            }
            else if (!startMenu && FindFirstObjectByType<GameProcessor>().GetStrikes() >= 1)
            {
                if (UnityEngine.Random.Range(0f, 1f) > .85f)
                {
                    PostLowKarmaChat();
                }
                else
                {
                    PostGeneralChat();
                }
            }
            else
            {
                PostGeneralChat();
            }
            
            timer = UnityEngine.Random.Range(minChatDelay, maxChatDelay);
        }
        else 
        { 
            timer -= Time.deltaTime; 
        }
    }

    private void PostGeneralChat()
    {
        PostChat(general[g]);
        ++g;
        g = g % general.Count;
    }

    private void PostLowKarmaChat()
    {
        PostChat(lowKarma[l]);
        ++l;
        l = l % lowKarma.Count;
    }

    private void PostChat(string msg)
    {
        ChatMessage cm = Instantiate(chatMessage, chatHolder.transform).GetComponent<ChatMessage>();
        cm.InitializeMessage(users[UnityEngine.Random.Range(0, users.Count)] + ": " + msg);
        cm.transform.SetSiblingIndex(0);
    }

    public void SpamError()
    {
        StopAllCoroutines();
        StartCoroutine(Spam(incorrect, 9, 14));
    }

    public void SpamFake()
    {
        StopAllCoroutines();
        StartCoroutine(Spam(fake, 8, 13));
    }

    public void SpamReal()
    {
        StopAllCoroutines();
        StartCoroutine(Spam(real, 6, 9));
    }

    private IEnumerator Spam(List<string> pool, int min, int max)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.125f, .4f));
        int chatsToDo = UnityEngine.Random.Range(min, max);
        for (int j = 0; j < chatsToDo; ++j)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.07f, .15f));
            if (pool == real)
            {
                PostChat(pool[r]);
                ++r;
                r = r % real.Count;
            }
            else if (pool == fake)
            {
                PostChat(pool[f]);
                ++f;
                f = f % fake.Count;
            }
            else if (pool == incorrect)
            {
                PostChat(pool[i]);
                ++i;
                i = i % incorrect.Count;
            }
        }

    }

    public void InitializeLists()
    {
        general = new List<string>();
        real = new List<string>();
        fake = new List<string>();
        incorrect = new List<string>();
        lowKarma = new List<string>();
        users = new List<string>();

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header
        {
            string[] fields = lines[i].Split(',');
            if (fields.Length >= 2)
            {
                string user = fields[0].Replace("\r", "");
                string message = fields[1].Replace("\r", "");
                string cat = fields[2].Replace("\r", "");
                if (user.Length > 0)
                {
                    users.Add(user);
                }
                if (message.Length > 0)
                {
                    if (cat.Equals("LowKarma"))
                    {
                        lowKarma.Add(message);
                    }
                    else if (cat.Equals("trueArticle"))
                    {
                        real.Add(message);
                    }
                    else if (cat.Equals("falseArticle"))
                    {
                        fake.Add(message);
                    }
                    else if (cat.Equals("Incorrect"))
                    {
                        incorrect.Add(message);
                    }
                    else
                    {
                        general.Add(message);
                    }
                }
            }
        }
    }

}
