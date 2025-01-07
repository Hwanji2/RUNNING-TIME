using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // ��ư ����� ���� �߰�

public class PauseMenu : MonoBehaviour
{
    public GameObject escMenu;  // ESC �޴� UI
    public Button resumeButton; // �̾��ϱ� ��ư
    public Button quitButton;   // ���� ��ư
    public GameManager gameManager;  // GameManager ����

    private bool isPaused = false;

    void Start()
    {
        // �ν����Ϳ��� GameManager�� �������� ���� ��� �ڵ����� ã��
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        escMenu.SetActive(false);  // ���� �� ESC �޴� ��Ȱ��ȭ

        // ��ư Ŭ�� �̺�Ʈ ���
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

    // �̾��ϱ� ��ư ����
    public void ResumeGame()
    {
        isPaused = false;
        escMenu.SetActive(false);
        Time.timeScale = 1;
        gameManager.ResumeBGM();
    }

    // ���� ��ư ����
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // ������ ��� ����
#else
        Application.Quit();  // ����� ���� ����
#endif
    }
}
