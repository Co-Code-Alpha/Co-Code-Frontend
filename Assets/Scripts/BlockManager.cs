using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Queue<Block> blockQueue = new Queue<Block>();

    public void EnqueueBlock(Block block)
    {
        blockQueue.Enqueue(block);
    }

    public void DequeueBlock()
    {
        if (blockQueue.Count > 0)
        {
            Block dequeuedBlock = blockQueue.Dequeue();
            PlayBlock(dequeuedBlock);
        }
        else
        {
            return;
        }
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