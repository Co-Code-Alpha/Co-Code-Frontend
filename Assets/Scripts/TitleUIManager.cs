using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject signInWindow;
    [Header("Sign Up")]
    public GameObject signUpWindow;

    public TMP_InputField idText;
    public TMP_InputField pwText;
    public TMP_InputField pwCheckText;
    public TMP_InputField emailText;
    public TMP_InputField nicknameText;
    public TMP_Text signUpErrorText;
    public Button signUpButton;
    
    private bool signInWindowOpened = false;
    private bool signUpWindowOpened = false;

    private ServerManager server;
    
    void Start()
    {
        server = FindObjectOfType<ServerManager>();
        signUpButton.onClick.AddListener(TrySignUp);
    }

    public void ManageSignIn()
    {
        signInWindow.SetActive(signInWindowOpened = !signInWindowOpened);
    }

    public void ManageSignUp()
    {
        signUpWindow.SetActive(signUpWindowOpened = !signUpWindowOpened);
    }

    public void TrySignUp()
    {
        if (pwText.text != pwCheckText.text)
        {
            signUpErrorText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }
        
        server.SignUpRequest(idText.text, pwText.text, nicknameText.text, emailText.text);
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene(2);
    }
}
