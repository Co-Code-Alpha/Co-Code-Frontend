using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapObject
{
    public int x;
    public int y;
    public int z;
        
    // 0 Forward / 1 Back / 2 Left / 3 Right / 4 Up / 5 Down
    public int direction;
    public string modelId;

    public bool isTile;
    
    public MapObject(int x, int y, int z, int direction, string id, bool isTile)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.direction = direction;
        this.modelId = id;
        this.isTile = isTile;
    }

    public int GetX()
    {
        return x;
    }

    public void SetX(int x)
    {
        this.x = x;
    }

    public int GetY()
    {
        return y;
    }

    public void SetY(int y)
    {
        this.y = y;
    }

    public int GetZ()
    {
        return z;
    }

    public void SetZ(int z)
    {
        this.z = z;
    }

    public int GetDirection()
    {
        return direction;
    }

    public void SetDirection(int direction)
    {
        this.direction = direction;
    }

    public string GetID()
    {
        return modelId;
    }

    public void SetID(string id)
    {
        this.modelId = id;
    }

    public bool GetTile()
    {
        return isTile;
    }

    public void SetTile(bool isTile)
    {
        this.isTile = isTile;
    }
}

[Serializable]
public class MapObjectPrefab
{
    public string modelId;
    public Sprite icon;
}

public class MapEditor : MonoBehaviour
{
    [Serializable]
    public class MapData
    {
        public int mapId;
        public MapObject[] data;
    }

    public List<MapObjectPrefab> placeablePrefabs;
    public List<MapObject> placedObjects;
    public List<GameObject> placedInstances;

    private MapEditorUIManager uiManager;
    
    void Start()
    {
        uiManager = FindObjectOfType<MapEditorUIManager>();
        uiManager.InitPlaceableUI(placeablePrefabs);
        
        placedObjects = new List<MapObject>();
    }

    public void AddObject(MapObject target)
    {
        placedObjects.Add(target);
        GameObject targetModel = Resources.Load<GameObject>("Models/" + target.modelId);
        Quaternion rotation = GetDirection(target.direction);
        GameObject instance = Instantiate(targetModel, new Vector3(target.x, target.y, target.z), rotation);
        placedInstances.Add(instance);
    }
    
    private Quaternion GetDirection(int idx)
    {
        Debug.Log(idx);
        switch(idx)
        {
            case 0:
                return Quaternion.Euler(0f, 0f, 0f);
            case 1:
                return Quaternion.Euler(0f, 180f, 0f);
            case 2:
                return Quaternion.Euler(0f, -90f, 0f);
            case 3:
                return Quaternion.Euler(0f, 90f, 0f);
            case 4:
                return Quaternion.Euler(90f, 0f, 0f);
            case 5:
                return Quaternion.Euler(-90f, 0f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    public void RemoveObject(MapObject target)
    {
        Debug.Log("OBJECT REMOVE");
        for (int i = 0; i < placedObjects.Count; i++)
        {
            if (placedObjects[i] == target)
            {
                placedObjects.RemoveAt(i);
                Destroy(placedInstances[i]);
                placedInstances.RemoveAt(i);
                uiManager.RemoveInstance(i);
                return;
            }
        }
        Debug.Log("NO OBJECT");
    }

    public bool CheckPlaced(int x, int y, int z)
    {
        foreach (MapObject obj in placedObjects)
            if (obj.x == x && obj.y == y && obj.z == z) return true;

        return false;
    }

    public void SaveToJson()
    {
        MapData data = new MapData();
        data.mapId = 1;
        data.data = placedObjects.ToArray();
        string json = JsonUtility.ToJson(data);
        
        Debug.Log(json);
    }
}