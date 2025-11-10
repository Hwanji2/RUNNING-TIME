using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 2.0f; // 구름의 이동 속도
    public float resetPositionX = -30.0f; // 구름이 맵의 끝에 도달했을 때의 X 좌표
    public float startPositionX = 30.0f; // 구름이 반대쪽 끝에서 다시 시작할 때의 X 좌표

    void Update()
    {
        // 구름을 왼쪽으로 이동
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // 구름이 리셋 포지션에 도달했는지 확인
        if (IsAtResetPosition())
        {
            // 구름을 반대쪽 끝으로 이동
            ResetCloudPosition();
        }
    }

    // 구름이 리셋 포지션에 도달했는지 확인하는 함수
    bool IsAtResetPosition()
    {
        return transform.position.x <= resetPositionX;
    }

    // 구름의 위치를 반대쪽 끝으로 이동시키는 함수
    void ResetCloudPosition()
    {
        Vector2 newPosition = new Vector2(startPositionX, transform.position.y);
        transform.position = newPosition;
    }
}