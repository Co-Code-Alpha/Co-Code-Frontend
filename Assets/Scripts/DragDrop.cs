using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEditor.PackageManager.Requests;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public bool isDropped = false;
    public bool isClicked = false;
    public bool isAttaching = false;
    public bool onWindow = false;
    public bool onTrash = false;
    public bool divideCheck = false;
    
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
        onTrash = false;
        defaultPos = this.transform.position;
        isClicked = true;
        if (isAttaching)
        {
            divideCheck = true;
        }
        else
        {
            divideCheck = false;
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

            if (result.gameObject.CompareTag("Trash"))
            {
                onTrash = true;
            }
        }

        if (onTrash && isDropped)
        {
            blockManager.listOfLists[listIndex].blockList.RemoveRange(blockIndex, blockManager.listOfLists[listIndex].blockList.Count - blockIndex);
            
            Transform bottomBlock = blockColliderBottom.otherBlock;
            while (bottomBlock != null)
            {
                Transform nextBottomBlock = null;
                BlockColliderBottom nextCollider = bottomBlock.GetComponentInChildren<BlockColliderBottom>();
                if (nextCollider != null && !nextCollider.isBottom)
                {
                    nextBottomBlock = nextCollider.otherBlock;
                }
                
                Destroy(bottomBlock.gameObject);
                bottomBlock = nextBottomBlock;
                /*
                var seq = DOTween.Sequence();
                seq.Append(bottomBlock.DOScale(0.1f, 1f));
                seq.Play().OnComplete(() =>
                {
                    
                });*/
            }
    
            var seq2 = DOTween.Sequence();
            seq2.Append(gameObject.transform.DOScale(0.1f, 1f));
            seq2.Play().OnComplete(() =>
            {
                Destroy(gameObject);
            });

            return;
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
                    coppiedBlock.GetComponent<DragDrop>().listIndex = listIndex;
                    Debug.Log(coppiedBlock.GetComponent<DragDrop>().listIndex);
                    blockManager.AddBlock(coppiedBlock.GetComponent<DragDrop>().listIndex, blockScript.blockData);
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
                coppiedBlock.GetComponent<DragDrop>().prevIndex = coppiedBlock.GetComponent<DragDrop>().listIndex;
            }
            else if (isAttaching)
            {
                if (prevIndex != listIndex)
                {
                    blockManager.AddList(listIndex, prevIndex, blockIndex);
                }

                blockIndex = blockColliderTop.otherBlock.GetComponent<DragDrop>().blockIndex + 1;
                transform.position = attachPosition;



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
                        bottomBlock.GetComponent<DragDrop>().blockIndex =
                            currentBlock.GetComponent<DragDrop>().blockIndex + 1;

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
                    listIndex = blockManager.listOfLists.Count - 1;
                    blockIndex = 0;
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
                        bottomBlock.GetComponent<DragDrop>().blockIndex =
                            currentBlock.GetComponent<DragDrop>().blockIndex + 1;

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
        blockColliderBottom = FindObjectOfType<BlockColliderBottom>();
        codeWindow = GameObject.Find("Code Window");
    }
    
}
