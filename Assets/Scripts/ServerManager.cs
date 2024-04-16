using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ServerManager : MonoBehaviour
{
    private string url = "http://13.209.99.100:3000/";
    public GameObject loadPanel;
    private GameObject loadInstance;
    
    void Start()
    {
        StartCoroutine(Test());
    }
    
    // CLASS
    
    [Serializable]
    public class User
    {
        public string userId;
        public string password;
        public string nickname;
        public string email;

        public User(string userId, string password, string nickname, string email)
        {
            this.userId = userId;
            this.password = password;
            this.nickname = nickname;
            this.email = email;
        }
    }
    
    // METHOD

    public void SignUpRequest(string userId, string password, string nickname, string email)
    {
        StartCoroutine(SignUp(userId, password, nickname, email));
    }
    
    IEnumerator SignUp(string id, string password, string nickname, string email)
    {
        User user = new User(id, password, nickname, email);
        string json = JsonUtility.ToJson(user);
        UnityWebRequest request = UnityWebRequest.PostWwwForm(url + "api/auth/join", json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        Debug.Log(request.result);
    }

    IEnumerator Test()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        Debug.Log(request);

        SetLoadPanel("서버 요청 중");

        yield return new WaitForSeconds(1f);
        
        ///yield return request.SendWebRequest();

        DestroyLoadPanel();
    }

    private void SetLoadPanel(string target)
    {
        TMP_Text text = loadPanel.transform.GetChild(1).GetComponent<TMP_Text>();
        text.text = target;
        Canvas canvas = FindObjectOfType<Canvas>();
        loadInstance = Instantiate(loadPanel, canvas.transform);
        Debug.Log("SET LOAD PANEL");
    }

    private void DestroyLoadPanel()
    {
        Destroy(loadInstance);
        loadInstance = null;
    }
}