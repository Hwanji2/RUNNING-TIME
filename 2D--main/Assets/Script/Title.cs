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
    public Image initialFadeImage; // �ʱ� ���̵� ��/�ƿ��� ���� �̹���
    public AudioClip transitionClip; // �� ��ȯ ���� ����� ����� Ŭ��
    public Button button1;
    public Button button2;
    public string sceneName1 = "Main"; // ù ��° ��ư�� ��ȯ�� �� �̸�
    public string sceneName2 = "Main1"; // �� ��° ��ư�� ��ȯ�� �� �̸�
    public float fadeDuration = 1.0f;
    public float buttonVisibleDuration = 5.0f; // ��ư�� ���̴� �ð�
    public VideoPlayer videoPlayer; // ���� �÷��̾�
    private AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
        buttonGroup.alpha = 0;
        buttonGroup.interactable = false;
        buttonGroup.blocksRaycasts = false;
        // ������ �� ȭ���� �����ϰ� ��ȯ
        StartCoroutine(InitialFadeSequence());
    }

    private void LoadScene(string sceneName)
    {
        StartCoroutine(PlayTransitionClipAndLoadScene(sceneName));
    }

    private void Update()
    {
        // �ƹ� Ű�� ������ �� ��ư ���̵� �� ����
        if (Input.anyKeyDown && !buttonGroup.blocksRaycasts)
        {
            StartCoroutine(FadeInButtons());
        }
    }

    private IEnumerator InitialFadeSequence()
    {
        // �ʱ� ���̵� ��
        yield return StartCoroutine(FadeOut(initialFadeImage));

        yield return StartCoroutine(FadeIn(initialFadeImage));
        yield return StartCoroutine(FadeIn(fadeImage));
        // ���� ���
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

        // ��ư Ŭ�� �̺�Ʈ�� �޼��� ����
        button1.onClick.AddListener(() => LoadScene(sceneName1));
        button2.onClick.AddListener(() => LoadScene(sceneName2));

        // ���� �ð� �� ��ư ���̵� �ƿ� ����
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

        yield return new WaitForSeconds(0.5f); // ��� ���

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayTransitionClipAndLoadScene(string sceneName)
    {
        // ����� Ŭ�� ���
        audioSource.clip = transitionClip;
        audioSource.Play();

        // ����� Ŭ�� ����� ���� ������ ���
        yield return new WaitForSeconds(audioSource.clip.length);

        // ���̵� �ƿ� �� �� ��ȯ
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
