using UnityEngine;

public class TextMeshSortingOrder : MonoBehaviour
{
    [SerializeField] private int sortingOrder = 100; // ✅ 기본적으로 최전방 배치

    private Renderer textRenderer;

    void Start()
    {
        textRenderer = GetComponent<Renderer>();

        if (textRenderer != null)
        {
            textRenderer.sortingOrder = sortingOrder; // ✅ 우선순위 적용
        }
        else
        {
            Debug.LogWarning("TextMeshSortingOrder: Renderer가 없습니다. TextMesh가 있는지 확인하세요!");
        }
    }

    // ✅ 필요하면 실시간으로 우선순위 변경 가능
    public void SetSortingOrder(int newOrder)
    {
        if (textRenderer != null)
        {
            sortingOrder = newOrder;
            textRenderer.sortingOrder = sortingOrder;
        }
    }
}
