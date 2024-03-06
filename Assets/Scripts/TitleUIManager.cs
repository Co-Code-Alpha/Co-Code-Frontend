using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject signInWindow;
    
    private bool signInWindowOpened = false;
    
    void Start()
    {
        
    }

    public void ManageSignIn()
    {
        signInWindow.SetActive(signInWindowOpened = !signInWindowOpened);
    }
}
