using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTransition : MonoBehaviour
{
    public AudioSource bgmSource; // ���� �������� BGM ����� �ҽ�
    public GameObject currentStage; // ���� ��������
    public GameObject tutorialStage; // Ʃ�丮�� ��������
    public AudioSource tutorialBgmSource; // Ʃ�丮�� �������� BGM ����� �ҽ�
    public bool time = true;

    private GameManager gameManager;
    private float pausedTime;
    private float pausedBGMTime;

    void Start()
    {
        // GameManager �ν��Ͻ� ã��
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = false;
            Debug.Log("Player entered trigger zone");

            // ���� �������� BGM�� Ÿ�̸� ���߱�
            pausedBGMTime = bgmSource.time;
            bgmSource.Pause();
            Debug.Log("BGM paused at: " + pausedBGMTime);
            time = false;

            // ���� �������� ��Ȱ��ȭ
            currentStage.SetActive(false);
            Debug.Log("Current stage deactivated");

            // Ʃ�丮�� �������� Ȱ��ȭ
            tutorialStage.SetActive(true);
            Debug.Log("Tutorial stage activated");

            // Ʃ�丮�� BGM ���
            tutorialBgmSource.Play();
            Debug.Log("Tutorial BGM started");
        }
    }
}
