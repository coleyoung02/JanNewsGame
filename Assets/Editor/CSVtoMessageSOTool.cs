using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
public class CSVToArticleSO : EditorWindow
{
    [MenuItem("Tools/CSV to ScriptableObject Converter")]
    public static void ShowWindow()
    {
        GetWindow(typeof(CSVToArticleSO), false, "CSV to ScriptableObject Converter");
    }

    private TextAsset csvFile;
    private List<ArticleSO> messageDataList = new List<ArticleSO>();

    private void OnGUI()
    {
        GUILayout.Label("Select a .csv file:", EditorStyles.boldLabel);
        csvFile = EditorGUILayout.ObjectField(csvFile, typeof(TextAsset), false) as TextAsset;

        if (GUILayout.Button("Convert"))
        {
            ConvertCSVToScriptableObjects();
        }
    }

    private void ConvertCSVToScriptableObjects()
    {
        if (csvFile == null)
        {
            Debug.LogError("No .csv file selected!");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // Start from index 1 to skip the header
        {
            string[] fields = lines[i].Split(',');
            List<string> buzzwords = new List<string>();
            if (fields.Length >= 2)
            {
                Debug.Log("article " + i.ToString());
                for (int j = 0; j < fields.Length; ++j)
                {
                    Debug.Log("field " + j + " " + fields[j]);
                }

                string truthiness = fields[0].Replace("\r", "");
                string headline = fields[1].Replace("\r", "");
                string date = fields[2].Replace("\r", "");
                string subheading = fields[3];
                for (int j = 4; j < fields.Length; j++)
                {
                    if (fields[j].Replace("\r", "").Length > 0)
                    {
                        buzzwords.Add(fields[j].Replace("\r", ""));
                    }
                }
                ArticleSO messageData = ScriptableObject.CreateInstance<ArticleSO>();
                messageData.truthiness = truthiness.Equals("TRUE");
                messageData.headline = headline;
                messageData.date = date;
                messageData.subheading = subheading;
                messageData.buzzwords = buzzwords;

                string assetPath = "Assets/Articles/" + i + "_" + headline + ".asset";
                AssetDatabase.CreateAsset(messageData, assetPath);

                messageDataList.Add(messageData);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("CSV file converted to ScriptableObjects successfully!");
    }
}

#endif