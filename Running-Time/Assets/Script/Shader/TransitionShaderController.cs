using UnityEngine;

public class TransitionShaderController : MonoBehaviour
{
    public Material transitionMaterial; // 적용할 Shader Material
    public string lerpPropertyName = "_Lerp"; // Shader의 Lerp 변수 이름
    public float transitionDuration = 1.5f; // 트랜지션 지속 시간 (초)
    private float elapsedTime = 0f; // 경과 시간
    private float startValue = 0f; // 초기 Lerp 값
    private float targetValue = 1f; // 목표 Lerp 값
    private bool isTransitioning = false; // 트랜지션 실행 여부
    private bool isFadingOut = false; // 페이드 아웃 여부


    void Start()
    {

        // 게임 시작 시 자동으로 화면 열리는 효과 (페이드 아웃)
        StartFadeOut();
    }

    void Update()
    {
        if (isTransitioning)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            if (isFadingOut)
            {
                float easeOut = Mathf.SmoothStep(0, 1, t);
                float overshoot = 1.1f - 0.1f * Mathf.Cos(t * Mathf.PI * 2f);
                float elastic = Mathf.Lerp(startValue, targetValue, easeOut) * overshoot;
                transitionMaterial.SetFloat(lerpPropertyName, Mathf.Clamp01(elastic));
            }
            else
            {
                float easeIn = 1 - Mathf.Pow(1 - t, 3);
                float bounce = 0.08f * Mathf.Sin(t * Mathf.PI * 3f) * (1 - t);
                float elastic = Mathf.Lerp(startValue, targetValue, easeIn) - bounce;
                transitionMaterial.SetFloat(lerpPropertyName, Mathf.Clamp01(elastic));
            }

            if (elapsedTime >= transitionDuration)
            {
                isTransitioning = false;
                transitionMaterial.SetFloat(lerpPropertyName, targetValue);
                Debug.Log("✅ Transition Complete!");
            }
        }
    }

    public void StartFadeIn()
    {
        if (!isTransitioning)
        {
            startValue = 1f;
            targetValue = 0f;
            elapsedTime = 0f;
            isTransitioning = true;
            isFadingOut = false;
            Debug.Log("씨발");
        }
    }

    public void StartFadeOut()
    {
        if (!isTransitioning)
        {
            startValue = 0f;
            targetValue = 1f;
            elapsedTime = 0f;
            isTransitioning = true;
            isFadingOut = true;
        }
    }


}
