using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image targetSlider;
    public TMP_Text targetText;
    public string targetSceneName;
    public AsyncOperation op;

    void Start( )
    {
        StartCoroutine( LoadScene( targetSceneName ) );
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