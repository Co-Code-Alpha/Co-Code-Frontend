using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{
    public string url;
    
    void Start()
    {
        
    }

    IEnumerator Test()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);


        yield return request.SendWebRequest();  // 응답이 올때까지 대기한다.

        if (request.error == null)  // 에러가 나지 않으면 동작.
        {
            // text.text += request.downloadHandler.text;
        }
    }
}