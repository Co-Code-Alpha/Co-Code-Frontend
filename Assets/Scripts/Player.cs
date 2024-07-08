using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 startPos;

    private Quaternion startRot;

    private FadeController fadeController;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        fadeController = FindObjectOfType<FadeController>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Answer") &&
            !transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Wrong") &&
            !transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Correct") &&
            !transform.GetComponent<PlayerCollider>().currentCollision.CompareTag("Tile"))
        {
            DieEvent();
        }*/
    }

    void DieEvent()
    {
        transform.position = startPos;
        transform.rotation = startRot;
        fadeController.Fade();
    }
}
