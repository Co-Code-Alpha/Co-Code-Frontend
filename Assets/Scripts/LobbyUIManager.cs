using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;

public class LobbyUIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject profilePanel;
    public GameObject shopPanel;
    public GameObject stagePanel;
    public GameObject challengePanel;
    public GameObject questionPanel;
    public GameObject rankingPanel;
    public GameObject optionPanel;
    public GameObject searchPanel;
    [Header("Profile Objects")]
    public GameObject[] editOnlyObjects;
    public GameObject editButton;
    private bool isEditMode = false;
    [Header("Option Objects")]
    public SliderManager backgroundMusicSlider;
    public SliderManager effectMusicSlider;
    public Toggle codeSaveToggle;
    public Button resetButton;
    
    private GameObject currentPanel;
    
    void Start()
    {
        currentPanel = profilePanel;
        
        ConnectOptionManager();
    }

    // PROFILE UI
    
    public void ManageProfile()
    {
        currentPanel.SetActive(false);
        currentPanel = profilePanel;
        currentPanel.SetActive(true);
    }

    public void EnterEditMode()
    {
        editButton.SetActive(false);
        isEditMode = true;
        foreach(GameObject obj in editOnlyObjects)
            obj.SetActive(isEditMode);
    }

    public void ExitEditMode()
    {
        editButton.SetActive(true);
        isEditMode = false;
        foreach(GameObject obj in editOnlyObjects)
            obj.SetActive(isEditMode);
    }

    public void SaveEdit()
    {
        editButton.SetActive(true);
        isEditMode = false;
        foreach(GameObject obj in editOnlyObjects)
            obj.SetActive(isEditMode);
    }
    
    public void ManageShop()
    {
        currentPanel.SetActive(false);
        currentPanel = shopPanel;
        currentPanel.SetActive(true);
    }
    
    public void ManageStage()
    {
        currentPanel.SetActive(false);
        currentPanel = stagePanel;
        currentPanel.SetActive(true);
    }
    
    public void ManageChallenge()
    {
        currentPanel.SetActive(false);
        currentPanel = challengePanel;
        currentPanel.SetActive(true);
    }
    
    public void ManageQuestion()
    {
        currentPanel.SetActive(false);
        currentPanel = questionPanel;
        currentPanel.SetActive(true);
    }
    
    public void ManageRanking()
    {
        currentPanel.SetActive(false);
        currentPanel = rankingPanel;
        currentPanel.SetActive(true);
    }
    
    // OPTION UI
    
    public void ManageOption()
    {
        currentPanel.SetActive(false);
        currentPanel = optionPanel;
        currentPanel.SetActive(true);
    }

    public void ConnectOptionManager()
    {
        OptionManager optionManager = FindObjectOfType<OptionManager>();
        Debug.Log("OPTION MANAGER CONNECT : " + optionManager);
        
        resetButton.onClick.AddListener(() =>
        {
            optionManager.ResetOption();
        });

        codeSaveToggle.isOn = PlayerPrefs.GetInt("codeSave") == 1;
        codeSaveToggle.onValueChanged.AddListener(delegate(bool arg)
        {
            optionManager.SaveCodeSave(arg);
        });

        backgroundMusicSlider.mainSlider.value = PlayerPrefs.GetFloat("backgroundMusic");
        backgroundMusicSlider.mainSlider.onValueChanged.AddListener(optionManager.SaveBackgroundMusic);

        effectMusicSlider.mainSlider.value = PlayerPrefs.GetFloat("effectMusic");
        effectMusicSlider.mainSlider.onValueChanged.AddListener(optionManager.SaveEffectMusic);
    }

    public void ResetOptionUI()
    {
        backgroundMusicSlider.mainSlider.value = 1f;
        effectMusicSlider.mainSlider.value = 1f;
        codeSaveToggle.isOn = true;
    }
    
    public void ManageSearch()
    {
        currentPanel.SetActive(false);
        currentPanel = searchPanel;
        currentPanel.SetActive(true);
    }
}
