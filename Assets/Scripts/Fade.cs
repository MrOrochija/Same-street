using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class Fade
{
    public static IEnumerator FadeRoutine(Image fadeImage, float targetAlpha)
    {
        if (fadeImage == null) yield break;

        float speed = 1f / 0.5f;
        float currentAlpha = fadeImage.color.a;

        while (!Mathf.Approximately(currentAlpha, targetAlpha))
        {
            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, speed * Time.deltaTime);
            Color c = fadeImage.color;
            c.a = currentAlpha;
            fadeImage.color = c;
            yield return null;
        }
    }
}