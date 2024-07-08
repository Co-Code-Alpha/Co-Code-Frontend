using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockColliderBottom : MonoBehaviour
{
    public bool isBottom = true;
    public Transform otherBlock;

    private void OnTriggerEnter2D(Collider2D other)
    {
        otherBlock = other.transform.parent;
        if (other.name == "Top")
        {
            isBottom = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        otherBlock = null;
        isBottom = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
