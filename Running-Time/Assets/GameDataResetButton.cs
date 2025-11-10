using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameDataResetButton : MonoBehaviour
{
    [Header("🧩 연결할 버튼")]
    public Button resetButton;

    [Header("📁 삭제할 데이터 폴더 경로 (선택사항)")]
    [Tooltip("Application.persistentDataPath 기준으로 삭제할 폴더 이름. 비워두면 PlayerPrefs만 초기화합니다.")]
    public string folderName = "SaveData";

    [Header("⚠️ 확인용 경고창 (선택)")]
    public GameObject confirmPanel;

    private string fullPath;

    void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetButtonPressed);

        if (!string.IsNullOrEmpty(folderName))
            fullPath = Path.Combine(Application.persistentDataPath, folderName);
    }

    public void OnResetButtonPressed()
    {
        // 경고창이 있으면 먼저 표시
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(true);
        }
        else
        {
            PerformDataReset();
        }
    }

    // ⚠️ 실제 데이터 삭제 함수
    public void PerformDataReset()
    {
        // 1️⃣ PlayerPrefs 데이터 삭제
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // 2️⃣ 지정 폴더 내 파일 삭제
        if (!string.IsNullOrEmpty(folderName) && Directory.Exists(fullPath))
        {
            try
            {
                Directory.Delete(fullPath, true);
                Debug.Log($"✅ '{fullPath}' 폴더 내 데이터 삭제 완료");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ 데이터 삭제 중 오류 발생: {e.Message}");
            }
        }

        // 3️⃣ 확인 메시지
        Debug.Log("🧹 게임 관련 데이터가 모두 초기화되었습니다.");

        // 4️⃣ 기본 커서 복원 (혹시 커서 숨김 상태일 경우)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
