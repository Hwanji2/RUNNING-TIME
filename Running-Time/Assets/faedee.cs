using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeIn : MonoBehaviour
{
    [Header("🌒 페이드 이미지 (UI Image)")]
    public Image fadeImage; // 반드시 Canvas 안의 전체 화면 Image

    [Header("⏳ 페이드 지속 시간 (초)")]
    public float fadeDuration = 1.5f;

    [Header("🎨 목표 색상 (기본: 검정, 투명→불투명)")]
    public Color targetColor = Color.black;

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("❌ 페이드 이미지가 연결되어 있지 않습니다!");
            return;
        }

        // 시작 시 투명하게 초기화
        Color c = targetColor;
        c.a = 0f;
        fadeImage.color = c;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        Color c = fadeImage.color;
        float startAlpha = 0f;
        float endAlpha = 1f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalized = Mathf.Clamp01(t / fadeDuration);
            c.a = Mathf.Lerp(startAlpha, endAlpha, normalized);
            fadeImage.color = c;
            yield return null;
        }

        // 보이는 상태 유지 (필요시 비활성화 가능)
        fadeImage.color = new Color(c.r, c.g, c.b, 1f);
    }
}
