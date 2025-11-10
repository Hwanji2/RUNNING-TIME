using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Text;

public class CSVDialogueManager : MonoBehaviour
{
    [Header("🔹 CSV 파일 설정")]
    public string stage1CSV = "Assets/CSV/stage1.csv";
    public string stage2CSV = "Assets/CSV/stage2.csv";
    public string stage3CSV = "Assets/CSV/stage3.csv";

    [Header("🔹 현재 활성화된 스테이지")]
    public GameObject stage1;
    public GameObject stage2;
    public GameObject stage3;

    [Header("🔹 필수 오브젝트 설정")]
    public Transform playerTransform;
    public Rigidbody2D playerRigidbody;
    public TextMeshProUGUI dialogueTMP;

    private List<DialogueData> dialogueList = new List<DialogueData>();
    private HashSet<int> displayedIndexes = new HashSet<int>();
    private int currentDialogueIndex = 0;
    private bool isDisplaying = false;
    private string currentCSVPath;

    private void Start()
    {
        displayedIndexes.Clear();
        LoadCSV();
    }

    private void LoadCSV()
    {
        dialogueList.Clear();

        if (stage1.activeSelf) currentCSVPath = stage1CSV;
        else if (stage2.activeSelf) currentCSVPath = stage2CSV;
        else if (stage3.activeSelf) currentCSVPath = stage3CSV;
        else
        {
            Debug.LogError("🚨 활성화된 스테이지가 없습니다!");
            return;
        }

        Debug.Log($"📂 CSV 파일 로드 중: {currentCSVPath}");

        try
        {
            using (StreamReader reader = new StreamReader(currentCSVPath, Encoding.Default)) // ✅ UTF-8 BOM 제거
            {
                string line;
                bool isFirstLine = true;

                while ((line = reader.ReadLine()) != null)
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        continue;
                    }

                    string[] data = ParseCSVLine(line);

                    if (data.Length >= 8)
                    {
                        DialogueData dialogue = new DialogueData
                        {
                            Index = TryParseInt(data[0], 0),
                            Frame = TryParseInt(data[1], 0),
                            Time = TryParseNullableFloat(data[2]),
                            X_Position = TryParseNullableFloat(data[3]),
                            Y_Position = TryParseNullableFloat(data[4]),
                            Velocity_X = TryParseNullableFloat(data[5]),
                            Velocity_Y = TryParseNullableFloat(data[6]),
                            IsFalling = TryParseBool(data[7], false),
                            Dialogue = data.Length > 8 ? data[8].Trim().Replace("\"", "") : ""
                        };
                        dialogueList.Add(dialogue);
                    }
                }
            }

            Debug.Log($"✅ CSV 로드 완료! 총 {dialogueList.Count}개의 대사 데이터");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"🚨 CSV 읽기 오류: {e.Message}");
        }
    }
    private string[] ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool insideQuotes = false;
        string currentField = "";

        foreach (char c in line)
        {
            if (c == '"') insideQuotes = !insideQuotes;
            else if (c == ',' && !insideQuotes)
            {
                result.Add(currentField);
                currentField = "";
            }
            else currentField += c;
        }
        result.Add(currentField);
        return result.ToArray();
    }

    private int TryParseInt(string value, int defaultValue)
    {
        return int.TryParse(value, out int result) ? result : defaultValue;
    }

    private float? TryParseNullableFloat(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        return float.TryParse(value, out float result) ? result : (float?)null;
    }

    private bool TryParseBool(string value, bool defaultValue)
    {
        return bool.TryParse(value, out bool result) ? result : defaultValue;
    }

    private IEnumerator ShowDialogue(string text, float playerSpeed)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        isDisplaying = true;
        dialogueTMP.text = ""; // ✅ 먼저 텍스트를 숨김

        yield return StartCoroutine(FadeDialogue(text, playerSpeed));

        isDisplaying = false;
    }

    private IEnumerator FadeDialogue(string text, float speed)
    {
        float fadeDuration = Mathf.Clamp(3f - (speed / 10f), 0.5f, 3f);

        dialogueTMP.text = text; // ✅ 페이드 인 시작할 때 텍스트 적용

        float elapsedTime = 0;
        Color tmpColor = dialogueTMP.color;
        tmpColor.a = 0;
        dialogueTMP.color = tmpColor;

        while (elapsedTime < fadeDuration / 2)
        {
            tmpColor.a = Mathf.Lerp(0, 1, elapsedTime / (fadeDuration / 2));
            dialogueTMP.color = tmpColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(fadeDuration / 2);

        elapsedTime = 0;
        while (elapsedTime < fadeDuration / 2)
        {
            tmpColor.a = Mathf.Lerp(1, 0, elapsedTime / (fadeDuration / 2));
            dialogueTMP.color = tmpColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dialogueTMP.text = "";
    }

    private void Update()
    {
        if (!isDisplaying && currentDialogueIndex < dialogueList.Count)
        {
            CheckForDialogue();
        }
    }

    public void CheckForDialogue()
    {
        if (isDisplaying || currentDialogueIndex >= dialogueList.Count) return;

        DialogueData dialogue = dialogueList[currentDialogueIndex];

        if (displayedIndexes.Contains(dialogue.Index))
        {
            currentDialogueIndex++;
            return;
        }

        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;
        float playerVX = playerRigidbody.velocity.x;
        float playerVY = playerRigidbody.velocity.y;

        bool isXMatched = !dialogue.X_Position.HasValue || Mathf.Abs(playerX - dialogue.X_Position.Value) <= 1.0f;
        bool isYMatched = !dialogue.Y_Position.HasValue || Mathf.Abs(playerY - dialogue.Y_Position.Value) <= 1.0f;
        bool isVXMatched = !dialogue.Velocity_X.HasValue || Mathf.Abs(playerVX - dialogue.Velocity_X.Value) <= 1.0f;
        bool isVYMatched = !dialogue.Velocity_Y.HasValue || Mathf.Abs(playerVY - dialogue.Velocity_Y.Value) <= 1.0f;

        bool shouldDisplay = isXMatched && isYMatched && isVXMatched && isVYMatched;

        if (shouldDisplay)
        {
            StartCoroutine(ShowDialogue(dialogue.Dialogue, playerRigidbody.velocity.magnitude));
            displayedIndexes.Add(dialogue.Index);
            currentDialogueIndex++;
        }
    }
}

[System.Serializable]
public class DialogueData
{
    public int Index;
    public int Frame;
    public float? Time;
    public float? X_Position;
    public float? Y_Position;
    public float? Velocity_X;
    public float? Velocity_Y;
    public bool IsFalling;
    public string Dialogue;
}
