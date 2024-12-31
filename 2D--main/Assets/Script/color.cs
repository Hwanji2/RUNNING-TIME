using UnityEngine;

public class CameraColorChange : MonoBehaviour
{
    public PlayerMove playerMove; // PlayerMove ��ũ��Ʈ�� ������ ����
    public Color originalColor = Color.white;
    public Color targetColor = Color.red;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // accel ���� ���� ���� ����
        UpdateBackgroundColorBasedOnSpeed();
    }

    void UpdateBackgroundColorBasedOnSpeed()
    {
        float accel = playerMove.accel; // PlayerMove ��ũ��Ʈ�� accel �� ����

        if (accel >= 100)
        {
            // accel ���� ����Ͽ� ���������� ��ȯ
            float t = Mathf.Clamp01((accel - 100) / 100f); // 100 �̻��� �� 0���� 1 ������ ������ ��ȯ
            mainCamera.backgroundColor = Color.Lerp(originalColor, targetColor, t);
        }
        else
        {
            // ���� ����� �ε巴�� ���ư���
            float t = Mathf.Clamp01(accel / 100f); // 0���� 100 ������ ������ ��ȯ
            mainCamera.backgroundColor = Color.Lerp(targetColor, originalColor, t);
        }
    }
}
