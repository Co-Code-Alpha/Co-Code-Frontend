using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemManager : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetString("currentProblemId", "0");
    }

    public void CheckAnswer()
    {
        string problemId = PlayerPrefs.GetString("currentProblemId");

        switch (problemId)
        {
            case "0" :
                bool isSolved = Answer0.Check();
                Debug.Log(isSolved);
                break;
            
            default :
                break;
        }
    }
}