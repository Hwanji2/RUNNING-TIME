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
    public Text recordText; // 신기록 UI 텍스트 추가
    public Text pressButtonText; // "버튼을 눌러도 됩니다" 텍스트 추가

    public Image gradeImage; // 점수에 따른 이미지를 표시할 UI 이미지

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
    public AudioClip transitionClip; // 씬 전환 전에 재생할 오디오 클립

    public Image fadeImage; // UI Image for fade effect

    private float remainingTime;
    private int moneyPoint;
    private int coffeeCount;
    private int milkCount;
    private int unknownItemCount;
    private int secretCount;
    private float bestTime;
    private AudioSource audioSource;
    private bool canPressButton = false; // 버튼을 누를 수 있는지 여부

    private string[] pressButtonMessages = { // 여러 개의 텍스트 배열
        "재수강하려면 아무 키나 눌러주세요",
        "아무 키나 누르면 재수강",
        "아무 키나 눌러서 재수강하십시오",
        "빌넣=아무 키",
        "E키를 누르면 상점이 열립니다."
    };

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // 데이터 불러오기
        remainingTime = PlayerPrefs.GetFloat("RemainingTime", 0);
        moneyPoint = PlayerPrefs.GetInt("StagePoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);
        secretCount = PlayerPrefs.GetInt("SecretCount", 0);
        bestTime = PlayerPrefs.GetFloat("BestTime", 0); // 신기록 불러오기, 초기값은 최대값

        // 신기록 업데이트
        if (remainingTime > bestTime)
        {
            bestTime = remainingTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }

        // UI 업데이트
        remainingTimeText.text = FormatTime(remainingTime);
        moneyPointText.text = moneyPoint.ToString();
        secretCountText.text = secretCount.ToString();
        recordText.text = "BEST  " + FormatTime(bestTime); // 신기록 UI 업데이트

        // 점수 계산
        string grade = CalculateGrade(remainingTime, secretCount);
        gradeText.text = grade;

        // 점수에 따른 이미지 업데이트
        UpdateGradeImage(grade);

        // 점수에 따른 배경음악 재생
        PlayGradeMusic(grade);

        // 시작할 때 화면을 투명하게 변환
        StartCoroutine(FadeIn());

        // 5초 후에 "버튼을 눌러도 됩니다" 텍스트 표시
        StartCoroutine(ShowPressButtonText());
    }

    void Update()
    {
        if (canPressButton && Input.anyKeyDown)
        {
            // 재생 중인 배경음악 정지
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (gradeText.text == "A+")
            {
                // 씬 전환 전에 오디오 클립 재생
                StartCoroutine(PlayTransitionClipAndLoadScene("LAST"));
            }
            else
            {
                // 씬 전환 전에 오디오 클립 재생
                StartCoroutine(PlayTransitionClipAndLoadScene("Title"));
            }
            
        }
        else if(Input.anyKeyDown)
        {
            if (gradeText.text == "F")
            {
                pressButtonText.text = "체육관에 가면 튜토리얼을 할 수 있어요.";
            }
            else
            {
                pressButtonText.text = "수강신청 기간이 아닙니다. 수강신청 메뉴 진입은 시작 30분전에 가능합니다.";
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

    IEnumerator ShowPressButtonText()
    {
        yield return new WaitForSeconds(5f); // 5초 대기

        // "버튼을 눌러도 됩니다" 텍스트 페이드 인
        float duration = 1.0f; // 페이드 지속 시간
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
            pressButtonText.text = "A+을 받으면 크레딧이 있다고 합니다.";
        }
        else
        {
            // 랜덤으로 텍스트 선택
            int randomIndex = Random.Range(0, pressButtonMessages.Length);
            pressButtonText.text = pressButtonMessages[randomIndex];
        }
        // 버튼을 누를 수 있는 상태로 변경
        canPressButton = true;

    }
}
