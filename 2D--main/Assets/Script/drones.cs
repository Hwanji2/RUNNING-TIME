using UnityEngine;

public class Object2Script : MonoBehaviour
{
    public Object1Script object1Script; // ������1�� ��ũ��Ʈ�� ����
    public float moveHeight = 5f; // �ö� ����
    public float moveSpeed = 2f; // �̵� �ӵ�
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Start()
    {
        originalPosition = transform.position; // �ʱ� ��ġ ����
        targetPosition = originalPosition; // �ʱ� Ÿ�� ��ġ ����
    }

    void Update()
    {
        if (object1Script.isButtonPressed)
        {
            targetPosition = new Vector3(transform.position.x, originalPosition.y + moveHeight, transform.position.z);
        }
        else
        {
            targetPosition = originalPosition;
        }

        // �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
