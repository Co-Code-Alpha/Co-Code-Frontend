using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetFloat("backgroundMusic", -1f) == -1f)
            CreateDefaultOption();
    }

    public void CreateDefaultOption()
    {
        PlayerPrefs.SetFloat("backgroundMusic", 1f);
        PlayerPrefs.SetFloat("effectMusic", 1f);
        PlayerPrefs.SetInt("codeSave", 1); // 0 : 저장 X / 1 : 저장 0
    }

    public void ResetOption()
    {
        CreateDefaultOption();
    }
    
    public void SaveBackgroundMusic(float target)
    {
        PlayerPrefs.SetFloat("backgroundMusic", target);
    }

    public void SaveEffectMusic(float target)
    {
        PlayerPrefs.SetFloat("effectMusic", target);
    }

    public void SaveCodeSave(int target)
    {
        PlayerPrefs.SetInt("codeSave", target);
    }
}