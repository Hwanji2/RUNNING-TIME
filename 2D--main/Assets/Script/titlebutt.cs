using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button targetButton;
    public RectTransform targetImage;
    public float moveDistance = 50f;
    public float moveSpeed = 5f;
    public float fadeDuration = 0.5f;
    public float scaleMultiplier = 1.2f;
    public float shakeAmount = 2f; // 흔들림 정도를 줄였습니다.
    public float shakeSpeed = 2f;

    private Vector2 originalPosition;
    private Vector3 originalScale;
    private CanvasGroup canvasGroup;
    private bool isHovering = false;

    void Start()
    {
        if (targetImage != null)
        {
            originalPosition = targetImage.anchoredPosition;
            originalScale = targetImage.localScale;
            canvasGroup = targetImage.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = targetImage.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0; // 초기에는 이미지가 보이지 않도록 설정
        }

        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClick);
        }
    }

    void Update()
    {
        if (targetImage != null)
        {
            // 이미지 위치 이동 및 페이드 효과
            Vector2 targetPosition = isHovering ? originalPosition + new Vector2(0, moveDistance) : originalPosition;
            targetImage.anchoredPosition = Vector2.Lerp(targetImage.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);

            float targetAlpha = isHovering ? 1 : 0;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, Time.deltaTime / fadeDuration);

            // 이미지 크기 조절
            Vector3 targetScale = isHovering ? originalScale * scaleMultiplier : originalScale;
            targetImage.localScale = Vector3.Lerp(targetImage.localScale, targetScale, Time.deltaTime * moveSpeed);

            // 버튼 크기 조절
            if (targetButton != null)
            {
                Vector3 buttonScale = isHovering ? originalScale * scaleMultiplier : originalScale;
                targetButton.transform.localScale = Vector3.Lerp(targetButton.transform.localScale, buttonScale, Time.deltaTime * moveSpeed);

              
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
    }

    private void OnButtonClick()
    {
        // 버튼 클릭 시 실행할 코드
    }
}
