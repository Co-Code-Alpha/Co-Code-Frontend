using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;


[Serializable]
public class BlockDataList
{
    public List<BlockData> blockList = new List<BlockData>();
}

public class BlockManager : MonoBehaviour
{
    public GameObject player;
    public float speed = 5f;
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
        StartCoroutine(ExecuteBlock(block));
    }
    
    public IEnumerator PlayBlocks(List<BlockData> blocks)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            yield return StartCoroutine(ExecuteBlock(blocks[i]));
        }
    }

    private IEnumerator ExecuteBlock(BlockData block)
    {
        if (block == null)
        {
            yield break;
        }

        int blockIdx = Int32.Parse(block.num);
        switch (blockIdx)
        {
            case 1:
                yield return StartCoroutine(Walk());
                break;
            case 2:
                yield return StartCoroutine(TurnRight());
                break;
            case 3:
                yield return StartCoroutine(TurnLeft());
                break;
            default:
                break;
        }
    }

    private IEnumerator Walk()
    {
        anim.SetBool("isWalking", true);
        Vector3 currentPosition = playerRigidbody.position;
        Vector3 targetPosition = currentPosition + player.transform.forward * 1f;
        
        float distance = Vector3.Distance(currentPosition, targetPosition);
        float duration = distance / speed;
        
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            playerRigidbody.MovePosition(Vector3.Lerp(currentPosition, targetPosition, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        anim.SetBool("isWalking", false);
    }

    private IEnumerator TurnRight()
    {
        anim.SetBool("rightTurn", true);
        Quaternion fromRotation = player.transform.rotation;
        Quaternion toRotation = player.transform.rotation * Quaternion.Euler(Vector3.up * 90f);
        
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            player.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("rightTurn", false);
    }

    private IEnumerator TurnLeft()
    {
        anim.SetBool("leftTurn", true);
        Quaternion fromRotation = player.transform.rotation;
        Quaternion toRotation = player.transform.rotation * Quaternion.Euler(Vector3.up * -90f);
        
        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            player.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        anim.SetBool("leftTurn", false);
    }

    private IEnumerator RotatePlayer(Vector3 axis, float angle, float duration)
    {
        Quaternion fromRotation = player.transform.rotation;
        Quaternion toRotation = player.transform.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            player.transform.rotation = Quaternion.Slerp(fromRotation, toRotation, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.rotation = toRotation;
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