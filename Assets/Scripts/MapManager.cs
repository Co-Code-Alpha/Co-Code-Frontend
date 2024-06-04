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