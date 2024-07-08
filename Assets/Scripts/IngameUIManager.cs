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
    public GameObject solvedWindow;
    public TMP_Text solvedText;
    public TMP_Text conversionText;
    public Button cButton;
    public Button pythonButton;
    public Button javaButton;
    public Button exitButton;
    public GameObject generatedCode;
    public TMP_Text codeText;
    
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

    public void SetSolved()
    {
        var seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.AppendCallback(() =>
        {
            solvedWindow.SetActive(true);
        });
        seq.Append(solvedText.DOFade(1f, 1f));
        seq.AppendInterval(1f);
        seq.Append(solvedText.DOFade(0f, 1f));
        seq.AppendCallback(() =>
        {
            solvedText.gameObject.SetActive(false);
            conversionText.DOFade(1f, 1f);
            cButton.GetComponent<Image>().DOFade(1f, 1f);
            pythonButton.GetComponent<Image>().DOFade(1f, 1f);
            javaButton.GetComponent<Image>().DOFade(1f, 1f);
            exitButton.GetComponent<Image>().DOFade(1f, 1f);
            cButton.transform.GetChild(0).GetComponent<TMP_Text>().DOFade(1f, 1f);
            pythonButton.transform.GetChild(0).GetComponent<TMP_Text>().DOFade(1f, 1f);
            javaButton.transform.GetChild(0).GetComponent<TMP_Text>().DOFade(1f, 1f);
            exitButton.transform.GetChild(0).GetComponent<TMP_Text>().DOFade(1f, 1f);

            string code = "";
            foreach(GameObject i in FindObjectOfType<BlockManager>().listOfLists[0].blockList)
            {
                BlockData data = i.GetComponent<BlockData>();
                switch (data.name)
                {
                    case "Walk":
                        code += "걷다 ";
                        break;
                    case "Turn Right":
                        code += "오른쪽으로 회전하다 ";
                        break;
                    case "Turn Left":
                        code += "왼쪽으로 회전하다 ";
                        break;
                    case "Loop" :
                        code += "지금부터";
                        break;
                    case "LoopEnd" :
                        code += "여기까지 ";
                        break;
                    default:
                        break;
                }
            }
            
            cButton.onClick.AddListener(() =>
            {
                FindObjectOfType<ServerManager>().GenerateCodeRequest(code + " 를 C++ 코드로 쉽게 변환해 줘");
            });
            
            pythonButton.onClick.AddListener(() =>
            {
                Debug.Log("GOGOGO");
                FindObjectOfType<ServerManager>().GenerateCodeRequest(code + " 를 파이썬 코드로 쉽게 변환해 줘");
            });
            
            javaButton.onClick.AddListener(() =>
            {
                FindObjectOfType<ServerManager>().GenerateCodeRequest(code + " 를 Java 코드로 쉽게 변환해 줘");
            });
        });

        seq.Play();
    }

    public void GenerateTest()
    {
        FindObjectOfType<ServerManager>().GenerateCodeRequest("앞으로 이동 5번 를 파이썬 코드로 쉽게 변환해 줘");
    }

    public void SetCode(string text)
    {
        generatedCode.SetActive(true);
        codeText.text = text;
    }
}
