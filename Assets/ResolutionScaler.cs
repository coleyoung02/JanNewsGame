using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.currentResolution.width / 2, Mathf.RoundToInt(Screen.currentResolution.height * .9445f), false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
