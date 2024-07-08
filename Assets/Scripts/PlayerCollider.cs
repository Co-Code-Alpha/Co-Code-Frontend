using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public Transform currentCollision;
    
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        currentCollision = other.transform;
    }
}
