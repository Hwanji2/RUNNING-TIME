using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [Header("🎯 이동할 씬 이름")]
    public string targetSceneName; // 인스펙터에서 지정할 씬 이름

    [Header("⏳ 페이드 시간 (선택)")]
    public float fadeDuration = 0f; // 페이드 없이 즉시 이동하려면 0

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("❌ Button 컴포넌트가 없습니다. UI Button 오브젝트에 이 스크립트를 붙이세요.");
            return;
        }

        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning("⚠️ 이동할 씬 이름이 지정되지 않았습니다.");
            return;
        }

        Debug.Log($"▶ {targetSceneName} 씬으로 이동합니다.");

        if (fadeDuration > 0f)
            StartCoroutine(FadeAndLoadScene());
        else
            SceneManager.LoadScene(targetSceneName);
    }

    private System.Collections.IEnumerator FadeAndLoadScene()
    {
        CanvasGroup fadeCanvas = new GameObject("FadeCanvas").AddComponent<CanvasGroup>();
        Canvas canvas = fadeCanvas.gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeCanvas.gameObject.AddComponent<UnityEngine.UI.Image>().color = Color.black;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(targetSceneName);
    }
}
