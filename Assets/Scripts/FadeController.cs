using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image panel;
    private float time = 0f;
    private float FadeTime = 1f;

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        panel.gameObject.SetActive(true);
        Color alpha = panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / FadeTime;
            alpha.a = Mathf.Lerp(0f, 1f, time);
            panel.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(1f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / FadeTime;
            alpha.a = Mathf.Lerp(1f, 0f, time);
            panel.color = alpha;
            yield return null;
        }
        panel.gameObject.SetActive(false);
        yield return null;
    }
    
}
