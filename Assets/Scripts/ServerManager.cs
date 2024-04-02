using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ServerManager : MonoBehaviour
{
    public string url;
    public GameObject loadPanel;
    private GameObject loadInstance;
    
    void Start()
    {
        SetLoadPanel("연결 확인 중");
    }

    IEnumerator Test()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);


        yield return request.SendWebRequest();

        if (request.error == null)
        {
            // text.text += request.downloadHandler.text;
        }
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