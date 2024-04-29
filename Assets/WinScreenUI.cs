using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreenUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tm;

    // Start is called before the first frame update
    void Start()
    {
        tm.text = "Peak Viewers: " + PersistantManager.Instance.GetPeakViewers().ToString("N0");
    }
}
