using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    void Start()
    {
        //string mapId = PlayerPrefs.GetString("currentMapId");
        string mapId = "2";
        GenerateMap(mapId);
        if (mapId == "3")
        {
            GameObject.FindWithTag("box").transform.rotation = Quaternion.Euler(0, 0, 0);
            Transform player = GameObject.FindWithTag("Player").transform;
            player.transform.position = new Vector3(-3, 4, 3);
        }
        else
        {
            GameObject.FindWithTag("box").transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    private void GenerateMap(string mapId)
    {
        MapEditor.MapData data =
            JsonUtility.FromJson<MapEditor.MapData>(Resources.Load<TextAsset>("Maps/" + mapId).text);

        foreach (MapObject obj in data.data)
        {
            Debug.Log(obj.modelId + " / " + obj.x + " , " + obj.y + " , " + obj.z);
            Vector3 pos = new Vector3(obj.x, obj.y, obj.z);
            GameObject objectInstance = Instantiate(Resources.Load<GameObject>("Models/" + obj.modelId), pos, Quaternion.identity);
        }
    }
}