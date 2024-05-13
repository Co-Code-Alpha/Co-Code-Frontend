using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class IngameUIManager : MonoBehaviour
{
    [Header("Block Window Objects")]
    public GameObject workWindow;
    public GameObject controlWindow;
    public GameObject moveWindow;
    private GameObject currentWindow;
    public Michsky.MUIP.ButtonManager workButton;
    public Michsky.MUIP.ButtonManager controlButton;
    public Michsky.MUIP.ButtonManager moveButton;
    private bool isFolded = false;
    public Image foldIcon;
    public Sprite downImage;
    public Sprite upImage;
    public GameObject blockWindow;
    
    void Start()
    {
        InitBlockWindow();
    }

    public void InitBlockWindow()
    {
        workButton.onClick.AddListener(() =>
        {
            OpenBlockWindow(workWindow);
        });
        
        controlButton.onClick.AddListener(() =>
        {
            OpenBlockWindow(controlWindow);
        });
        
        moveButton.onClick.AddListener(() =>
        {
            OpenBlockWindow(moveWindow);
        });
        
        OpenBlockWindow(workWindow);
    }

    public void OpenBlockWindow(GameObject target)
    {
        if(currentWindow != null) currentWindow.SetActive(false);
        currentWindow = target;
        currentWindow.SetActive(true);
    }

    public void FoldBlockWindow()
    {
        isFolded = !isFolded;
        blockWindow.transform.DOMoveY(isFolded ? blockWindow.transform.position.y - 230f : blockWindow.transform.position.y + 230f, 1f);
        if (isFolded) foldIcon.sprite = upImage;
        else foldIcon.sprite = downImage;
    }
}
