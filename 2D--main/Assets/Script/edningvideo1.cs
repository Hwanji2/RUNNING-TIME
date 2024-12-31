using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class SceneChangeres : MonoBehaviour
{
    public Image fadeImage; // UI Image for fade effect
    public AudioClip transitionClip; // 씬 전환 전에 재생할 오디오 클립
    public VideoPlayer videoPlayer; // 비디오 플레이어 컴포넌트
    public VideoClip videoClipA; // F가 아닌 경우 재생할 비디오
    public VideoClip videoClipF; // F인 경우 재생할 비디오

    private float remainingTime;
    private int moneyPoint;
    private int coffeeCount;
    private int milkCount;
    private int unknownItemCount;
    private int secretCount;
    private float bestTime;

    private AudioSource audioSource;
    private bool canPressButton = false; // 버튼을 누를 수 있는지 여부

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // 데이터 불러오기
        remainingTime = PlayerPrefs.GetFloat("RemainingTime", 0);
        moneyPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);
        secretCount = PlayerPrefs.GetInt("SecretCount", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0);

        // 시작할 때 화면을 투명하게 변환
        StartCoroutine(FadeIn());
        if (IsGradeF())
        videoPlayer.clip = videoClipF;
      else
            videoPlayer.clip = videoClipA;
   

   
        StartCoroutine(PlayTransitionClipAndLoadScene("score"));
    }


    // F 점수 여부를 계산
    bool IsGradeF()
    {
        // 조건: remainingTime이 특정 값보다 낮고, 특정 조건들이 성립할 때 F로 판단
        if (remainingTime <= 0 || (moneyPoint <= 0 && secretCount == 0))
        {
            return true;
        }
        return false;
    }

    IEnumerator FadeIn()
    {
        float duration = 1.0f; // Fade duration
        float currentTime = 0f;

        Color color = fadeImage.color;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float duration = 1.0f; // Fade duration
        float currentTime = 0f;

        Color color = fadeImage.color;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(color.r, color.g, color.b, 1f);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlayTransitionClipAndLoadScene(string sceneName)
    {

        // 오디오 클립 재생
        audioSource.clip = transitionClip;
        audioSource.Play();

        // 오디오 클립 재생이 끝날 때까지 대기
        yield return new WaitForSeconds(audioSource.clip.length);

        // 비디오가 끝날 때까지 대기
        while (videoPlayer.isPlaying&& !Input.anyKeyDown)
        {
            yield return null;
        }
        canPressButton = true;
        // 페이드 아웃 후 씬 전환
        StartCoroutine(FadeOutAndLoadScene("score"));


    }
    

    IEnumerator EnableSceneTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPressButton = true; // 이제 버튼을 누를 수 있습니다.
    }
}
