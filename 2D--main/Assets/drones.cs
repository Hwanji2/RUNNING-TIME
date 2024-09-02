using UnityEngine;

public class Object2Script : MonoBehaviour
{
    public Object1Script object1Script; // 오브제1의 스크립트를 참조
    public float moveHeight = 5f; // 올라갈 높이
    public float moveSpeed = 2f; // 이동 속도
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Start()
    {
        originalPosition = transform.position; // 초기 위치 저장
        targetPosition = originalPosition; // 초기 타겟 위치 설정
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

        // 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
