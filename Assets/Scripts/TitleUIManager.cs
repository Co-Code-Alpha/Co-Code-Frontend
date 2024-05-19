using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("Sign In")]
    public GameObject signInWindow;
    public TMP_InputField signInIdText;
    public TMP_InputField signInPwText;
    public TMP_Text signInErrorText;
    public Toggle saveToggle;
    [Header("Sign Up")]
    public GameObject signUpWindow;
    public TMP_InputField idText;
    public TMP_InputField pwText;
    public TMP_InputField pwCheckText;
    public TMP_InputField emailText;
    public TMP_InputField nicknameText;
    public TMP_Text signUpErrorText;
    public Button signUpButton;
    public GameObject alertWindow;
    public TMP_Text alertText;
    [Header("ID / PW Find")]
    public GameObject findBackground;
    public GameObject idFindWindow;
    public GameObject pwFindWindow;
    public Button swapButton;
    public TMP_InputField idFindEmailText;
    public TMP_Text idFindErrorText;
    public TMP_InputField pwFindEmailText;
    public TMP_Text pwFindErrorText;
    public GameObject pwChangeWindow;
    public TMP_InputField pwChangeText;
    public TMP_InputField pwChangeCheckText;
    public TMP_Text pwChangeErrorText;
    [Header("Email Authentication")]
    public GameObject authWindow;
    public TMP_Text emailAuthText;
    public TMP_InputField emailCodeText;
    public TMP_Text authCountText;
    public TMP_Text authErrorText;
    public Button authButton;
    
    private bool signInWindowOpened = false;
    private bool signUpWindowOpened = false;

    private bool isIdFind = true;
    private string pwChangeEmail;

    private float emailAuthCount = 300f;
    private bool isAuthCount = false;

    private ServerManager server;
    
    void Start()
    {
        server = FindObjectOfType<ServerManager>();
        signUpButton.onClick.AddListener(TrySignUp);
    }

    void Update()
    {
        if (isAuthCount)
        {
            emailAuthCount -= Time.deltaTime;
            authCountText.text = Mathf.Floor(emailAuthCount / 60).ToString() + ":" +
                                 Mathf.RoundToInt(emailAuthCount % 60).ToString();
            if (emailAuthCount <= 0)
            {
                isAuthCount = false;
                EmailAuthExpire();
            }
        }
    }

    public void ManageSignIn()
    {
        signInWindow.SetActive(signInWindowOpened = !signInWindowOpened);
    }

    public void ManageSignUp()
    {
        signUpWindow.SetActive(signUpWindowOpened = !signUpWindowOpened);
    }

    public void ManageAccountFind()
    {
        isIdFind = !isIdFind;
        idFindWindow.SetActive(isIdFind);
        pwFindWindow.SetActive(!isIdFind);

        swapButton.transform.GetChild(0).GetComponent<TMP_Text>().text = isIdFind ? "비밀번호 변경" : "아이디 찾기";
    }

    public void TryLogin()
    {
        if (signInIdText.text == "" || signInPwText.text == "")
        {
            signInErrorText.text = "모든 정보를 입력해 주세요.";
            return;
        }
        
        server.SignInRequest(signInIdText.text, signInPwText.text);
    }

    public void SetLoginError(string error)
    {
        signInErrorText.text = error;
    }

    public void TrySignUp()
    {
        if (idText.text == "" || pwText.text == "" || pwCheckText.text == "" || nicknameText.text == "" || emailText.text == "")
        {
            signUpErrorText.text = "모든 정보를 입력해 주세요.";
            return;
        }
        
        if (pwText.text != pwCheckText.text)
        {
            signUpErrorText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }
        
        server.SignUpCheckRequest(idText.text, pwText.text, nicknameText.text, emailText.text);
    }

    public void SetSignUpError(string error)
    {
        signUpErrorText.text = error;
    }

    public void DeleteSignUpInfo()
    {
        idText.text = "";
        pwText.text = "";
        pwCheckText.text = "";
        emailText.text = "";
        nicknameText.text = "";
    }

    public void SignUpEmailAuth(string email)
    {
        DeleteSignUpInfo();
        signUpWindow.SetActive(false);
        authWindow.SetActive(true);

        emailAuthText.text = email + "\n주소로 인증 메일을 전송하였습니다.";
        emailAuthCount = 300f;
        isAuthCount = true;
        
        authButton.onClick.AddListener(() =>
        {
            EmailAuthSend(0);
        });
    }

    public void SignUpSuccess(string nickname)
    {
        authWindow.SetActive(false);
        Alert(string.Format("회원가입이 완료되었습니다.\n환영합니다. {0}님!", nickname));
    }

    public void EmailAuthSend(int code)
    {
        if (emailCodeText.text != PlayerPrefs.GetString("authCode"))
        {
            authErrorText.text = "인증번호가 일치하지 않습니다.";
        }
        else
        {
            if (code == 0)
            {
                server.SignUpRequest(PlayerPrefs.GetString("joinId"), PlayerPrefs.GetString("joinPw"), PlayerPrefs.GetString("joinNick"), PlayerPrefs.GetString("joinEmail"));
            } else if (code == 1)
            {
                authWindow.SetActive(false);
                pwChangeWindow.SetActive(true);
            }
        }
    }

    public void EmailAuthExpire()
    {
        emailAuthText.text = "";
        PlayerPrefs.DeleteKey("authCode");
        authWindow.SetActive(false);
    }

    public void TryIdFind()
    {
        if (idFindEmailText.text == "")
        {
            idFindErrorText.text = "올바른 정보를 입력해 주세요.";
            return;
        }
        
        server.IdFindRequest(idFindEmailText.text);
    }

    public void Alert(string text)
    {
        alertWindow.SetActive(true);
        alertText.text = text;
    }

    public void IdFindError(string error)
    {
        idFindErrorText.text = error;
    }

    public void PwFindEmailAuth()
    {
        if (pwFindEmailText.text == "")
        {
            pwFindErrorText.text = "올바른 정보를 입력해 주세요.";
            return;
        }

        server.EmailAuthRequest(pwFindEmailText.text, 1);
    }

    public void PwFindEmailAuthSuccess(string email)
    {
        pwChangeEmail = email;
        pwFindWindow.SetActive(false);
        authWindow.SetActive(true);

        emailAuthText.text = email + "\n주소로 인증 메일을 전송하였습니다.";
        emailAuthCount = 300f;
        isAuthCount = true;
        
        authButton.onClick.AddListener(() =>
        {
            EmailAuthSend(1);
        });
    }

    public void TryPwChange()
    {
        if (pwChangeText.text != pwChangeCheckText.text)
        {
            pwChangeErrorText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        server.PwChangeRequest(pwChangeEmail, pwChangeText.text);
    }
}
