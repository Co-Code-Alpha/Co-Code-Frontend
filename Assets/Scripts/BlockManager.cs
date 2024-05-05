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

    private void PlayBlock(BlockData block)
    {
        if (block == null)
        {
            return;
        }

        if (block is MoveBlock)
        {
            MoveBlock moveBlock = (MoveBlock)block;
            moveBlock.Walk();
        }
        else if (block is ControlBlock)
        {
            ControlBlock controlBlock = (ControlBlock)block;
            controlBlock.Repeat();
        }
        else if (block is WorkBlock)
        {
            WorkBlock workBlock = (WorkBlock)block;
            workBlock.PickUp();
        }
        else
        {
            return;
        }
    }
}