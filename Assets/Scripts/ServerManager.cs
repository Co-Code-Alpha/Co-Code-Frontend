using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    public static HttpClient client;
    public string url;
    
    void Start()
    {
        client = new HttpClient();
        
        ConnectionTest();
    }

    private async void ConnectionTest()
    {
        try
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Above three lines can be replaced with new helper method below
            // string responseBody = await client.GetStringAsync(uri);

            Debug.Log(responseBody);
        }
        catch (HttpRequestException e)
        {
            Debug.Log("\nException Caught!");
            Debug.Log("Message :{0} " + e.Message);
        }
    }
}
