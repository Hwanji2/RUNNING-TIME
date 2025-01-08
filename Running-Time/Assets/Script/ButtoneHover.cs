using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button buttonToScale;
    public float scaleMultiplier = 1.2f;
    public float scaleSpeed = 5f;

    private Vector3 initialScale;
    private bool isHovered = false;

    void Awake() // Start 대신 Awake 사용
    {
        if (buttonToScale != null)
        {
            initialScale = buttonToScale.transform.localScale;
        }
    }

    void LateUpdate() // Update 대신 LateUpdate 사용
    {
        if (buttonToScale != null)
        {
            Vector3 targetScale = isHovered ? initialScale * scaleMultiplier : initialScale;
            buttonToScale.transform.localScale = Vector3.Lerp(buttonToScale.transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
