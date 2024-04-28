using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeadlineNormalizer : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    public void InitializeMessage(string messageString)
    {
        textMeshProUGUI.text = messageString;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (textMeshProUGUI.fontSize) * GetNumLines() + 20);
    }

    public int GetNumLines()
    {
        // Calculate the number of lines by dividing the preferred height by the line height
        return Mathf.RoundToInt(textMeshProUGUI.preferredHeight / textMeshProUGUI.fontSize);
    }
}
