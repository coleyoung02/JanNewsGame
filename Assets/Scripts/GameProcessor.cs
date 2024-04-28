using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameProcessor : MonoBehaviour
{
    [SerializeField] private LoadArticle loader;
    [SerializeField] private GameObject lives;
    [SerializeField] private GameObject x;
    [SerializeField] private TextMeshProUGUI viewerUI;
    [SerializeField] private ChatManager chatManager;
    bool currentReal;
    bool articlesRemain = true;
    int viewers = 10000;

    // Start is called before the first frame update
    void Start()
    {
        loader.Initialize();
        currentReal = loader.PopulateArticle();
    }

    private void LoadNext()
    {
        viewerUI.text = viewers.ToString("N0");
        currentReal = loader.PopulateArticle();
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
        LoadNext();
    }

    public void Incorrect()
    {
        Debug.Log(currentReal);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
