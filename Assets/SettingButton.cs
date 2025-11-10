using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu1 : MonoBehaviour
{
    public GameObject settingsPanel;  // 설정 패널 UI
    public Button openSettingsButton; // 설정 열기 버튼
    public Button closeSettingsButton; // 설정 닫기 버튼

    // Start 메서드는 한 번만 정의해야 합니다.
    void Start()
    {
        // 시작 시 설정 패널 비활성화
        settingsPanel.SetActive(false);

        // 버튼 클릭 이벤트 등록
        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
    }

    // 설정 패널 열기
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0; // 게임 일시 정지
    }

    // 설정 패널 닫기
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1; // 게임 재개
    }
}
