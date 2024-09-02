using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SceneChanger : MonoBehaviour
{
    public Image fadeImage; // UI Image for fade effect
    public AudioClip transitionClip; // 씬 전환 전에 재생할 오디오 클립

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // 시작할 때 화면을 투명하게 변환
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        // 아무 키나 눌렀을 때 씬 전환
        if (Input.anyKeyDown)
        {
            StartCoroutine(PlayTransitionClipAndLoadScene("Main")); // 전환할 씬의 이름을 입력하세요
        }
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

        yield return new WaitForSeconds(0.5f); // 잠시 대기

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlayTransitionClipAndLoadScene(string sceneName)
    {
        // 오디오 클립 재생
        audioSource.clip = transitionClip;
        audioSource.Play();

        // 오디오 클립 재생이 끝날 때까지 대기
        yield return new WaitForSeconds(audioSource.clip.length);

        // 페이드 아웃 후 씬 전환
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }
}
