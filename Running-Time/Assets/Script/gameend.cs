using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameOverManager : MonoBehaviour
{
    public Text remainingTimeText;
    public Text moneyPointText;
    public Text secretCountText;
    public Text gradeText;
    public Text recordText; // �ű�� UI �ؽ�Ʈ �߰�
    public Text pressButtonText; // "��ư�� ������ �˴ϴ�" �ؽ�Ʈ �߰�

    public Image gradeImage; // ������ ���� �̹����� ǥ���� UI �̹���

    public Sprite gradeAPlus;
    public Sprite gradeA;
    public Sprite gradeBPlus;
    public Sprite gradeB;
    public Sprite gradeCPlus;
    public Sprite gradeC;
    public Sprite gradeD;
    public Sprite gradeF;

    public AudioClip gradeAPlusClip;
    public AudioClip gradeAClip;
    public AudioClip gradeBPlusClip;
    public AudioClip gradeBClip;
    public AudioClip gradeCPlusClip;
    public AudioClip gradeCClip;
    public AudioClip gradeDClip;
    public AudioClip gradeFClip;
    public AudioClip transitionClip; // �� ��ȯ ���� ����� ����� Ŭ��

    public Image fadeImage; // UI Image for fade effect

    private float remainingTime;
    private int moneyPoint;
    private int coffeeCount;
    private int milkCount;
    private int unknownItemCount;
    private int secretCount;
    private float bestTime;
    private AudioSource audioSource;
    private bool canPressButton = false; // ��ư�� ���� �� �ִ��� ����

    private string[] pressButtonMessages = { // ���� ���� �ؽ�Ʈ �迭
        "������Ϸ��� �ƹ� Ű�� �����ּ���",
        "�ƹ� Ű�� ������ �����",
        "�ƹ� Ű�� ������ ������Ͻʽÿ�",
        "����=�ƹ� Ű",
        "EŰ�� ������ ������ �����ϴ�."
    };

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // ������ �ҷ�����
        remainingTime = PlayerPrefs.GetFloat("RemainingTime", 0);
        moneyPoint = PlayerPrefs.GetInt("StagePoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);
        secretCount = PlayerPrefs.GetInt("SecretCount", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0); // �ű�� �ҷ�����, �ʱⰪ�� �ִ밪

        // �ű�� ������Ʈ
        if (remainingTime > bestTime)
        {
            bestTime = remainingTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // UI ������Ʈ
        remainingTimeText.text = FormatTime(remainingTime);
        moneyPointText.text = moneyPoint.ToString();
        secretCountText.text = secretCount.ToString();
        recordText.text = "BEST  " + FormatTime(bestTime); // �ű�� UI ������Ʈ

        // ���� ���
        string grade = CalculateGrade(remainingTime, secretCount);
        gradeText.text = grade;

        // ������ ���� �̹��� ������Ʈ
        UpdateGradeImage(grade);

        // ������ ���� ������� ���
        PlayGradeMusic(grade);

        // ������ �� ȭ���� �����ϰ� ��ȯ
        StartCoroutine(FadeIn());

        // 5�� �Ŀ� "��ư�� ������ �˴ϴ�" �ؽ�Ʈ ǥ��
        StartCoroutine(ShowPressButtonText());
    }

    void Update()
    {
        if (canPressButton && Input.anyKeyDown)
        {
            // ��� ���� ������� ����
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (gradeText.text == "A+")
            {
                // �� ��ȯ ���� ����� Ŭ�� ���
                StartCoroutine(PlayTransitionClipAndLoadScene("LAST"));
            }
            else
            {
                // �� ��ȯ ���� ����� Ŭ�� ���
                StartCoroutine(PlayTransitionClipAndLoadScene("Title"));
            }
            
        }
        else if(Input.anyKeyDown)
        {
            if (gradeText.text == "F")
            {
                pressButtonText.text = "ü������ ���� Ʃ�丮���� �� �� �־��.";
            }
            else
            {
                pressButtonText.text = "������û �Ⱓ�� �ƴմϴ�. ������û �޴� ������ ���� 30������ �����մϴ�.";
            }
   

        }

    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60000);
        int seconds = Mathf.FloorToInt((time % 60000) / 1000);
        int milliseconds = Mathf.FloorToInt(time % 1000);
        return string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    string CalculateGrade(float time, int secretCount)
    {
        if ((time >= 100) && (secretCount >= 4))
        {
            return "A+";
        }
        else if ((time >= 100) && (secretCount >=3))
        {
            return "A";
        }
        else if (time >= 50000 && (secretCount >= 1))
        {
            return "A";
        }
        else if (time >= 40000 || (time >= 20000 && (secretCount >= 1)))
        {
            return "B+";
        }
        else if ((time >= 30000) || (time >= 1000 && (secretCount >= 1)))
        {
            return "B";
        }
        else if (time >= 10000)
        {
            return "C+";
        }
        else if (time >= 10)
        {
            return "C";
        }
        else if (time > 0)
        {
            return "D";
        }
        else
        {
            return "F";
        }
    }

    void UpdateGradeImage(string grade)
    {
        switch (grade)
        {
            case "A+":
                gradeImage.sprite = gradeAPlus;
                break;
            case "A":
                gradeImage.sprite = gradeA;
                break;
            case "B+":
                gradeImage.sprite = gradeBPlus;
                break;
            case "B":
                gradeImage.sprite = gradeB;
                break;
            case "C+":
                gradeImage.sprite = gradeCPlus;
                break;
            case "C":
                gradeImage.sprite = gradeC;
                break;
            case "D":
                gradeImage.sprite = gradeD;
                break;
            case "F":
                gradeImage.sprite = gradeF;
                break;
        }
    }

    void PlayGradeMusic(string grade)
    {
        switch (grade)
        {
            case "A+":
                audioSource.clip = gradeAPlusClip;
                break;
            case "A":
                audioSource.clip = gradeAClip;
                break;
            case "B+":
                audioSource.clip = gradeBPlusClip;
                break;
            case "B":
                audioSource.clip = gradeBClip;
                break;
            case "C+":
                audioSource.clip = gradeCPlusClip;
                break;
            case "C":
                audioSource.clip = gradeCClip;
                break;
            case "D":
                audioSource.clip = gradeDClip;
                break;
            case "F":
                audioSource.clip = gradeFClip;
                break;
        }

        if (audioSource.clip != null)
        {
            audioSource.Play();
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

    IEnumerator ShowPressButtonText()
    {
        yield return new WaitForSeconds(5f); // 5�� ���

        // "��ư�� ������ �˴ϴ�" �ؽ�Ʈ ���̵� ��
        float duration = 1.0f; // ���̵� ���� �ð�
        float currentTime = 0f;

        Color color = pressButtonText.color;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            pressButtonText.color = new Color(color.r, color.g, color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        pressButtonText.color = new Color(color.r, color.g, color.b, 1f);

        if (gradeText.text == "A+") 
        {
            pressButtonText.text = "Thanks for playing!";

        }
        else if (gradeText.text == "A")
        {
            pressButtonText.text = "A+�� ������ ũ������ �ִٰ� �մϴ�.";
        }
        else
        {
            // �������� �ؽ�Ʈ ����
            int randomIndex = Random.Range(0, pressButtonMessages.Length);
            pressButtonText.text = pressButtonMessages[randomIndex];
        }
        // ��ư�� ���� �� �ִ� ���·� ����
        canPressButton = true;

    }
}
