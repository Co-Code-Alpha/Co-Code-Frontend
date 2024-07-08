using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using File = System.IO.File;

public class SceneLoader : MonoBehaviour
{
    public Image targetSlider;
    public TMP_Text targetText;
    public AsyncOperation op;
    public string tipFilePath;

    public GameObject loadCompleteUI;

    void Start( )
    {
        SetRandomTip();
        SceneHandler handler = FindObjectOfType<SceneHandler>();
        StartCoroutine( LoadScene( handler.GetTargetScene() ) );
    }

    private void SetRandomTip()
    {
        //string[] lines = File.ReadAllLines(tipFilePath);
        TextAsset text = Resources.Load<TextAsset>("tips");
        string[] lines = text.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 배열의 내용을 출력합니다.
        foreach (string line in lines)
        {
            Debug.Log(line);
        }

        int randIdx = Random.Range(0, lines.Length);
        targetText.text = lines[randIdx];
    }

    private IEnumerator LoadScene( string targetScene )
    {
        yield return null;
        op = SceneManager.LoadSceneAsync(targetScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            //float randSec = Random.Range(0.05f, 0.1f);
            //yield return new WaitForSeconds(randSec);
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                targetSlider.fillAmount = Mathf.Lerp(targetSlider.fillAmount, op.progress, timer);
                if (targetSlider.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                targetSlider.fillAmount = Mathf.Lerp(targetSlider.fillAmount, 1f, timer);
                if (targetSlider.fillAmount == 1.0f)
                {
                    yield return new WaitForSeconds( 1f );
                    // 로딩 완료 후
                    loadCompleteUI.SetActive(true);
                    yield break;
                }
            }
        }
    }

    public void MoveScene()
    {
        StartCoroutine(SceneActivate());
    }

    public IEnumerator SceneActivate()
    {
        yield return null;
        op.allowSceneActivation = true;
    }
}