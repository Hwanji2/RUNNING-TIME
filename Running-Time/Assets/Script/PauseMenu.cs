using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject escMenu;  // ESC 메뉴 UI
    public GameManager gameManager;  // 인스펙터에서 지정 가능하게 공개

    private bool isPaused = false;

    void Start()
    {
        // 인스펙터에서 GameManager가 지정되지 않은 경우 자동으로 찾기
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
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

    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        escMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void ResumeGame()
    {
        isPaused = false;
        escMenu.SetActive(false);
        Time.timeScale = 1;
        gameManager.ResumeBGM();  // 이어하기 버튼 눌렀을 때 BGM 재개
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
