using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject signInWindow;
    public GameObject signUpWindow;
    
    private bool signInWindowOpened = false;
    private bool signUpWindowOpened = false;
    
    void Start()
    {
        
    }

    public void ManageSignIn()
    {
        signInWindow.SetActive(signInWindowOpened = !signInWindowOpened);
    }

    public void ManageSignUp()
    {
        signUpWindow.SetActive(signUpWindowOpened = !signUpWindowOpened);
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene(2);
    }
}
