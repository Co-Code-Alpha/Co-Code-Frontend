using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ServerTest : MonoBehaviour
{
    public TMP_Text text;
    public string url;
    
    void Start()
    {
        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        string json =
            "{\"mapId\":1,\"data\":[{\"x\":0,\"y\":0,\"z\":0,\"direction\":0,\"modelId\":\"test_1\"},{\"x\":0,\"y\":0,\"z\":1,\"direction\":0,\"modelId\":\"test_1\"},{\"x\":0,\"y\":0,\"z\":2,\"direction\":0,\"modelId\":\"test_1\"},{\"x\":0,\"y\":1,\"z\":0,\"direction\":0,\"modelId\":\"test_2\"}]}";

        UnityWebRequest request = UnityWebRequest.PostWwwForm(url, json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");


        yield return request.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (request.error == null)  // 에러가 나지 않으면 동작.
        {
            text.text += "--------------------\n";
            text.text += request.downloadHandler.text;
        }
    }
}
