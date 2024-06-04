using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Answer") &&
            !transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Wrong") &&
            !transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Correct") &&
            !transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Tile"))
        {
            DieEvent();
        }
    }

    void DieEvent()
    {
        
    }
}
