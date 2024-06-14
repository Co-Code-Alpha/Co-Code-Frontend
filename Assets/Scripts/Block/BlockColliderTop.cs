using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockColliderTop : MonoBehaviour
{
    public Transform otherBlock;
    public int ohterBlockListIndex = -1;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!transform.parent.GetComponent<DragDrop>().isClicked)
        {
            return;
        }
        otherBlock = other.transform.parent;
        transform.parent.GetComponent<DragDrop>().isAttaching = true;
        float offset = otherBlock.gameObject.GetComponent<RectTransform>().rect.height / 2;
        transform.parent.GetComponent<DragDrop>().attachPosition = otherBlock.transform.position;
        transform.parent.GetComponent<DragDrop>().attachPosition.y -=
            offset + (transform.gameObject.GetComponent<RectTransform>().rect.height / 2);
        ohterBlockListIndex = otherBlock.GetComponent<DragDrop>().listIndex;
        transform.parent.GetComponent<DragDrop>().listIndex = ohterBlockListIndex;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.parent.GetComponent<DragDrop>().isAttaching = false;
        //ohterBlockListIndex = -1;
        otherBlock = null;
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
