using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemManager : MonoBehaviour
{
    public int problemNum;

    public bool isCorrect = false;
    public bool isWrong = false;

    public TMP_Text title;
    public TMP_Text desc;

    public Transform player;

    public Animator anim;

    public FadeController fadeController;
    
    void Start()
    {
        PlayerPrefs.SetString("currentProblemId", "1");
        player = GameObject.FindWithTag("Player").transform;
        fadeController = FindObjectOfType<FadeController>();
        anim = GameObject.FindWithTag("box").GetComponent<Animator>();
        SetDescription();
    }

    public void SetDescription()
    {
        string problemId = PlayerPrefs.GetString("currentProblemId");

        switch (problemId)
        {
            case "0" :
                title.text = "목적지까지 도착하기.";
                desc.text = "주어진 이동 관련 블럭을 사용하여 목적지까지 도착해보세요!";
                break;
            case "1" :
                title.text = "주어진 자료형에 해당하는 변수 찾기.";
                desc.text = "주어진 자료형인 Int에 해당하는 변수가 써진 발판 위로 올라가보세요!";
                break;
            case "2" :
                title.text = "알맞은 연산자 찾기.";
                desc.text = "따옴표에 들어갈 알맞은 연산자가 써진 발판 위로 올라가보세요!";
                break;
            case "3" :
                title.text = "멀리 있는 목적지까지 도착하기.";
                desc.text = "주어진 이동 관련 블럭과 반복문을 사용하여 목적지까지 도착해보세요!";
                break;
            case "4" :
                title.text = "막혀있지 않은 길을 통해 목적지까지 도착하기.";
                desc.text = "두 개의 갈림길 중 하나는 막혀있습니다. 조건문을 사용해 올바른 길을 찾아보세요.";
                break;
            case "5" :
                title.text = "배열에서 해당하는 인덱스의 위치 찾기.";
                desc.text = "주어진 5개의 발판은 크기가 5인 배열입니다. 3번 인덱스의 위치에 해당하는 발판 위로 올라가보세요!";
                break;
        }
    }
    public void CheckAnswer()
    {
        string problemId = PlayerPrefs.GetString("currentProblemId");
        bool isSolved;

        if (player.GetComponent<PlayerCollider>().currentCollision.CompareTag("Answer"))
        {
            FindObjectOfType<IngameUIManager>().SetSolved();
            anim.SetBool("isClear", true);
        }
        else Fail();
    }

    public void Checking()
    {
        if (player.GetComponent<PlayerCollider>().currentCollision.CompareTag("grass") || 
            player.GetComponent<PlayerCollider>().currentCollision.CompareTag("Wrong")) Fail();
    }
    
    public void Fail()
    {
        fadeController.Fade();
        Invoke("FailDelayed", 1f);
    }

    void FailDelayed()
    {
        string problemId = PlayerPrefs.GetString("currentProblemId");
        if (problemId == "3")
        {
            player.transform.position = new Vector3(-3, 4, 3);
        }
        else 
        {        
            player.transform.position = new Vector3(-1, 4, 0);
        }
        player.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
}
