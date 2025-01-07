using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // 버튼 사용을 위해 추가

public class PauseMenu : MonoBehaviour
{
    public GameObject escMenu;  // ESC 메뉴 UI
    public Button resumeButton; // 이어하기 버튼
    public Button quitButton;   // 종료 버튼
    public GameManager gameManager;  // GameManager 참조

    private bool isPaused = false;

    void Start()
    {
        // 인스펙터에서 GameManager가 지정되지 않은 경우 자동으로 찾기
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        escMenu.SetActive(false);  // 시작 시 ESC 메뉴 비활성화

        // 버튼 클릭 이벤트 등록
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
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
        escMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            gameManager.PauseBGM();
        }
        else
        {
            gameManager.ResumeBGM();
        }
    }

    // 이어하기 버튼 동작
    public void ResumeGame()
    {
        isPaused = false;
        escMenu.SetActive(false);
        Time.timeScale = 1;
        gameManager.ResumeBGM();
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
