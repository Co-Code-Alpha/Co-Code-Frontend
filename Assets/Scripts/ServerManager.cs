using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class ServerManager : MonoBehaviour
{
    private string url = "http://13.209.99.100:3000/";
    public GameObject loadPanel;
    private GameObject loadInstance;

    public DateTime ConvertDateTime(string target)
    {
        return DateTime.Parse(target, null, DateTimeStyles.RoundtripKind);
    }
    
    public int GetWeekOfMonth(DateTime dt)
    {
        DateTime now = dt;
        int basisWeekOfDay = (now.Day - 1) % 7;
        int thisWeek = (int)now.DayOfWeek;
        double val = Math.Ceiling((double)now.Day / 7);
        if (basisWeekOfDay > thisWeek) val++;
        return Convert.ToInt32(val);
    }
    
    void Start()
    {
        
    }
    
    // JSON CONVERTION

    [Serializable]
    class TokenData
    {
        public string token;
    }

    [Serializable]
    class ProfileData
    {
        public string nickname;
        public string profile;
        public string background;
        public string[] item;
    }

    private string CreateJsonString(string[] keyValuePairs)
    {
        if (keyValuePairs.Length % 2 != 0)
        {
            Debug.LogError("Key-value pairs array length must be even.");
            return null;
        }

        System.Text.StringBuilder jsonStringBuilder = new System.Text.StringBuilder();
        jsonStringBuilder.Append("{");

        for (int i = 0; i < keyValuePairs.Length; i += 2)
        {
            string key = keyValuePairs[i];
            string value = keyValuePairs[i + 1];

            jsonStringBuilder.AppendFormat("\"{0}\":\"{1}\"", key, value);

            if (i < keyValuePairs.Length - 2)
            {
                jsonStringBuilder.Append(",");
            }
        }

        jsonStringBuilder.Append("}");

        return jsonStringBuilder.ToString();
    }
    
    // METHOD
    
    // ------------------------------
    // 로그인

    public void SignInRequest(string userId, string password)
    {
        StartCoroutine(SignIn(userId, password));
    }
    
    IEnumerator SignIn(string id, string password)
    {
        string[] pair = { "userId", id, "password", password };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/login", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();
        
        string response = request.downloadHandler.text;
        if (response == "WRONG_USER_INFO")
        {
            TitleUIManager uiManager = FindObjectOfType<TitleUIManager>();
            uiManager.SetLoginError("잘못된 회원 정보입니다.");
        }
        else
        {
            TokenData tokenData = JsonUtility.FromJson<TokenData>(response);
            Debug.Log(tokenData.token);
            PlayerPrefs.SetString("token", tokenData.token);
            PlayerPrefs.SetString("userId", id);
            
            SceneHandler handler = FindObjectOfType<SceneHandler>();
            handler.SetTargetScene("Lobby");
            SceneManager.LoadScene("Load");
        }
    }
    
    // ------------------------------

    // ------------------------------
    // 회원가입

    public void SignUpRequest(string userId, string password, string nickname, string email)
    {
        StartCoroutine(SignUp(userId, password, nickname, email));
    }
    
    IEnumerator SignUp(string id, string password, string nickname, string email)
    {
        string[] pair = { "userId", id, "password", password, "nickname", nickname, "email", email };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/join", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        if (request.responseCode == 201)
        {
            TitleUIManager uiManager = FindObjectOfType<TitleUIManager>();
            uiManager.SignUpSuccess(nickname);
        }
        else
        {
            Debug.Log("회원가입 에러 발생");
        }
        
    }
    
    // ------------------------------
    
    // ------------------------------
    // 회원가입 중복 검사

    public void SignUpCheckRequest(string userId, string password, string nickname, string email)
    {
        StartCoroutine(SignUpCheck(userId, password, nickname, email));
    }

    IEnumerator SignUpCheck(string id, string password, string nickname, string email)
    {
        PlayerPrefs.SetString("joinId", id);
        PlayerPrefs.SetString("joinPw", password);
        PlayerPrefs.SetString("joinNick", nickname);
        PlayerPrefs.SetString("joinEmail", email);
        
        string[] pair = { "userId", id, "password", password, "nickname", nickname, "email", email };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/checkJoin", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        if (request.responseCode == 200)
        {
            Debug.Log("중복 X");
            EmailAuthRequest(email, 0);
        }
        else
        {
            TitleUIManager uiManager = FindObjectOfType<TitleUIManager>();
            uiManager.SetSignUpError("중복된 데이터가 존재합니다.");
        }
    }
    
    // ------------------------------

    // ------------------------------
    // 이메일 인증
    // code 0 : 회원가입 시 인증
    // code 1 : 비밀번호 변경 시 인증
    
    public void EmailAuthRequest(string email, int code)
    {
        StartCoroutine(EmailAuth(email, code));
    }

    IEnumerator EmailAuth(string email, int code)
    {
        string[] pair = { "email", email };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/code", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();
        
        PlayerPrefs.SetString("authCode", request.downloadHandler.text);
        TitleUIManager uiManager = FindObjectOfType<TitleUIManager>();

        switch (code)
        {
            case 0 :
                uiManager.SignUpEmailAuth(email);
                break;
            
            case 1 :
                uiManager.PwFindEmailAuthSuccess(email);
                break;
            
            default :
                break;
        }
    }

    // ------------------------------

    // ------------------------------
    // 아이디 찾기

    public void IdFindRequest(string email)
    {
        StartCoroutine(IdFind(email));
    }

    IEnumerator IdFind(string email)
    {
        string[] pair = { "email", email };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/findId", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        TitleUIManager uiManager = FindObjectOfType<TitleUIManager>();
        
        if (request.responseCode == 200)
        {
            uiManager.findBackground.SetActive(false);
            uiManager.Alert(string.Format("해당 이메일로 등록된 ID는\n{0}입니다.", request.downloadHandler.text));
        }
        else
        {
            uiManager.IdFindError("등록되지 않은 이메일입니다.");
        }
    }
    
    // ------------------------------

    // ------------------------------
    // 비밀번호 변경

    public void PwChangeRequest(string email, string password)
    {
        StartCoroutine(PwChange(email, password));
    }

    IEnumerator PwChange(string email, string password)
    {
        string[] pair = { "email", email, "password", password };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/changePw", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        TitleUIManager uiManager = FindObjectOfType<TitleUIManager>();
        
        if (request.responseCode == 200)
        {
            uiManager.findBackground.SetActive(false);
            uiManager.Alert("비밀번호가 성공적으로 변경되었습니다.");
        }
    }
    
    // ------------------------------

    // ------------------------------
    // 프로필 요청

    public void ProfileRequest()
    {
        StartCoroutine(Profile());
    }

    IEnumerator Profile()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "api/lobby/profile");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();
        ProfileData data = JsonUtility.FromJson<ProfileData>(request.downloadHandler.text);

        LobbyUIManager ui = FindObjectOfType<LobbyUIManager>();
        ui.SetProfile(data.nickname, data.profile, data.background);
    }
    
    // ------------------------------

    // ------------------------------
    // 상점 불러오기
    
    public class ShopResult
    {
        public int money;
        public string[] item;
    }

    public void ShopRequest()
    {
        StartCoroutine(Shop());
    }

    IEnumerator Shop()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "api/lobby/shop");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        if (request.responseCode == 200)
        {
            ShopResult result = JsonUtility.FromJson<ShopResult>(request.downloadHandler.text);
            FindObjectOfType<LobbyUIManager>().OpenShop(result);
        }
    }
    
    // ------------------------------

    // ------------------------------
    // Q&A 채팅 전송

    [Serializable]
    public class ChatData
    {
        public string generated_text;
    }

    public void QuestionChatRequest( string text )
    {
        StartCoroutine( QuestionChat( text ) );
    }

    IEnumerator QuestionChat( string text )
    {
        string[] pair = { "instruction", text };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.PostWwwForm("https://9589-34-124-203-232.ngrok-free.app/generate", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        if (request.responseCode == 200)
        {
            ChatData data = JsonUtility.FromJson<ChatData>(request.downloadHandler.text);
            FindObjectOfType<LobbyUIManager>().CreateChat(data.generated_text, false);
        }
    }
    
    // ------------------------------
    
    // ------------------------------
    // 누적 랭킹

    [Serializable]
    public class Rank
    {
        public int rank;
        public string nickname;
        public string profileId;
        public int clearedProblems;
    }

    [Serializable]
    public class RankResponse
    {
        public List<Rank> rank;
    }

    public void RankingRequest()
    {
        StartCoroutine(Ranking());
    }

    IEnumerator Ranking()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "api/lobby/rank");
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();
        
        RankResponse rankResponse = JsonUtility.FromJson<RankResponse>(request.downloadHandler.text);
        FindObjectOfType<LobbyUIManager>().SetRanking(rankResponse.rank.ToArray());
    }
    
    // ------------------------------
    
    // ------------------------------
    // 프로필 검색

    [Serializable]
    public class Search
    {
        public string nickname;
        public string profile;
    }

    [Serializable]
    public class SearchResponse
    {
        public Search[] users;
    }

    public void SearchProfileRequest(string user)
    {
        StartCoroutine(SearchProfile(user));
    }

    IEnumerator SearchProfile(string user)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "api/lobby/search?nickname=" + user);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        if (request.responseCode == 200)
        {
            SearchResponse response = JsonUtility.FromJson<SearchResponse>("{\"users\":" + request.downloadHandler.text + "}");
            FindObjectOfType<LobbyUIManager>().SetSearchResult(response.users, user);
        }
    }
    
    // ------------------------------

    // ------------------------------
    // 타 유저 프로필

    [Serializable]
    public class OtherUser
    {
        public string nickname;
        public string profile;
        public string background;
    }

    public void SearchedUserProfileRequest(string nickname)
    {
        StartCoroutine(SearchedUserProfile(nickname));
    }

    IEnumerator SearchedUserProfile(string nickname)
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "api/lobby/othersProfile?nickname=" + nickname);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        SetLoadPanel("서버 응답 대기 중");
        yield return request.SendWebRequest();
        DestroyLoadPanel();

        if (request.responseCode == 200)
        {
            OtherUser user = JsonUtility.FromJson<OtherUser>(request.downloadHandler.text);
            FindObjectOfType<LobbyUIManager>().SetProfile(user.nickname, user.profile, user.background);
            FindObjectOfType<LobbyUIManager>().ManageProfile();
        }
    }
    
    // ------------------------------

    // ------------------------------
    // 회원 탈퇴

    public void DeleteUserRequest(string email)
    {
        StartCoroutine(DeleteUser(email));
    }

    IEnumerator DeleteUser(string email)
    {
        string[] pair = { "email", email };
        string json = CreateJsonString(pair);
        UnityWebRequest request = UnityWebRequest.Delete(url + "api/auth/deleteId");
        yield return null;
    }
    
    // ------------------------------
    
    // ETC

    private void SetLoadPanel(string target)
    {
        TMP_Text text = loadPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        text.text = target;
        Canvas canvas = FindObjectOfType<Canvas>();
        loadInstance = Instantiate(loadPanel, canvas.transform);
        loadInstance.transform.SetParent(canvas.transform);
    }

    private void DestroyLoadPanel()
    {
        Destroy(loadInstance);
        loadInstance = null;
    }
}