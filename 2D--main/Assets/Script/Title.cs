using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class SceneChanger : MonoBehaviour
{
    public CanvasGroup buttonGroup;
    public Image fadeImage; // UI Image for fade effect
    public Image initialFadeImage; // 초기 페이드 인/아웃을 위한 이미지
    public AudioClip transitionClip; // 씬 전환 전에 재생할 오디오 클립
    public Button button1;
    public Button button2;
    public string sceneName1 = "Main"; // 첫 번째 버튼이 전환할 씬 이름
    public string sceneName2 = "Main1"; // 두 번째 버튼이 전환할 씬 이름
    public float fadeDuration = 1.0f;
    public float buttonVisibleDuration = 5.0f; // 버튼이 보이는 시간
    public VideoPlayer videoPlayer; // 비디오 플레이어
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        buttonGroup.alpha = 0;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;
        // 시작할 때 화면을 투명하게 변환
        StartCoroutine(InitialFadeSequence());
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(PlayTransitionClipAndLoadScene(sceneName));
    }

    private void Update()
    {
        // 아무 키나 눌렀을 때 버튼 페이드 인 시작
        if (Input.anyKeyDown && !buttonGroup.blocksRaycasts)
        {
            StartCoroutine(FadeInButtons());
        }
    }

    private IEnumerator InitialFadeSequence()
    {
        // 초기 페이드 인
        yield return StartCoroutine(FadeOut(initialFadeImage));

        yield return StartCoroutine(FadeIn(initialFadeImage));
        yield return StartCoroutine(FadeIn(fadeImage));
        // 비디오 재생
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    private IEnumerator FadeInButtons()
    {
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            buttonGroup.alpha = Mathf.Lerp(0, 1, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        buttonGroup.alpha = 1;
        buttonGroup.interactable = true;
        buttonGroup.blocksRaycasts = true;

        // 버튼 클릭 이벤트에 메서드 연결
        button1.onClick.AddListener(() => LoadScene(sceneName1));
        button2.onClick.AddListener(() => LoadScene(sceneName2));

        // 일정 시간 후 버튼 페이드 아웃 시작
        yield return new WaitForSeconds(buttonVisibleDuration);
        StartCoroutine(FadeOutButtons());
    }

    private IEnumerator FadeIn(Image image)
    {
        float duration = fadeDuration; // Fade duration
        float currentTime = 0f;

        Color color = image.color;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(color.r, color.g, color.b, 0f);
    }

    private IEnumerator FadeOut(Image image)
    {
        float duration = fadeDuration; // Fade duration
        float currentTime = 0f;

        Color color = image.color;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            image.color = new Color(color.r, color.g, color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        image.color = new Color(color.r, color.g, color.b, 1f);
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float duration = fadeDuration; // Fade duration
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

        yield return new WaitForSeconds(0.5f); // 잠시 대기

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayTransitionClipAndLoadScene(string sceneName)
    {
        // 오디오 클립 재생
        audioSource.clip = transitionClip;
        audioSource.Play();

        // 오디오 클립 재생이 끝날 때까지 대기
        yield return new WaitForSeconds(audioSource.clip.length);

        // 페이드 아웃 후 씬 전환
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    private IEnumerator FadeOutButtons()
    {
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            buttonGroup.alpha = Mathf.Lerp(1, 0, progress);
            progress += rate * Time.deltaTime;
            yield return null;
        }

        buttonGroup.alpha = 0;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;
    }
}
