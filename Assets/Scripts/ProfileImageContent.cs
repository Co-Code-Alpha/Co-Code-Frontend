using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImageContent : MonoBehaviour
{
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        rectTransform.sizeDelta = new Vector2(width, width);
    }
}
