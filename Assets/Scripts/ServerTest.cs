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
        Test();
    }

    IEnumerator Test()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (www.error == null)  // 에러가 나지 않으면 동작.
        {
            text.text += "--------------------\n";
            text.text += www.downloadHandler.text;
        }
    }
}
