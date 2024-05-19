using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;
using DG.Tweening;
using UnityEngine.Timeline;

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
    public TMP_Text profileNicknameText;
    public Image profileImage;
    public Image profileBackground;
    [Header("Q&A Objects")]
    public TMP_InputField chatInput;
    public GameObject chatPrefab;
    public Transform chatContent;
    [Header("Option Objects")]
    public SliderManager backgroundMusicSlider;
    public SliderManager effectMusicSlider;
    public Toggle codeSaveToggle;
    public Button resetButton;
    [Header("ETC")]
    public Image userImage;
    public TMP_Text userNickname;
    private GameObject currentPanel;

    private ServerManager server;
    
    void Start()
    {
        currentPanel = profilePanel;
        server = FindObjectOfType<ServerManager>();
        server.ProfileRequest();
        
        ConnectOptionManager();

        CreateChat("kajdflkajsdflkjasdlkfajsakldjflakjdflkadsjflkasjdflkasjdfklasldfnaskldfjaklsdfjaklsejfdklasjdfklasjdklfsajdlkfasjdfklajsdfklajsdkflsajdfkljsdlkjajsdklfajsdflkajsdklfasjdflkajdsflkfdjdklgsjdlgkjasldkgjasldkgjasdlkgjaslkdgjsldkgjd");
    }

    // PROFILE UI
    
    public void ManageProfile()
    {
        currentPanel.SetActive(false);
        currentPanel = profilePanel;
        currentPanel.SetActive(true);
    }

    public void SetProfile(string nickname, string image, string background)
    {
        profileNicknameText.text = nickname;
        profileImage.sprite = Resources.Load<Sprite>("Profile/Image/" + image);
        profileBackground.sprite = Resources.Load<Sprite>("Profile/Background/" + background);
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
        
        server.ShopRequest();
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
    
    // Q&A UI

    public void SendChat()
    {
        ServerManager manager = FindObjectOfType<ServerManager>();
        string text = chatInput.text;
    }

    public void CreateChat(string text)
    {
        GameObject chat = Instantiate(chatPrefab, chatContent);
        TMP_Text targetText = chat.transform.GetChild(1).GetComponent<TMP_Text>();
        targetText.text = text;
        RectTransform rect = chat.GetComponent<RectTransform>();

        int charCount = targetText.text.Length;
        int lineCount = (int)Mathf.Ceil(charCount / 60f);
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, lineCount * 50f);
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
    
    // ETC

    public void TrySignOut()
    {
        PlayerPrefs.DeleteKey("token");
        SceneHandler handler = FindObjectOfType<SceneHandler>();
        handler.SetTargetScene("Title");
        SceneManager.LoadScene("Load");
    }
}
