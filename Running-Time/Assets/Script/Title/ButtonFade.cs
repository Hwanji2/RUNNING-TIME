using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ESCButtonToggle : MonoBehaviour
{
    [Header("🔹 ESC 키를 누르면 사라질 버튼들")]
    public List<Button> buttonsToToggle = new List<Button>(); // Inspector에서 버튼 지정

    private bool isHidden = true; // 버튼이 현재 사라졌는지 여부
    private List<CanvasGroup> buttonCanvasGroups = new List<CanvasGroup>();

    void Start()
    {
        // 버튼들에 CanvasGroup 추가 (없으면 자동 추가)
        foreach (Button button in buttonsToToggle)
        {
            if (button != null)
            {
                CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
                }
                buttonCanvasGroups.Add(canvasGroup);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleButtons(!isHidden);
            isHidden = !isHidden;
        }
    }

    void ToggleButtons(bool show)
    {
        foreach (CanvasGroup canvasGroup in buttonCanvasGroups)
        {
            canvasGroup.alpha = show ? 1 : 0;
            canvasGroup.interactable = show; // 숨겨진 상태에서는 클릭 불가능
            canvasGroup.blocksRaycasts = show;
        }
    }
}
