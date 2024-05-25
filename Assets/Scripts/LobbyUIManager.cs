using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;
using DG.Tweening;
using Unity.VisualScripting;
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
    public Button profileButton;
    public GameObject[] editOnlyObjects;
    public GameObject editButton;
    private bool isEditMode = false;
    public TMP_Text profileNicknameText;
    public Image profileImage;
    public Image profileBackground;
    [Header("Shop Objects")]
    public TMP_Text moneyText;
    public Transform profileIconContent;
    public GameObject profileIconObject;
    public GameObject alertPanel;
    [Header("Q&A Objects")]
    public TMP_InputField chatInput;
    public GameObject chatPrefab;
    public Transform chatContent;
    public Button chatSubmitButton;
    private Transform lastChat;
    [Header("Ranking Objects")] public Button rankingButton;
    public Transform rankingContent;
    public GameObject rankingObject;
    public Transform challengeRankingContent;
    public GameObject challengeRankingObject;
    [Header("Option Objects")]
    public SliderManager backgroundMusicSlider;
    public SliderManager effectMusicSlider;
    public Toggle codeSaveToggle;
    public Button resetButton;
    [Header("Search Objects")]
    public TMP_InputField userSearchInputField;
    public TMP_Text searchResultText;
    public Transform searchResultContent;
    public GameObject searchResultObject;
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

        InitChatInput( );
        InitRanking();
        InitSearch();
        InitProfile();
    }

    // PROFILE UI

    public void InitProfile()
    {
        profileButton.onClick.AddListener(() =>
        {
            server.ProfileRequest();
        });
    }
    
    public void ManageProfile()
    {
        currentPanel.SetActive(false);
        currentPanel = profilePanel;
        currentPanel.SetActive(true);
    }

    public void SetProfile(string nickname, string image, string background)
    {
        Debug.Log(nickname + " / " + image + " / " + background);
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
    
    // SHOP UI
    
    public void ManageShop()
    {
        currentPanel.SetActive(false);
        currentPanel = shopPanel;
        currentPanel.SetActive(true);
        
        server.ShopRequest();
    }

    public void OpenShop(ServerManager.ShopResult result)
    {
        moneyText.text = result.money + "코인 보유 중";
        int currentMoney = result.money;

        Item[] profiles = Resources.LoadAll<Item>("Items/ProfileImage");
        Item[] backgrounds = Resources.LoadAll<Item>("Items/ProfileBackground");
        Item[] items = Resources.LoadAll<Item>("Items/UsableItem");

        for (int i = profileIconContent.childCount - 1; i >= 0; i--)
        {
            Transform child = profileIconContent.GetChild(i);
            Destroy(child.gameObject);
        }
        
        foreach (Item item in profiles)
        {
            GameObject profileIconInstance = Instantiate(profileIconObject, profileIconContent);
            Transform t = profileIconInstance.transform;
            t.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Profile/Image/" + item.id);
            t.GetChild(1).GetComponent<TMP_Text>().text = item.name;

            bool isHave = false;
            foreach (string target in result.item)
            {
                if (target == item.id)
                {
                    isHave = true;
                    t.GetChild(2).GetComponent<Image>().color = new Color(99 / 255f, 123 / 255f, 157 / 255f);
                    t.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = "보유 중";
                }
            }

            if (!isHave)
            {
                t.GetChild(2).GetComponent<Image>().color = new Color(141 / 255f, 189 / 255f, 255 / 255f);
                t.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = item.price.ToString();
                t.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (currentMoney < item.price) return;
                    server.ItemPurchaseRequest(item.id, item.name);
                });
            }
        }
    }

    public void SetPurchaseResult(string item, string money)
    {
        alertPanel.SetActive(true);
        alertPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = item + "\n구매가 완료되었습니다!";
        alertPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "잔액 : " + money + "코인";
    }
    
    // Q&A UI
    
    public void ManageQuestion()
    {
        currentPanel.SetActive(false);
        currentPanel = questionPanel;
        currentPanel.SetActive(true);
    }

    private void InitChatInput( )
    {
        chatInput.onSubmit.AddListener( ( string text ) =>
        {
            SendChat();
            CreateChat( text, true );
            chatInput.text = "";
        } );
        
        chatSubmitButton.onClick.AddListener( ( ) =>
        {
            SendChat();
            CreateChat( chatInput.text, true );
            chatInput.text = "";
        } );
    }
    
    public void SendChat()
    {
        ServerManager manager = FindObjectOfType<ServerManager>();
        string text = chatInput.text;
        manager.QuestionChatRequest( text );
    }

    public void CreateChat(string text, bool isUser)
    {
        GameObject chat = Instantiate(chatPrefab, chatContent);
        TMP_Text targetText = chat.transform.GetChild(1).GetComponent<TMP_Text>();
        targetText.text = text;
        if (isUser)
            chat.transform.GetChild( 0 ).GetComponent<Image>( ).sprite = profileImage.sprite;
        RectTransform rect = chat.GetComponent<RectTransform>();

        int charCount = targetText.text.Length;
        int lineCount = (int)Mathf.Ceil(charCount / 60f);
        if (lineCount > 3)
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, lineCount * 50f);

        Image background = chat.GetComponent<Image>( );
        Image icon = chat.transform.GetChild( 0 ).GetComponent<Image>( );
        background.DOFade( 100 / 255f, 1f );
        icon.DOFade( 1f, 1f );
        targetText.DOFade( 1f, 1f );
    }
    
    // RANKING UI
    
    public void ManageRanking()
    {
        currentPanel.SetActive(false);
        currentPanel = rankingPanel;
        currentPanel.SetActive(true);
    }

    public void InitRanking()
    {
        ServerManager manager = FindObjectOfType<ServerManager>();
        rankingButton.onClick.AddListener(() =>
        {
            manager.RankingRequest();
        });
    }

    public void SetRanking(ServerManager.Rank[] rank)
    {
        foreach (ServerManager.Rank r in rank)
        {
            GameObject rankInstance = Instantiate(rankingObject, rankingContent);
            rankInstance.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Profile/Image/" + r.profileId);
            rankInstance.transform.GetChild(1).GetComponent<TMP_Text>().text = r.nickname;
            rankInstance.transform.GetChild(2).GetComponent<TMP_Text>().text = r.rank.ToString();
            rankInstance.transform.GetChild(3).GetComponent<TMP_Text>().text = r.clearedProblems.ToString() + "문제 해결";
        }
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
    
    // SEARCH UI
    
    public void ManageSearch()
    {
        currentPanel.SetActive(false);
        currentPanel = searchPanel;
        currentPanel.SetActive(true);
    }

    public void InitSearch()
    {
        userSearchInputField.onSubmit.AddListener( ( string text ) =>
        {
            ManageSearch();
            SearchUser( text );
        } );
    }

    public void SearchUser(string user)
    {
        FindObjectOfType<ServerManager>().SearchProfileRequest(user);
    }

    public void SetSearchResult(ServerManager.Search[] search, string user)
    {
        searchResultText.text = user + " 검색 결과";
        
        for (int i = searchResultContent.childCount - 1; i >= 0; i--)
        {
            Transform child = searchResultContent.GetChild(i);
            Destroy(child.gameObject);
        }
        
        foreach (ServerManager.Search s in search)
        {
            GameObject searchInstance = Instantiate(searchResultObject, searchResultContent);
            searchInstance.transform.GetChild(0).GetComponent<Image>().sprite =
                Resources.Load<Sprite>("Profile/Image/" + s.profile);
            searchInstance.transform.GetChild(1).GetComponent<TMP_Text>().text = s.nickname;
            searchInstance.GetComponent<Button>().onClick.AddListener(() =>
            {
                server.SearchedUserProfileRequest(s.nickname);
            });
        }
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
