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
    public string targetSceneName;
    public AsyncOperation op;
    public string tipFilePath;

    void Start( )
    {
        SetRandomTip();
        StartCoroutine( LoadScene( targetSceneName ) );
    }

    private void SetRandomTip()
    {
        string[] lines = File.ReadAllLines(tipFilePath);

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
            float randSec = Random.Range(0.05f, 0.4f);
            yield return new WaitForSeconds(randSec);
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
                    yield break;
                }
            }
        }
    }

    public IEnumerator MoveScene()
    {
        yield return new WaitForSeconds( 1f );
        
        // 애니메이션
        
        yield return new WaitForSeconds( 1f );
        
        op.allowSceneActivation = true;
    }
}