using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnButton : MonoBehaviour
{
    [Header("🎯 이동할 씬 이름")]
    public string targetSceneName; // 인스펙터에서 지정할 씬 이름

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

        Debug.Log($"▶ {targetSceneName} 씬으로 즉시 이동합니다.");
        Time.timeScale = 1f; // 혹시 멈춰 있던 경우를 대비
        SceneManager.LoadScene(targetSceneName);
    }
}
