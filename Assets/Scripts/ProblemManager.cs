using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemManager : MonoBehaviour
{
    public int problemNum;

    public bool isCorrect = false;
    public bool isWrong = false;

    public Transform player;
    
    void Start()
    {
        PlayerPrefs.SetString("currentProblemId", "1");
        player = GameObject.FindWithTag("Player").transform;
    }

    public void CheckAnswer()
    {
        string problemId = PlayerPrefs.GetString("currentProblemId");
        bool isSolved;

        switch (problemId)
        {
            case "0" :
                problemNum = 0;
                isSolved = Answer0.Check();
                if(isSolved)
                    FindObjectOfType<IngameUIManager>().SetSolved();
                break;
            
            case "1" :
                problemNum = 1;
                isSolved = Answer1.Check();
                if (!isCorrect || isWrong) isSolved = false;
                if(isSolved)
                    FindObjectOfType<IngameUIManager>().SetSolved();
                break;
            
            
            default :
                break;
        }
    }

    public void Checking()
    {
        string problemId = PlayerPrefs.GetString("currentProblemId");

        switch (problemId)
        {
            case "0" :
                problemNum = 0;
                break;
            
            case "1" :
                problemNum = 1;
                if (player.GetComponent<PlayerCollider>().currentCollision.CompareTag("Correct")) isCorrect = true;
                if (player.GetComponent<PlayerCollider>().currentCollision.CompareTag("Wrong")) isWrong = true;
                break;
            
            default :
                break;
        } 
    }
}
