using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool isDropped = false;
    public bool isClicked = false;
    public bool isAttaching = false;
    public bool onWindow = false;
    public bool divideCheck = false;
    public bool addCheck = false;
    
    public static Vector2 defaultPos;
    public Vector3 attachPosition;
    
    public BlockManager blockManager;
    public BlockColliderTop blockColliderTop;
    public BlockColliderBottom blockColliderBottom;
    public Block blockScript;
    
    private GameObject coppiedBlock;
    private GameObject codeWindow;
    
    public int listIndex;
    public int blockIndex;
    public int prevIndex;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        onWindow = false;
        defaultPos = this.transform.position;
        isClicked = true;
        if (isAttaching)
        {
            divideCheck = true;
            addCheck = false;
        }
        else
        {
            divideCheck = false;
            addCheck = true;
            prevIndex = listIndex;
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        this.transform.position = currentPos;

        if (!blockColliderBottom.isBottom)
        {
            Transform bottomBlock = blockColliderBottom.otherBlock;
            Transform currentBlock = transform;
            Vector3 currentBlockPos = transform.position;
            while (bottomBlock != null)
            {
                Vector3 bottomBlockPos = currentBlockPos;
                bottomBlockPos.y = currentBlockPos.y -
                                   currentBlock.gameObject.GetComponent<RectTransform>().rect.height / 2 -
                                   bottomBlock.GetComponent<RectTransform>().rect.height / 2;
                bottomBlock.position = bottomBlockPos;

                // 다음 아래에 붙어있는 블록으로 이동
                BlockColliderBottom nextCollider = bottomBlock.GetComponentInChildren<BlockColliderBottom>();
                if (nextCollider != null && !nextCollider.isBottom)
                {
                    currentBlock = bottomBlock;
                    currentBlockPos = bottomBlock.position;
                    bottomBlock = nextCollider.otherBlock;
                }
                else
                {
                    bottomBlock = null;
                }
            }
        }
        
        if (isAttaching)
        {
            listIndex = blockColliderTop.ohterBlockListIndex;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        isClicked = false;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Window"))
            {
                onWindow = true;
            }
        }

        if (onWindow)
        {
            if (isDropped == false)
            {
                coppiedBlock = Instantiate(Resources.Load<GameObject>("Prefabs/" + gameObject.name),
                    codeWindow.transform, false);
                coppiedBlock.GetComponent<DragDrop>().isDropped = true;
                if (isAttaching)
                {
                    coppiedBlock.transform.position = attachPosition;
                    blockManager.AddBlock(blockColliderTop.ohterBlockListIndex, blockScript.blockData);
                }
                else
                {
                    coppiedBlock.transform.position = eventData.position;
                    BlockData blockData = coppiedBlock.GetComponent<Block>().blockData;
                    blockManager.MakeList(blockData);
                    coppiedBlock.GetComponent<DragDrop>().listIndex = blockManager.listOfLists.Count-1;
                }
                coppiedBlock.GetComponent<DragDrop>().blockIndex = blockManager.listOfLists[coppiedBlock.GetComponent<DragDrop>().listIndex].blockList
                    .Count - 1;
                transform.position = defaultPos;
            }
            else if (isAttaching)
            {
                blockIndex = blockManager.listOfLists[listIndex].blockList.Count - 1;
                transform.position = attachPosition;

                if (addCheck)
                {
                    if (listIndex != prevIndex)
                    {
                        blockManager.AddList(listIndex, prevIndex, blockIndex);
                    }
                }

                if (!blockColliderBottom.isBottom)
                {
                    Transform bottomBlock = blockColliderBottom.otherBlock;
                    Transform currentBlock = transform;
                    Vector3 currentBlockPos = transform.position;
                    while (bottomBlock != null)
                    {
                        Vector3 bottomBlockPos = currentBlockPos;
                        bottomBlockPos.y = currentBlockPos.y -
                                           currentBlock.gameObject.GetComponent<RectTransform>().rect.height / 2 -
                                           bottomBlock.GetComponent<RectTransform>().rect.height / 2;
                        bottomBlock.position = bottomBlockPos;

                        // 다음 아래에 붙어있는 블록으로 이동
                        BlockColliderBottom nextCollider = bottomBlock.GetComponentInChildren<BlockColliderBottom>();
                        if (nextCollider != null && !nextCollider.isBottom)
                        {
                            currentBlock = bottomBlock;
                            currentBlockPos = bottomBlock.position;
                            bottomBlock = nextCollider.otherBlock;
                        }
                        else
                        {
                            bottomBlock = null;
                        }
                    }
                }
            }
            else
            {
                if (divideCheck)
                {
                    blockManager.DivideList(listIndex, blockIndex);
                    Debug.Log(blockIndex);
                }
                transform.position = eventData.position;
            }
        }
        else
        {
            transform.position = defaultPos;
        }
    }
    
    void Start()
    {
        blockManager = FindObjectOfType<BlockManager>();
        blockColliderTop = FindObjectOfType<BlockColliderTop>();
        blockScript = FindObjectOfType<Block>();
        blockColliderBottom = FindObjectOfType<BlockColliderBottom>();
        codeWindow = GameObject.Find("Code Window");
    }
    
}
