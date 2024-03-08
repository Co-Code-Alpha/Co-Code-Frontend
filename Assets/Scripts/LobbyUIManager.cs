using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject profilePanel;
    public GameObject shopPanel;
    public GameObject stagePanel;
    public GameObject challengePanel;
    public GameObject questionPanel;
    public GameObject rankingPanel;
    public GameObject optionPanel;
    public GameObject searchPanel;

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
    
    public void ManageOption()
    {
        currentPanel.SetActive(false);
        currentPanel = optionPanel;
        currentPanel.SetActive(true);
    }
    
    public void ManageSearch()
    {
        currentPanel.SetActive(false);
        currentPanel = searchPanel;
        currentPanel.SetActive(true);
    }
}
