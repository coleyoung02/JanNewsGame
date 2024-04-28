using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class LoadArticle : MonoBehaviour
{
    [SerializeField] private HeadlineNormalizer headline;
    [SerializeField] private HeadlineNormalizer subheading;
    [SerializeField] private TextMeshProUGUI author;
    [SerializeField] private TextMeshProUGUI date;
    [SerializeField] private TextMeshProUGUI articleBody;

    [SerializeField] private List<ArticleSO> articles;
    private int articleIndex;


    // unserialize and read from csv later
    [SerializeField] private TextAsset csvFile;
    private List<string> firstNames;
    private List<string> lastNames;


    public void Initialize()
    {
        articles = articles.OrderBy(x => Random.value).ToList();
        firstNames = new List<string>();
        lastNames = new List<string>();
        SetNames();
    }

    public bool PopulateArticle()
    {
        if (articleIndex > articles.Count - 1)
        {
            FindFirstObjectByType<GameProcessor>().FinishedLast();
            return true;
        }
        headline.InitializeMessage(articles[articleIndex].headline);
        if (articles[articleIndex].subheading.Length > 0)
        {
            subheading.InitializeMessage(articles[articleIndex].subheading);
            subheading.gameObject.SetActive(true);
        }
        else
        {
            subheading.gameObject.SetActive(false);
        }
        author.text = firstNames[UnityEngine.Random.Range(0, firstNames.Count)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Count)];
        date.text = articles[articleIndex].date;
        FillBody(articles[articleIndex].buzzwords);
        return articles[articleIndex++].truthiness;
    }

    private void FillBody(List<string> buzzes)
    {
        string b = "";
        int word_max_length = 14;
        int sentence_min_length = 5;
        int senetence_max_length = 20;
        int paragraph_min_length = 2;
        if (buzzes.Count >= 4)
        {
            paragraph_min_length = 5;
            sentence_min_length = 6;
        }
        int paragraph_max_length = 7;
        int senetence_length;
        int word_length;
        int paragraph_length;
        //paragraphs
        for (int p = 0; p < 2; p++)
        {
            paragraph_length = UnityEngine.Random.Range(paragraph_min_length, paragraph_max_length);
            b += "\t";

            //sentences
            for (int i = 0; i < paragraph_length; ++i)
            {
                senetence_length = UnityEngine.Random.Range(sentence_min_length, senetence_max_length);

                //words
                for (int j = 0; j < senetence_length; ++j)
                {
                    b += " ";
                    word_length = UnityEngine.Random.Range(1, word_max_length);
                    if (word_length < 3 || word_length > 8)
                    {
                        word_length = UnityEngine.Random.Range(1, word_max_length);
                    }
                    if (word_length > 10)
                    {
                        word_length = UnityEngine.Random.Range(1, word_max_length);
                    }
                    if (buzzes.Count <= 2)
                    {
                        if (p == 0 && buzzes.Count >= 1 && i == 0 && j == senetence_length / 2)
                        {
                            b += buzzes[0];
                        }
                        else if (p == 1 && buzzes.Count >= 2 && i == 0 && j == senetence_length / 2)
                        {
                            b += buzzes[1];
                        }
                        else
                        {
                            //letters
                            for (int k = 0; k < word_length; ++k)
                            {
                                b += "~";
                            }
                        }
                        
                    }
                    else
                    {
                        if (p == 0 && buzzes.Count >= 1 && i == 0 && j == senetence_length / 2)
                        {
                            b += buzzes[0];
                        }
                        else if (p == 0 && buzzes.Count >= 2 && i == 2 && j == senetence_length / 2)
                        {
                            b += buzzes[1];
                        }
                        else if (p == 0 && buzzes.Count >= 3 && i == 4 && j == senetence_length / 2)
                        {
                            b += buzzes[2];
                        }
                        else if (p == 1 && buzzes.Count >= 4 && i == 0 && j == senetence_length / 2)
                        {
                            b += buzzes[3];
                        }
                        else if (p == 1 && buzzes.Count >= 5 && i == 2 && j == senetence_length / 2)
                        {
                            b += buzzes[4];
                        }
                        else if (p == 1 && buzzes.Count >= 6 && i == 4 && j == senetence_length / 2)
                        {
                            b += buzzes[5];
                        }
                        else
                        {
                            //letters
                            for (int k = 0; k < word_length; ++k)
                            {
                                b += "~";
                            }
                        }
                    }
                    

                    
                }
                b += ". ";
            }
            b += "\n";
        }
        articleBody.text = b;
    }

    private void SetNames()
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header
        {
            string[] fields = lines[i].Split(',');
            if (fields.Length >= 2)
            {
                string first = fields[0];
                string last = fields[1];
                if (first.Length > 1)
                {
                    firstNames.Add(first);
                }
                if (last.Length > 1)
                {
                    lastNames.Add(last);
                }
            }
        }
    }
}
