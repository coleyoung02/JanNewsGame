using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool hovered = false;
    private float scale = 1f;
    private float maxScale = 1.05f;
    private float changeRate = .4f;
    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (hovered)
        {
            scale = Mathf.Clamp(scale + changeRate * Time.deltaTime, 1, maxScale);
        }
        else
        {
            scale = Mathf.Clamp(scale - changeRate * Time.deltaTime, 1, maxScale);
        }
        rt.localScale = new Vector3(scale, scale, scale);
    }

    private void OnEnable()
    {
        hovered = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }
}
