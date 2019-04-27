using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public float fadeDelay = 3f;

    private CanvasGroup canvasGroup;


    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0; // Set to be transparent until we need it.
        // FadeIn(); // Do not call FadeIn on start, instead call when needed
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeIn()
    {
        canvasGroup.alpha = 0;
        StartCoroutine(DoFadeIn());
    }

    IEnumerator DoFadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / 2;
            yield return null;
        }

        DelayFadeOut();
        yield return null;
    }

    public void FadeOut()
    {
        canvasGroup.alpha = 1;
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeOut()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }

        yield return null;
    }

    public void DelayFadeOut()
    {
        StartCoroutine(StartFadeOutDelay());
    }

    IEnumerator StartFadeOutDelay()
    {
        yield return new WaitForSeconds(fadeDelay);
        FadeOut();
        yield return null;
    }
}
