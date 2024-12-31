using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class SceneChangeres : MonoBehaviour
{
    public Image fadeImage; // UI Image for fade effect
    public AudioClip transitionClip; // �� ��ȯ ���� ����� ����� Ŭ��
    public VideoPlayer videoPlayer; // ���� �÷��̾� ������Ʈ
    public VideoClip videoClipA; // F�� �ƴ� ��� ����� ����
    public VideoClip videoClipF; // F�� ��� ����� ����

    private float remainingTime;
    private int moneyPoint;
    private int coffeeCount;
    private int milkCount;
    private int unknownItemCount;
    private int secretCount;
    private float bestTime;

    private AudioSource audioSource;
    private bool canPressButton = false; // ��ư�� ���� �� �ִ��� ����

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // ������ �ҷ�����
        remainingTime = PlayerPrefs.GetFloat("RemainingTime", 0);
        moneyPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);
        secretCount = PlayerPrefs.GetInt("SecretCount", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0);

        // ������ �� ȭ���� �����ϰ� ��ȯ
        StartCoroutine(FadeIn());
        if (IsGradeF())
        videoPlayer.clip = videoClipF;
      else
            videoPlayer.clip = videoClipA;
   

   
        StartCoroutine(PlayTransitionClipAndLoadScene("score"));
    }


    // F ���� ���θ� ���
    bool IsGradeF()
    {
        // ����: remainingTime�� Ư�� ������ ����, Ư�� ���ǵ��� ������ �� F�� �Ǵ�
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

        // ����� Ŭ�� ���
        audioSource.clip = transitionClip;
        audioSource.Play();

        // ����� Ŭ�� ����� ���� ������ ���
        yield return new WaitForSeconds(audioSource.clip.length);

        // ������ ���� ������ ���
        while (videoPlayer.isPlaying&& !Input.anyKeyDown)
        {
            yield return null;
        }
        canPressButton = true;
        // ���̵� �ƿ� �� �� ��ȯ
        StartCoroutine(FadeOutAndLoadScene("score"));


    }
    

    IEnumerator EnableSceneTransitionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPressButton = true; // ���� ��ư�� ���� �� �ֽ��ϴ�.
    }
}
