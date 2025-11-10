using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // 버튼 사용을 위해 추가

public class PauseMenu : MonoBehaviour
{
    public GameObject escMenu;  // ESC 메뉴 UI
    public Button resumeButton; // 이어하기 버튼
    public Button quitButton;   // 종료 버튼
    public GameManager gameManager;  // GameManager 참조 (null일 수도 있음)

    private bool isPaused = false;

    void Start()
    {
        // escMenu가 null이 아닐 때만 비활성화
        if (escMenu != null)
        {
            escMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ escMenu가 설정되지 않았습니다. Unity에서 확인하세요.");
        }

        // 버튼이 null이 아닐 때만 이벤트 리스너 등록
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        else
        {
            Debug.LogWarning("⚠️ resumeButton이 설정되지 않았습니다.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogWarning("⚠️ quitButton이 설정되지 않았습니다.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;

        if (escMenu != null)
        {
            escMenu.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0 : 1;

        // gameManager가 null이 아닐 때만 BGM 제어
        if (gameManager != null)
        {
            if (isPaused)
            {
                gameManager.PauseBGM();
            }
            else
            {
                gameManager.ResumeBGM();
            }
        }
    }

    // 이어하기 버튼 동작
    public void ResumeGame()
    {
        isPaused = false;

        if (escMenu != null)
        {
            escMenu.SetActive(false);
        }

        Time.timeScale = 1;

        // gameManager가 null이 아닐 때만 BGM 제어
        if (gameManager != null)
        {
            gameManager.ResumeBGM();
        }
    }

    // 종료 버튼 동작
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 에디터 모드 종료
#else
        Application.Quit();  // 빌드된 게임 종료
#endif
    }
}
