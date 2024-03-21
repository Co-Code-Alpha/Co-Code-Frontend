using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private List<List<Block>> listOfLists = new List<List<Block>>();

    public void MakeList()
    {
        List<Block> newList = new List<Block>();
        listOfLists.Add(newList);
    }
    public void AddBlock(int a, Block block)
    {
        listOfLists[a].Add(block);
    }

    public void AddList(int a, int b)
    {
        listOfLists[a].AddRange(listOfLists[b]);
    }

    /*public void DequeueBlock()
    {
        if (blockList.Count > 0)
        {
            Block dequeuedBlock = blockList.Dequeue();
            PlayBlock(dequeuedBlock);
        }
        else
        {
            return;
        }
    }*/
    

    private void DivideList(int a, int n)
    {
        List<Block> newList = listOfLists[a].GetRange(n,-1);
        listOfLists.Add(newList);
    }

    private void PlayBlock(Block block)
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