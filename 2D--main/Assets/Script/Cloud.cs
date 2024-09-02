using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2.0f; // ������ �̵� �ӵ�
    public float resetPositionX = -30.0f; // ������ ���� ���� �������� ���� X ��ǥ
    public float startPositionX = 30.0f; // ������ �ݴ��� ������ �ٽ� ������ ���� X ��ǥ

    void Update()
    {
        // ������ �������� �̵�
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // ������ ���� �����ǿ� �����ߴ��� Ȯ��
        if (IsAtResetPosition())
        {
            // ������ �ݴ��� ������ �̵�
            ResetCloudPosition();
        }
    }

    // ������ ���� �����ǿ� �����ߴ��� Ȯ���ϴ� �Լ�
    bool IsAtResetPosition()
    {
        return transform.position.x <= resetPositionX;
    }

    // ������ ��ġ�� �ݴ��� ������ �̵���Ű�� �Լ�
    void ResetCloudPosition()
    {
        Vector2 newPosition = new Vector2(startPositionX, transform.position.y);
        transform.position = newPosition;
    }
}