using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetFloat("backgroundMusic", -1f) == -1f)
        {
            Debug.Log("NO SAVED OPTION");
            CreateDefaultOption();
        }
    }

    public void CreateDefaultOption()
    {
        PlayerPrefs.SetFloat("backgroundMusic", 1f);
        PlayerPrefs.SetFloat("effectMusic", 1f);
        PlayerPrefs.SetInt("codeSave", 1); // 0 : 저장 X / 1 : 저장 0
        Debug.Log("DEFAULT OPTION CREATED");
    }

    public void ResetOption()
    {
        float a = PlayerPrefs.GetFloat("backgroundMusic");
        float b = PlayerPrefs.GetFloat("effectMusic");
        float c = PlayerPrefs.GetInt("codeSave");
        Debug.Log(a + " / " + b + " / " + c);
        CreateDefaultOption();
    }
    
    public void SaveBackgroundMusic(float target)
    {
        Debug.Log("background save : " + target);
        PlayerPrefs.SetFloat("backgroundMusic", target);
    }

    public void SaveEffectMusic(float target)
    {
        Debug.Log("effect save : " + target);
        PlayerPrefs.SetFloat("effectMusic", target);
    }

    public void SaveCodeSave(bool target)
    {
        Debug.Log("code save : " + target);
        PlayerPrefs.SetInt("codeSave", (target) ? 1 : 0);
    }
}