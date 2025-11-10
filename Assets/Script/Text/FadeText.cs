using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro 지원
using System.Collections.Generic;

public class ESCToggleText : MonoBehaviour
{
    [Header("🔹 ESC 키를 누르면 사라질 텍스트들")]
    public List<Text> uiTexts = new List<Text>(); // Unity UI Text 리스트
    public List<TMP_Text> tmpTexts = new List<TMP_Text>(); // TextMeshPro TMP_Text 리스트

    private bool isHidden = true; // 텍스트가 현재 사라졌는지 여부

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleTexts(!isHidden);
            isHidden = !isHidden;
        }
    }

    void ToggleTexts(bool show)
    {
        foreach (Text text in uiTexts)
        {
            if (text != null)
            {
                text.enabled = show;
            }
        }

        foreach (TMP_Text tmpText in tmpTexts)
        {
            if (tmpText != null)
            {
                tmpText.enabled = show;
            }
        }
    }
}
