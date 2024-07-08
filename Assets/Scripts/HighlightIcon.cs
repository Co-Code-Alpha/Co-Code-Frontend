using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HighlightIcon : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().rectTransform.DOScale(0.7f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
