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
        });
        seq.Play();
    }
}
