using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeedLineEffect : MonoBehaviour
{
    [Header("🎮 플레이어 Transform")]
    public Transform player;

    [Header("📏 선 개수 및 길이/두께")]
    public int lineCount = 20;
    public float lineLength = 300f;
    public float lineThickness = 2f;

    [Header("🎨 선 색상 및 지속시간")]
    public Color lineColor = Color.white;
    public float showDuration = 0.3f;

    [Header("🖼️ 부모 Canvas (Screen Space - Overlay)")]
    public Canvas canvas;

    private List<Image> lines = new List<Image>();
    private bool isShowing = false;

    void Start()
    {
        if (canvas == null)
        {
            Debug.LogError("❌ Canvas를 지정하세요! (Screen Space - Overlay 모드 필요)");
            return;
        }

        // 초기 라인 생성
        for (int i = 0; i < lineCount; i++)
        {
            GameObject lineObj = new GameObject($"SpeedLine_{i}");
            lineObj.transform.SetParent(canvas.transform, false);
            Image img = lineObj.AddComponent<Image>();
            img.color = lineColor;

            RectTransform rt = img.rectTransform;
            rt.sizeDelta = new Vector2(lineLength, lineThickness);
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);

            img.enabled = false;
            lines.Add(img);
        }
    }

    void Update()
    {
        if (player == null) return;

        // 아무 키나 누를 때 속도선 표시
        if (Input.anyKeyDown && !isShowing)
            StartCoroutine(ShowLines());
    }

    System.Collections.IEnumerator ShowLines()
    {
        isShowing = true;

        Vector2 dir = Vector2.zero;

        // 플레이어가 바라보는 방향으로 선을 뻗게 함
        if (player != null)
            dir = player.right.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        foreach (var img in lines)
        {
            RectTransform rt = img.rectTransform;
            float randomY = Random.Range(-Screen.height * 0.45f, Screen.height * 0.45f);
            float randomX = Random.Range(-50f, 50f);

            rt.anchoredPosition = new Vector2(randomX, randomY);
            rt.rotation = Quaternion.Euler(0, 0, angle + Random.Range(-5f, 5f));

            img.enabled = true;
        }

        yield return new WaitForSeconds(showDuration);

        foreach (var img in lines)
            img.enabled = false;

        isShowing = false;
    }
}
