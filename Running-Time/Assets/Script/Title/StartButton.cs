using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Collections;

public class VideoButtonEffect : MonoBehaviour
{
    [Header("🎥 비디오 플레이어 (Inspector에서 지정)")]
    public VideoPlayer videoPlayer;

    [Header("🔘 애니메이션 적용할 TMP 버튼 (Inspector에서 지정)")]
    public Button targetButton;

    [Header("⚙️ 깜빡임 및 움직임 설정")]
    public float blinkInterval = 0.5f; // 점멸 간격 (초)
    public float moveAmount = 5f; // 위아래 움직이는 거리
    public float moveSpeed = 1f; // 움직임 속도

    [Header("🔄 스페이스 키 애니메이션 설정")]
    public float squishAmount = 0.6f; // 버튼 찌그러짐 비율 (가로)
    public float stretchAmount = 1.3f; // 버튼 늘어남 비율 (세로)
    public float moveDownAmount = 20f; // 버튼이 내려가는 거리
    public float animationSpeed = 0.2f; // 버튼 변형 속도

    private RectTransform buttonRectTransform;
    private CanvasGroup buttonCanvasGroup;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private bool isBlinking = false;
    private bool isAnimating = false; // 스페이스 키 애니메이션 실행 중 여부

    void Start()
    {
        // Inspector에서 지정한 VideoPlayer와 Button이 없을 경우 경고 메시지 출력
        if (videoPlayer == null)
        {
            Debug.LogError("⚠️ VideoPlayer가 지정되지 않았습니다! Inspector에서 설정하세요.");
            return;
        }
        if (targetButton == null)
        {
            Debug.LogError("⚠️ TMP Button이 지정되지 않았습니다! Inspector에서 설정하세요.");
            return;
        }

        buttonRectTransform = targetButton.GetComponent<RectTransform>();
        buttonCanvasGroup = targetButton.GetComponent<CanvasGroup>();

        if (buttonCanvasGroup == null)
        {
            buttonCanvasGroup = targetButton.gameObject.AddComponent<CanvasGroup>();
        }

        originalPosition = buttonRectTransform.anchoredPosition;
        originalScale = buttonRectTransform.localScale;
        buttonCanvasGroup.alpha = 0; // 처음에는 버튼 숨김
    }

    void Update()
    {
        if (videoPlayer.isPlaying && !isAnimating) // 애니메이션 중이 아닐 때만
        {
            double currentTime = videoPlayer.time;

            // 비디오 특정 구간에서만 버튼 점멸 & 움직임 실행
            if ((currentTime >= 4 && currentTime <= 15) || (currentTime >= 69)) // 1:09(69초)부터 다시 표시
            {
                buttonCanvasGroup.alpha = 1; 
                if (!isBlinking)
                {
                    StartCoroutine(BlinkAndMoveButton());
                }
            }
            else
            {
                StopBlinking();
                buttonCanvasGroup.alpha = 0; // 처음에는 버튼 숨김
            }
        }

        // 스페이스 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopBlinking(); // 점멸 & 움직임 멈춤
            StartCoroutine(AnimatePress()); // 버튼 애니메이션 실행
        }
    }

    IEnumerator BlinkAndMoveButton()
    {
        isBlinking = true;
        while (isBlinking)
        {
            float elapsedTime = 0f;

            while (elapsedTime < blinkInterval)
            {
                elapsedTime += Time.deltaTime;

                // 버튼 깜빡이기
                buttonCanvasGroup.alpha = Mathf.PingPong(Time.time * 2, 1); // 0~1 사이 반복

                // 버튼 위아래 움직이기
                float offset = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
                buttonRectTransform.anchoredPosition = originalPosition + new Vector2(0, offset);

                yield return null;
            }
        }
    }

    void StopBlinking()
    {
        isBlinking = false;
        buttonCanvasGroup.alpha = 1; // 버튼 완전히 보이게 설정
        buttonRectTransform.anchoredPosition = originalPosition; // 원래 위치로 복귀
    }

    IEnumerator AnimatePress()
    {
        isAnimating = true;

        // 버튼 원래 위치로 복귀
        buttonRectTransform.anchoredPosition = originalPosition;
        buttonCanvasGroup.alpha = 1;

        // 가로로 찌그러지며 아래로 이동
        yield return StartCoroutine(SquishAndMoveDown());

        // 스페이스 키를 떼면 복귀
        while (!Input.GetKeyUp(KeyCode.Space))
        {
            yield return null;
        }

        // 세로로 늘어나면서 원래 위치로 돌아옴
        yield return StartCoroutine(StretchAndReturn());

        isAnimating = false; // 애니메이션 완료
    }

    IEnumerator SquishAndMoveDown()
    {
        float elapsedTime = 0f;
        Vector3 squishedScale = new Vector3(squishAmount, stretchAmount, 1);
        Vector2 movedPosition = originalPosition + new Vector2(0, -moveDownAmount);

        while (elapsedTime < animationSpeed)
        {
            elapsedTime += Time.deltaTime;
            buttonRectTransform.localScale = Vector3.Lerp(originalScale, squishedScale, elapsedTime / animationSpeed);
            buttonRectTransform.anchoredPosition = Vector2.Lerp(originalPosition, movedPosition, elapsedTime / animationSpeed);
            yield return null;
        }

        buttonRectTransform.localScale = squishedScale;
        buttonRectTransform.anchoredPosition = movedPosition;
    }

    IEnumerator StretchAndReturn()
    {
        float elapsedTime = 0f;
        Vector3 stretchedScale = new Vector3(stretchAmount, squishAmount, 1);

        while (elapsedTime < animationSpeed)
        {
            elapsedTime += Time.deltaTime;
            buttonRectTransform.localScale = Vector3.Lerp(buttonRectTransform.localScale, stretchedScale, elapsedTime / animationSpeed);
            yield return null;
        }

        buttonRectTransform.localScale = stretchedScale;

        // 원래 위치와 크기로 복귀
        elapsedTime = 0f;
        while (elapsedTime < animationSpeed)
        {
            elapsedTime += Time.deltaTime;
            buttonRectTransform.localScale = Vector3.Lerp(stretchedScale, originalScale, elapsedTime / animationSpeed);
            buttonRectTransform.anchoredPosition = Vector2.Lerp(buttonRectTransform.anchoredPosition, originalPosition, elapsedTime / animationSpeed);
            yield return null;
        }

        buttonRectTransform.localScale = originalScale;
        buttonRectTransform.anchoredPosition = originalPosition;
    }
}
