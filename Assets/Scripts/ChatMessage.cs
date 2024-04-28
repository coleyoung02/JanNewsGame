using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    public static int LINE_BUFFER = 3;

    public void InitializeMessage(string messageString)
    {
        textMeshProUGUI.text = messageString;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (textMeshProUGUI.fontSize + LINE_BUFFER) * GetNumLines());
    }

    public int GetNumLines()
    {
        // Calculate the number of lines by dividing the preferred height by the line height
        return Mathf.RoundToInt(textMeshProUGUI.preferredHeight / textMeshProUGUI.fontSize);
    }
}
