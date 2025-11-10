using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedTextAnimator : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // 플레이어의 Transform
    [SerializeField] private float maxDisplayDistance = 10f; // 최대 표시 거리
    [SerializeField] private TextMesh textMesh; // ✅ 3D TextMesh로 변경
    [SerializeField] private List<string> textList = new List<string>(); // 표시할 텍스트 목록
    [SerializeField] private List<float> displayTimes = new List<float>(); // 각 텍스트당 표시 시간
    [SerializeField] private float typingSpeed = 0.05f; // 한 글자씩 타이핑 속도
    [SerializeField] private float fadeDuration = 0.5f; // 페이드 효과 지속 시간

    private int currentIndex = 0;
    private bool isWithinRange = false;
    private Coroutine textCoroutine;

    private void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= maxDisplayDistance)
        {
            if (!isWithinRange)
            {
                isWithinRange = true;
                currentIndex = 0; // 첫 번째 텍스트부터 다시 시작
                RestartTextAnimation();
            }
        }
        else
        {
            if (isWithinRange)
            {
                isWithinRange = false;
                StopTextAnimation();
            }
        }
    }

    private void RestartTextAnimation()
    {
        // ✅ 중복 방지: 기존 코루틴이 실행 중이면 정지
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
        }

        textMesh.text = ""; // ✅ 기존 텍스트 초기화
        textCoroutine = StartCoroutine(CycleText());
    }

    private void StopTextAnimation()
    {
        if (textCoroutine != null)
        {
            StopCoroutine(textCoroutine);
            textCoroutine = null;
        }

        textMesh.text = ""; // ✅ 거리가 멀어지면 텍스트 삭제
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 0); // 투명화
    }

    private IEnumerator CycleText()
    {
        while (isWithinRange)
        {
            string currentText = textList[currentIndex];
            float displayTime = (currentIndex < displayTimes.Count) ? displayTimes[currentIndex] : 2f; // 기본값 2초

            yield return StartCoroutine(TypeText(currentText)); // 한 글자씩 타이핑 효과
            yield return new WaitForSeconds(displayTime); // 유지 시간 대기
            yield return StartCoroutine(FadeText("", false)); // 페이드 아웃 후 텍스트 삭제

            currentIndex = (currentIndex + 1) % textList.Count; // 다음 텍스트로 이동
        }
    }

    private IEnumerator TypeText(string text)
    {
        textMesh.text = "";
        Color textColor = textMesh.color;
        textColor.a = 1f; // 타이핑 시에는 투명도 유지
        textMesh.color = textColor;

        foreach (char letter in text)
        {
            textMesh.text += letter;
            yield return new WaitForSeconds(typingSpeed); // 한 글자씩 타이핑
        }
    }

    private IEnumerator FadeText(string newText, bool fadeIn)
    {
        float elapsedTime = 0f;
        Color textColor = textMesh.color;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        if (fadeIn)
            textMesh.text = newText;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            textMesh.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        textMesh.color = new Color(textColor.r, textColor.g, textColor.b, endAlpha);

        if (!fadeIn)
            textMesh.text = ""; // 페이드 아웃 후 텍스트 제거
    }
}
