using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject codeWindow;
    public BlockManager blockManager;
    
    public void ResetButton()
    {
        Transform[] childBlock = new Transform[codeWindow.transform.childCount];
        for (int i = 0; i < codeWindow.transform.childCount; i++)
        {
            childBlock[i] = codeWindow.transform.GetChild(i);
        }
        
        foreach (Transform child in childBlock)
        {
            Destroy(child.gameObject);
        }
        
        blockManager.listOfLists.Clear();
    }

    public void PlayButton()
    {
        int listCnt = 0;
        int listNum = -1;
        for (int i = 0; i < blockManager.listOfLists.Count; i++)
        {
            if (blockManager.listOfLists[i].blockList.Count > 0)
            {
                listCnt++;
                listNum = i;
            }
        }

        if (listCnt != 1)
        {
            Debug.Log("실행하기 위해선 하나의 블록 리스트만 존재해야 합니다.");
            return;
        }
        
        Debug.Log(listNum);

        for (int i = 0; i < blockManager.listOfLists[listNum].blockList.Count; i++)
        {
            blockManager.PlayBlock(blockManager.listOfLists[listNum].blockList[i]);
        }
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        blockManager = FindObjectOfType<BlockManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
