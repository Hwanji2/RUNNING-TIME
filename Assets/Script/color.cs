using UnityEngine;

public class CameraColorChange : MonoBehaviour
{
    public PlayerMove playerMove; // PlayerMove 스크립트를 참조할 변수
    public Color originalColor = Color.white;
    public Color targetColor = Color.red;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // accel 값에 따라 색상 변경
        UpdateBackgroundColorBasedOnSpeed();
    }

    void UpdateBackgroundColorBasedOnSpeed()
    {
        float accel = playerMove.accel; // PlayerMove 스크립트의 accel 값 참조

        if (accel >= 100)
        {
            // accel 값에 비례하여 빨간색으로 변환
            float t = Mathf.Clamp01((accel - 100) / 100f); // 100 이상일 때 0에서 1 사이의 값으로 변환
            mainCamera.backgroundColor = Color.Lerp(originalColor, targetColor, t);
        }
        else
        {
            // 원래 색깔로 부드럽게 돌아가기
            float t = Mathf.Clamp01(accel / 100f); // 0에서 100 사이의 값으로 변환
            mainCamera.backgroundColor = Color.Lerp(targetColor, originalColor, t);
        }
    }
}
