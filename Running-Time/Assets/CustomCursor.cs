using UnityEngine;
using UnityEngine.UI;

public class CustomCursor : MonoBehaviour
{
    [Header("🖱️ 커서로 사용할 UI 이미지")]
    public Image cursorImage; // 인스펙터에서 원하는 이미지 연결

    [Header("⚙️ 커서 위치 보정 (이미지 중심 맞추기)")]
    public Vector2 offset = Vector2.zero;

    private bool cursorEnabled = true;

    void Start()
    {
        // 기본 마우스 커서 숨기기
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        if (cursorImage == null)
        {
            Debug.LogError("❌ 커서 이미지가 지정되지 않았습니다!");
        }
    }

    void Update()
    {
        // ESC 누르면 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorEnabled = !cursorEnabled;

            // 상태에 따라 커서 표시/숨김
            Cursor.visible = !cursorEnabled;
            cursorImage.enabled = cursorEnabled;
        }

        if (!cursorEnabled || cursorImage == null)
            return;

        // 마우스 위치를 UI 좌표로 변환
        Vector2 mousePos = Input.mousePosition;

        // 오프셋 적용
        cursorImage.rectTransform.position = mousePos + offset;
    }

    void OnDisable()
    {
        // 스크립트 비활성화 시 기본 커서 복원
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
