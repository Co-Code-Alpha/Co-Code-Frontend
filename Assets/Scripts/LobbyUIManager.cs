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
    [Header("Option Objects")]
    public SliderManager backgroundMusicSlider;
    public SliderManager effectMusicSlider;
    public Toggle codeSaveToggle;
    
    private GameObject currentPanel;
    
    void Start()
    {
        currentPanel = profilePanel;
    }

    public void ManageProfile()
    {
        currentPanel.SetActive(false);
        currentPanel = profilePanel;
        currentPanel.SetActive(true);
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
