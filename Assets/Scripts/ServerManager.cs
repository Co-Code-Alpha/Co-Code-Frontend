using System;
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
        Debug.Log(request.downloadHandler.text);
        DestroyLoadPanel();
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
        
        Debug.Log(request.downloadHandler.text);
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