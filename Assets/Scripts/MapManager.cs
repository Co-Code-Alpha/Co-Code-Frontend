using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<GameObject> generated;

    void Start()
    {
        //GenerateMap();
    }

    /*private Quaternion GetDirection(MapObject.Direction direction)
    {
        switch(direction)
        {
            case MapObject.Direction.Forward:
                return Quaternion.Euler(0f, 0f, 0f);
            case MapObject.Direction.Back:
                return Quaternion.Euler(0f, 180f, 0f);
            case MapObject.Direction.Left:
                return Quaternion.Euler(0f, -90f, 0f);
            case MapObject.Direction.Right:
                return Quaternion.Euler(0f, 90f, 0f);
            case MapObject.Direction.Up:
                return Quaternion.Euler(90f, 0f, 0f);
            case MapObject.Direction.Down:
                return Quaternion.Euler(-90f, 0f, 0f);
            default:
                return Quaternion.identity;
        }
    }

    public void GenerateMap()
    {
        generated = new List<GameObject>();
        
        foreach(MapObject obj in objects)
        {
            Vector3 pos = new Vector3(obj.x, obj.y, obj.z);
            GameObject instance = Instantiate(obj.model, pos, GetDirection(obj.direction));
            generated.Add(instance);
        }
    }*/
}