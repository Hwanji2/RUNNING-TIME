using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeInOut : MonoBehaviour
{
    public Text uiText;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 1.0f;
    public float delay = 3.0f;
    private bool hasFadedOut = false;

    private void Start()
    {
        // 처음에 페이드 인
        StartCoroutine(FadeInText());
    }

    private void Update()
    {
        // 스페이스 키를 눌렀을 때 페이드 아웃 시작
        if (Input.GetKeyDown(KeyCode.Space) && !hasFadedOut)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutText());
        }
    }

    private IEnumerator FadeInText()
    {
        Color originalColor = uiText.color;
        float rate = 1.0f / fadeInDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0, 1, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }

        uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

        // 3초 후에 페이드 아웃 시작
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        hasFadedOut = true;
        Color originalColor = uiText.color;
        float rate = 1.0f / fadeOutDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, progress));
            progress += rate * Time.deltaTime;
            yield return null;
        }

        uiText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
