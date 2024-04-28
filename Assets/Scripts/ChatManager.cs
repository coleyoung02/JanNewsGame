using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private float minChatDelay;
    [SerializeField] private float maxChatDelay;
    [SerializeField] private GameObject chatMessage;
    [SerializeField] private GameObject chatHolder;
    [SerializeField] private TextAsset csvFile;
    private float timer;
    private int count = 0;

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
    private Coroutine runningSpam = null;

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
            PostChat(general[g]);
            ++g;
            g = g % general.Count;
            timer = UnityEngine.Random.Range(minChatDelay, maxChatDelay);
        }
        else 
        { 
            timer -= Time.deltaTime; 
        }
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
        StartCoroutine(Spam(incorrect, 7, 12));
    }

    public void SpamFake()
    {
        StopAllCoroutines();
        StartCoroutine(Spam(fake, 4, 9));
    }

    public void SpamReal()
    {
        StopAllCoroutines();
        StartCoroutine(Spam(real, 3, 6));
    }

    private IEnumerator Spam(List<string> pool, int min, int max)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, .3f));
        int chatsToDo = UnityEngine.Random.Range(min, max);
        for (int i = 0; i < chatsToDo; ++i)
        {
            Debug.Log("chatting");
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
            else
            {
                Debug.Log("no pools hit");
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
                string user = fields[0];
                string message = fields[1];
                string cat = fields[2];
                if (user.Length > 1)
                {
                    users.Add(user);
                }
                if (message.Length > 1)
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
