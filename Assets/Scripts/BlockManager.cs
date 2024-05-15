using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;


[Serializable]
public class BlockDataList
{
    public List<BlockData> blockList = new List<BlockData>();
}

public class BlockManager : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    public Animator anim;
    public Rigidbody playerRigidbody;
    
    [SerializeField]
    public List<BlockDataList> listOfLists = new List<BlockDataList>();

    public void MakeList(BlockData blockData)
    {
        BlockDataList newList = new BlockDataList();
        newList.blockList.Add(blockData);
        listOfLists.Add(newList);
    }
    public void AddBlock(int a, BlockData block)
    {
        listOfLists[a].blockList.Add(block);
    }

    public void AddList(int a, int b, int n)
    {
        List<BlockData> newList = listOfLists[b].blockList.GetRange(n,listOfLists[b].blockList.Count - n);
        listOfLists[a].blockList.AddRange(newList);
        Debug.Log(n);
        listOfLists[b].blockList.RemoveRange(n, listOfLists[b].blockList.Count - n);    
    }
    
    public void DivideList(int a, int n)
    {
        List<BlockData> newList = listOfLists[a].blockList.GetRange(n,listOfLists[a].blockList.Count - n);
        BlockDataList newBlockDataList = new BlockDataList();
        newBlockDataList.blockList = newList;
        listOfLists.Add(newBlockDataList);
        listOfLists[a].blockList.RemoveRange(n, listOfLists[a].blockList.Count - n);
    }

    public void PlayBlock(BlockData block)
    {
        if (block == null)
        {
            return;
        }
        
        switch (block.num)
        {
            case 1:
                Walk();
                anim.SetBool("isWalking", false);
                break;
            case 2:
                TurnRight();
                break;
            case 3:
                TurnLeft();
                break;
            default:
                
                break;
        }
    }

    public void Walk()
    {
        anim.SetBool("isWalking", true);
        Vector3 newPosition = playerRigidbody.position + player.transform.forward * speed * 10f;
        playerRigidbody.MovePosition(newPosition);
    }


    public void TurnRight()
    {
        player.transform.Rotate(Vector3.up, 90f);
    }

    public void TurnLeft()
    {
        player.transform.Rotate(Vector3.up, -90f);
    }

    public void PickUp()
    {
        
    }

    public void Drop()
    {
        
    }

    public void If()
    {
        
    }

    public void Loop()
    {
        
    }

    private void Start()
    {
        anim = player.GetComponent<Animator>();
        playerRigidbody = player.GetComponent<Rigidbody>();
    }
}