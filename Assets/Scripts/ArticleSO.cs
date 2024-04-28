using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Article", menuName = "Articles/News Article")]
public class ArticleSO : ScriptableObject
{
    public bool truthiness;
    public string headline;
    public string date;
    public string subheading;
    public List<string> buzzwords;
}
