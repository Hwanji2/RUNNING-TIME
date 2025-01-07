using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject escMenu;  // ESC �޴� UI
    public GameManager gameManager;  // �ν����Ϳ��� ���� �����ϰ� ����

    private bool isPaused = false;

    void Start()
    {
        // �ν����Ϳ��� GameManager�� �������� ���� ��� �ڵ����� ã��
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
        gameManager.ResumeBGM();  // �̾��ϱ� ��ư ������ �� BGM �簳
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
