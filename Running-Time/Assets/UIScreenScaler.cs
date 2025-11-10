using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class UIScreenScaler : MonoBehaviour
{
    [Header("📺 기준 해상도 (QHD 2560x1440)")]
    public Vector2 referenceResolution = new Vector2(2560f, 1440f);

    [Header("⚙ 현재 해상도 자동 반영 여부")]
    public bool autoAdjustOnStart = true;

    private CanvasScaler scaler;

    void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        if (autoAdjustOnStart)
            AdjustScale();
    }

    public void AdjustScale()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float referenceRatio = referenceResolution.x / referenceResolution.y;

        // 비율 차이에 따른 스케일 계산
        if (screenRatio > referenceRatio)
        {
            // 더 가로로 넓은 화면 → 세로 기준
            scaler.matchWidthOrHeight = 1f;
        }
        else
        {
            // 더 세로로 긴 화면 → 가로 기준
            scaler.matchWidthOrHeight = 0f;
        }

        // 현재 해상도 비율을 기준으로 Canvas 크기 조정
        scaler.referenceResolution = referenceResolution;
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }
}
