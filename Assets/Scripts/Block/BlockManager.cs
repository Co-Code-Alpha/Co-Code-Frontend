using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;


[Serializable]
public class BlockList
{
    public List<GameObject> blockList = new List<GameObject>();
}

public class BlockManager : MonoBehaviour
{
    public GameObject player;
    public TMP_InputField ifPopup;
    
    public ProblemManager problemManager;
    
    public float speed = 3f;
    public Animator anim;
    public Rigidbody playerRigidbody;

    public bool isLoop = false;
    public bool isIf = false;
    public int loopTime = 1;
    
    [SerializeField]
    public List<BlockList> listOfLists = new List<BlockList>();

    [SerializeField] 
    public List<GameObject> loopList = new List<GameObject>();
    
    private void Start()
    {
        anim = player.GetComponent<Animator>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        problemManager = FindObjectOfType<ProblemManager>();
    }

    public void MakeList(GameObject block)
    {
        BlockList newList = new BlockList();
        newList.blockList.Add(block);
        listOfLists.Add(newList);
    }
    public void AddBlock(int a, GameObject block)
    {
        listOfLists[a].blockList.Add(block);
    }

    public void AddList(int a, int b, int n)
    {
        List<GameObject> newList = listOfLists[b].blockList.GetRange(n,listOfLists[b].blockList.Count - n);
        listOfLists[a].blockList.AddRange(newList);
        listOfLists[b].blockList.RemoveRange(n, listOfLists[b].blockList.Count - n);    
    }
    
    public void DivideList(int a, int n)
    {
        List<GameObject> newList = listOfLists[a].blockList.GetRange(n,listOfLists[a].blockList.Count - n);
        BlockList newBlockDataList = new BlockList();
        newBlockDataList.blockList = newList;
        listOfLists.Add(newBlockDataList);
        listOfLists[a].blockList.RemoveRange(n, listOfLists[a].blockList.Count - n);
    }

    public void PlayBlock(GameObject block)
    {
        StartCoroutine(ExecuteBlock(block));
    }
    
    public IEnumerator PlayBlocks(List<GameObject> blocks)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (player.GetComponent<PlayerCollider>().currentCollision.CompareTag("grass"))
            {
                problemManager.Fail();
                break;
            }

            if (player.GetComponent<PlayerCollider>().currentCollision.CompareTag("Trigger"))
            {
                GameObject obj = Resources.Load<GameObject>("Models/lock");
                GameObject obj1 = Instantiate(obj);
                obj1.transform.position = new Vector3(2, 3, 1);
                if (ifPopup.text == "False")
                {
                    problemManager.Fail();
                    Destroy(obj1);
                    break;
                }
            }
            Debug.Log(blocks[i].GetComponent<Block>().blockData.num);
            if (isIf)
            {
                yield break;
            }
            if (isLoop && blocks[i].GetComponent<Block>().blockData.num != 5)
            {
                loopList.Add(blocks[i]);
            }
            else
            {
                yield return StartCoroutine(ExecuteBlock(blocks[i]));
            }
        }
        
        FindObjectOfType<ProblemManager>().CheckAnswer();
    }

    private IEnumerator ExecuteBlock(GameObject block)
    {
        if (block == null)
        {
            yield break;
        }
        
        switch (block.GetComponent<Block>().blockData.num)
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
            case 4 :
                Debug.Log(Int32.Parse(block.GetComponent<Block>().loopTime.text));

                isLoop = true;
                loopTime = Int32.Parse(block.GetComponent<Block>().loopTime.text);
                break;
            case 5 :
                for (int i = 0; i < loopTime; i++)
                {
                    for (int j = 0; j < loopList.Count; j++)
                    {
                        yield return StartCoroutine(ExecuteBlock(loopList[j])); 
                    }
                }
                isLoop = false;
                loopTime = 1;
                loopList.Clear();
                break;
            case 6 :
                if (!block.GetComponent<IfBlock>().condition)
                {
                    isIf = true;
                }
                break;
            case 7 :
                isIf = false;
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
}