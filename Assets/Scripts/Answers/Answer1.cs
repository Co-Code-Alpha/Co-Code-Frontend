using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer1 : MonoBehaviour
{
    public static Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static bool Check()
    {
        return player.GetComponent<PlayerCollider>().currentCollision.CompareTag("Answer");
    }

   
}
