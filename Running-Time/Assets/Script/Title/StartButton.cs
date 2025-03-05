using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class VideoButtonEffect : MonoBehaviour
{
    [Header("🎥 비디오 플레이어 (Inspector에서 지정)")]
    public VideoPlayer videoPlayer;

    [Header("🔘 애니메이션 적용할 TMP 버튼 (Inspector에서 지정)")]
    public Button targetButton;

    [Header("⚙️ 깜빡임 및 움직임 설정")]
    public float blinkInterval = 0.5f;
    public float moveAmount = 5f;
    public float moveSpeed = 1f;

    [Header("🔄 스페이스 키 애니메이션 설정")]
    public float squishAmount = 0.6f;
    public float stretchAmount = 1.3f;
    public float moveDownAmount = 20f;
    public float animationSpeed = 0.2f;
    public float requiredHoldTime = 0.3f; // 최소 꾹 눌러야 하는 시간 (초)

    [Header("🔊 효과음 설정")]
    public AudioSource audioSource;
    public AudioClip pressSound;
    public AudioClip releaseSound;

    [Header("📌 이동할 씬 이름")]
    public string sceneName;

    private RectTransform buttonRectTransform;
    private CanvasGroup buttonCanvasGroup;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private bool isBlinking = false;
    private bool isAnimating = false;
    private bool isSpacePressed = false;
    private float spacePressStartTime = 0f; // 스페이스 키 누른 시간 저장

    void Start()
    {
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
        if (audioSource == null)
        {
            Debug.LogError("⚠️ AudioSource가 지정되지 않았습니다! Inspector에서 설정하세요.");
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
        buttonCanvasGroup.alpha = 0;
    }

    void Update()
    {
        if (videoPlayer.isPlaying && !isAnimating)
        {
            double currentTime = videoPlayer.time;

            if ((currentTime >= 4 && currentTime <= 15) || (currentTime >= 69))
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
                buttonCanvasGroup.alpha = 0;
            }
        }

        // 스페이스 키 눌렀을 때
        if (Input.GetKeyDown(KeyCode.Space) && !isSpacePressed)
        {
            isSpacePressed = true;
            spacePressStartTime = Time.time; // 누른 시점 기록
            audioSource.PlayOneShot(pressSound);
            StopBlinking();
            StartCoroutine(AnimatePress());
        }

        // 스페이스 키 뗄 때 (0.3초 이상 눌렀을 경우만 실행)
        if (Input.GetKeyUp(KeyCode.Space) && isSpacePressed)
        {
            float heldTime = Time.time - spacePressStartTime; // 눌렀던 총 시간 계산

            if (heldTime >= requiredHoldTime)
            {
                audioSource.PlayOneShot(releaseSound);
                StartCoroutine(DelayedSceneLoad()); // 1초 후 씬 전환
            }
            else
            {
                Debug.Log("❌ 스페이스 키를 너무 짧게 눌렀습니다. (0.3초 이상 필요)");
            }

            isSpacePressed = false;
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
                buttonCanvasGroup.alpha = Mathf.PingPong(Time.time * 2, 1);
                float offset = Mathf.Sin(Time.time * moveSpeed) * moveAmount;
                buttonRectTransform.anchoredPosition = originalPosition + new Vector2(0, offset);
                yield return null;
            }
        }
    }

    void StopBlinking()
    {
        isBlinking = false;
        buttonCanvasGroup.alpha = 1;
        buttonRectTransform.anchoredPosition = originalPosition;
    }

    IEnumerator AnimatePress()
    {
        isAnimating = true;
        buttonRectTransform.anchoredPosition = originalPosition;
        buttonCanvasGroup.alpha = 1;

        yield return StartCoroutine(SquishAndMoveDown());

        while (isSpacePressed) // 스페이스 키를 누르고 있는 동안 기다림
        {
            yield return null;
        }

        yield return StartCoroutine(StretchAndReturn());

        isAnimating = false;
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

    IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        LoadNextScene();
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("⚠️ 씬 이름이 설정되지 않았습니다! Inspector에서 sceneName을 지정하세요.");
        }
    }
}
