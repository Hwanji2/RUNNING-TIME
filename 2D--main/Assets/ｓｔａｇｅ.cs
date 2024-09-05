using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // 추가된 네임스페이스

public class TriggerAnimationAndSceneChange : MonoBehaviour
{
    public Animator animator; // 애니메이터 컴포넌트
    public string animationTriggerName; // 애니메이션 트리거 이름
    public string ENDING; // 전환할 기본 씬 이름
    public string score2; // 전환할 다른 씬 이름
    public AudioClip doorSound; // 문 소리 클립
    private AudioSource audioSource; // 오디오 소스

    private bool IsTriggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !IsTriggered)
        {
            IsTriggered = true;
            animator.SetTrigger(animationTriggerName);
            PlayDoorSound(); // 문 소리 재생
            StartCoroutine(WaitForAnimation()); // 수정된 부분
        }
    }

    private void PlayDoorSound()
    {
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }
    }

    IEnumerator WaitForAnimation()
    {
        // 애니메이션이 끝날 때까지 대기
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 데이터 저장
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            PlayerPrefs.SetInt("TotalPoint", gameManager.totalPoint);
            PlayerPrefs.SetInt("MoneyPoint", gameManager.moneyPoint);
            PlayerPrefs.SetInt("CoffeeCount", gameManager.coffeeCount);
            PlayerPrefs.SetInt("MilkCount", gameManager.milkCount);
            PlayerPrefs.SetInt("UnknownItemCount", gameManager.unknownItemCount);
            PlayerPrefs.SetFloat("RemainingTime", gameManager.timer);
            PlayerPrefs.SetInt("SecretCount", gameManager.player.secretCount);
        }

        // 현재 씬 이름에 따라 씬 전환
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Main")
        {
            SceneManager.LoadScene(ENDING);
        }
        else if (currentSceneName == "Main 1")
        {
            SceneManager.LoadScene(score2);
        }
    }
}
