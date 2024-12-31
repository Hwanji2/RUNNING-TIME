using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTransition : MonoBehaviour
{
    public AudioSource bgmSource; // 현재 스테이지 BGM 오디오 소스
    public GameObject currentStage; // 현재 스테이지
    public GameObject tutorialStage; // 튜토리얼 스테이지
    public AudioSource tutorialBgmSource; // 튜토리얼 스테이지 BGM 오디오 소스
    public bool time = true;

    private GameManager gameManager;
    private float pausedTime;
    private float pausedBGMTime;

    void Start()
    {
        // GameManager 인스턴스 찾기
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = false;
            Debug.Log("Player entered trigger zone");

            // 현재 스테이지 BGM과 타이머 멈추기
            pausedBGMTime = bgmSource.time;
            bgmSource.Pause();
            Debug.Log("BGM paused at: " + pausedBGMTime);
            time = false;

            // 현재 스테이지 비활성화
            currentStage.SetActive(false);
            Debug.Log("Current stage deactivated");

            // 튜토리얼 스테이지 활성화
            tutorialStage.SetActive(true);
            Debug.Log("Tutorial stage activated");

            // 튜토리얼 BGM 재생
            tutorialBgmSource.Play();
            Debug.Log("Tutorial BGM started");
        }
    }
}
