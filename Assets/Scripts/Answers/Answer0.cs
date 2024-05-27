using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer0 : MonoBehaviour
{
    void Start()
    {
        
    }

    public static bool Check()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        return (player.position == new Vector3(5f, 0.5f, 0f));
    }
}
