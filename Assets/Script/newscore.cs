using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // TextMeshPro 추가
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class ScoreManager : MonoBehaviour
{
    public Text remainingTimeText;
    public Text moneyPointText;
    public Text secretCountText;
    public Text recordText;
    public TMP_Text recordTextTMP; // TextMeshPro UI 지원 추가
    public Text pressButtonText;
    public InputField nameInputField;
    public TMP_InputField nameInputFieldTMP; // TMP 입력 필드 추가
    public Button submitButton;
    public AudioClip transitionClip;
    public Image fadeImage;

    private float remainingTime;
    private int moneyPoint;
    private int secretCount;
    private AudioSource audioSource;
    private bool canPressButton = false;

    private List<Record> records = new List<Record>();
    private const int maxRecords = 12;

    private string[] pressButtonMessages = {
        "재수강하려면 아무 키나 눌러주세요",
        "아무 키나 누르면 재수강",
        "아무 키나 눌러서 재수강하십시오",
        "빌넣=아무 키",
        "E키를 누르면 상점이 열립니다."
    };

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 데이터 불러오기
        remainingTime = PlayerPrefs.GetFloat("RemainingTime", 0);
        moneyPoint = PlayerPrefs.GetInt("StagePoint", 0);
        secretCount = PlayerPrefs.GetInt("SecretCount", 0);

        // 기록 불러오기
        LoadRecords();

        // UI 업데이트
        remainingTimeText.text = "시간 " + FormatTime(remainingTime);
        moneyPointText.text = "돈 " + moneyPoint.ToString();
        secretCountText.text = "시크릿 " + secretCount.ToString();
        UpdateRecordText();


        // 제출 버튼 클릭 이벤트 추가
        submitButton.onClick.AddListener(OnSubmitName);
    }

    void Update()
    {
        if (canPressButton && Input.anyKeyDown)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            // 씬 전환 전에 오디오 클립 재생
            StartCoroutine(PlayTransitionClipAndLoadScene("Title"));
        }
    }

    void OnSubmitName()
    {
        string playerName = nameInputField != null ? nameInputField.text : ""; // 일반 UI Text
        if (string.IsNullOrEmpty(playerName) && nameInputFieldTMP != null)
        {
            playerName = nameInputFieldTMP.text; // TextMeshPro 사용 시
        }

        if (!string.IsNullOrEmpty(playerName))
        {
            // 신기록 업데이트
            UpdateRecords(playerName, moneyPoint, remainingTime, secretCount);

            // 이름 입력 UI 숨기기
            if (nameInputField != null) nameInputField.gameObject.SetActive(false);
            if (nameInputFieldTMP != null) nameInputFieldTMP.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);

            // 기록 UI 업데이트
            UpdateRecordText();

            // 5초 후에 "버튼을 눌러도 됩니다" 텍스트 표시
            canPressButton = true;
            StartCoroutine(ShowPressButtonText());
        }
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60000);
        int seconds = Mathf.FloorToInt((time % 60000) / 1000);
        int milliseconds = Mathf.FloorToInt(time % 1000);
        return string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void LoadRecords()
    {
        records.Clear();
        for (int i = 0; i < maxRecords; i++)
        {
            string name = PlayerPrefs.GetString("RecordName" + i, "");
            int money = PlayerPrefs.GetInt("RecordMoney" + i, 0);
            float time = PlayerPrefs.GetFloat("RecordTime" + i, 0);
            int secret = PlayerPrefs.GetInt("RecordSecret" + i, 0);
            if (!string.IsNullOrEmpty(name))
            {
                records.Add(new Record(name, money, time, secret));
            }
        }
    }

    void SaveRecords()
    {
        for (int i = 0; i < records.Count; i++)
        {
            PlayerPrefs.SetString("RecordName" + i, records[i].name);
            PlayerPrefs.SetInt("RecordMoney" + i, records[i].money);
            PlayerPrefs.SetFloat("RecordTime" + i, records[i].time);
            PlayerPrefs.SetInt("RecordSecret" + i, records[i].secret);
        }
    }

    void UpdateRecords(string name, int money, float time, int secret)
    {
        records.Add(new Record(name, money, time, secret));

        // 정렬 기준: 시크릿 → 점수 → 시간
        records.Sort((a, b) =>
        {
            int secretComparison = b.secret.CompareTo(a.secret);
            if (secretComparison != 0) return secretComparison;

            int moneyComparison = b.money.CompareTo(a.money);
            if (moneyComparison != 0) return moneyComparison;

            return a.time.CompareTo(b.time);
        });

        if (records.Count > maxRecords)
        {
            records.RemoveAt(records.Count - 1);
        }

        SaveRecords();
    }

    void UpdateRecordText()
    {
        string text = "< 신기록 >\n\n";
        for (int i = 0; i < records.Count; i++)
        {
            text += $"{i + 1}위 {records[i].name} - 얻은 돈: {records[i].money}, 시간: {FormatTime(records[i].time)}, 시크릿: {records[i].secret}\n";
        }

        if (recordText != null) recordText.text = text;
        if (recordTextTMP != null) recordTextTMP.text = text;
    }

    IEnumerator FadeIn()
    {
        float duration = 1.0f;
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

    IEnumerator ShowPressButtonText()
    {
        yield return new WaitForSeconds(5f);
        pressButtonText.text = pressButtonMessages[Random.Range(0, pressButtonMessages.Length)];
        canPressButton = true;
    }

    IEnumerator PlayTransitionClipAndLoadScene(string sceneName)
    {
        if (transitionClip != null)
        {
            audioSource.PlayOneShot(transitionClip);
            yield return new WaitForSeconds(transitionClip.length);
        }
        SceneManager.LoadScene(sceneName);
    }
}

[System.Serializable]
public class Record
{
    public string name;
    public int money;
    public float time;
    public int secret;

    public Record(string name, int money, float time, int secret)
    {
        this.name = name;
        this.money = money;
        this.time = time;
        this.secret = secret;
    }
}
