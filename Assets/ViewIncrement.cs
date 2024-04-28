using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewIncrement : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    private float fadeClock;
    [SerializeField] private float moveSpeed;
    [SerializeField] private TextMeshProUGUI tm;
    [SerializeField] private RectTransform rt;
    [SerializeField] private Color initialColor;
    private Color clearColor;
    private int number = 0;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        Color c = initialColor;
        c.a = 0f;
        clearColor = c;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = rt.position;
        if (number > 0)
        {
            pos.y += moveSpeed * Time.deltaTime;
        }
        if (number < 0)
        {
            pos.y -= moveSpeed * Time.deltaTime;
        }
        rt.position = pos;
        fadeClock += Time.deltaTime;
        if (fadeClock > fadeTime)
        {
            Destroy(gameObject);
        }
        else
        {
            tm.color = Color.Lerp(initialColor, clearColor, fadeClock / fadeTime);
        }
    }

    public void SetNumber(int n)
    {
        number = n;
        if (n>0)
        {
            tm.text = "+" + number.ToString("N0");
        }
        else if (n<0)
        {
            tm.text = number.ToString("N0");
        }
    }
}
