using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Cinemachine;

public class TutorialManager : MonoBehaviour
{
    public GameObject textPanel;
    public TMP_Text tutorialText;
    public CinemachineVirtualCamera virtualCam;
    public GameObject highlightIcon;
    public Michsky.MUIP.ButtonManager playButton;

    private Status status = Status.Seq1;

    private enum Status
    {
        Seq1, Seq2, Seq3
    }
    
    void Start()
    {
        Seq1();
    }

    public void Seq1()
    {
        var seq = DOTween.Sequence();
        seq.Append(tutorialText.DOText("Co-Code에 오신 걸 환영해요!", 2f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
        });
        seq.Append(tutorialText.DOText("Co-Code는 블록 코딩을 활용해 여러 스테이지를 클리어하며 프로그래밍에 대한 이해력을 높여 주는 게임이에요.", 4f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
        });
        seq.Append(tutorialText.DOText("앞으로 저를 움직여 다양한 문제를 풀어나가게 되실 거에요.", 2f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
        });
        seq.Append(tutorialText.DOText("일단 코드 블록의 사용법부터 알아볼까요?", 2f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
            virtualCam.Priority = 9;
            Seq2();
        });
        seq.Play();
    }

    public void Seq2()
    {
        status = Status.Seq2;
        var seq = DOTween.Sequence();
        seq.Append(tutorialText.DOText("코드 블록은 크게 Work, Control, Move 3가지 카테고리로 분류돼요.", 2f));
        seq.AppendInterval(2f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
            highlightIcon.SetActive(true);
        });
        seq.Append(tutorialText.DOText("Work 블록은 제가 물건을 줍고 놓는 등의 행위를 나타내요.", 2f));
        seq.AppendInterval(4f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
            RectTransform rect = highlightIcon.GetComponent<RectTransform>();
            Vector2 newPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - 80f);
            highlightIcon.GetComponent<RectTransform>().anchoredPosition = newPosition;
        });
        seq.Append(tutorialText.DOText("Control 블록은 조건, 반복 등 상황에 맞게 응용할 수 있는 블록이에요.", 3f));
        seq.AppendInterval(4f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
            RectTransform rect = highlightIcon.GetComponent<RectTransform>();
            Vector2 newPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - 80f);
            highlightIcon.GetComponent<RectTransform>().anchoredPosition = newPosition;
        });
        seq.Append(tutorialText.DOText("Move 블록은 제가 걸어가거나 회전하는 이동 관련 행위를 나타내요.", 2f));
        seq.AppendInterval(4f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
            highlightIcon.SetActive(false);
        });
        seq.Append(tutorialText.DOText("코드를 만드실 때는 원하는 블록을 오른쪽 빈 공간에 드래그한 후, 오른쪽 위 Play 버튼을 누르시면 돼요.", 4f));
        seq.AppendInterval(4f);
        seq.AppendCallback(() =>
        {
            tutorialText.text = "";
        });
        seq.Append(tutorialText.DOText("Move 카테고리의 걷기 블록을 1개만 사용해 볼까요?", 2f));
        seq.AppendCallback(() =>
        {
            playButton.onClick.AddListener(() =>
            {
                if (FindObjectOfType<BlockManager>().listOfLists[0].blockList[0].GetComponent<Block>().blockData.name ==
                    "Walk")
                {
                    Debug.Log("SEQ3 GOGO");
                }
                else
                {
                    tutorialText.text = "";
                    tutorialText.DOText("Move 카테고리의 걷기 블록은 위 화살표 아이콘의 블록이에요.", 2f);
                }
            });
        });
        seq.Play();
    }
}
