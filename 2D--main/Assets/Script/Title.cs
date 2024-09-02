using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class SceneChanger : MonoBehaviour
{
    public Image fadeImage; // UI Image for fade effect
    public AudioClip transitionClip; // �� ��ȯ ���� ����� ����� Ŭ��

    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // ������ �� ȭ���� �����ϰ� ��ȯ
        StartCoroutine(FadeIn());
    }

    void Update()
    {
        // �ƹ� Ű�� ������ �� �� ��ȯ
        if (Input.anyKeyDown)
        {
            StartCoroutine(PlayTransitionClipAndLoadScene("Main")); // ��ȯ�� ���� �̸��� �Է��ϼ���
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

        yield return new WaitForSeconds(0.5f); // ��� ���

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlayTransitionClipAndLoadScene(string sceneName)
    {
        // ����� Ŭ�� ���
        audioSource.clip = transitionClip;
        audioSource.Play();

        // ����� Ŭ�� ����� ���� ������ ���
        yield return new WaitForSeconds(audioSource.clip.length);

        // ���̵� �ƿ� �� �� ��ȯ
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }
}
