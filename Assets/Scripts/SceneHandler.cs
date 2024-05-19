using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    void Start()
    {
        
    }

    public void SetTargetScene(string sceneName)
    {
        PlayerPrefs.SetString("TargetScene", sceneName);
    }

    public string GetTargetScene()
    {
        return PlayerPrefs.GetString("TargetScene");
    }
}
