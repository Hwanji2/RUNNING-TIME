using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Vector3 preInitialCameraPosition; // 초기 위치 전 초기 위치
    public Vector3 initialCameraPosition; // 초기 카메라 위치
    public float smoothTime = 0.3f; // 부드러운 이동을 위한 시간
    public Vector3[] stopPositions; // 카메라가 멈출 위치 배열
    public Vector3[] resumePositions; // 카메라가 다시 따라다닐 위치 배열
    public Vector2[] followOffsets; // 각 resumePosition에 대한 followOffset 배열

    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = true;
    private bool isMovingToInitial = true;
    private bool isOffsetChanging = false; // 오프셋 변경 중인지 여부
    private float transitionDuration = 1.0f; // 초기 위치로 이동하는 데 걸리는 시간

    void Start()
    {
        // 초기 위치 전 초기 위치로 설정
        transform.position = preInitialCameraPosition;
        isFollowing = false;
    }

    void LateUpdate()
    {
        if (isMovingToInitial)
        {
            // 초기 위치로 부드럽게 이동
            transform.position = Vector3.SmoothDamp(transform.position, initialCameraPosition, ref velocity, smoothTime);

            if (Vector3.Distance(transform.position, initialCameraPosition) < 0.1f)
            {
                isMovingToInitial = false;
            }
        }
        else if (isFollowing)
        {
            // 플레이어의 위치를 따라 카메라의 새로운 위치 계산
            Vector3 targetPosition = player.position + (Vector3)followOffsets[0]; // 기본 followOffset 사용
            targetPosition.z = transform.position.z;

            // 카메라가 멈출 위치 확인 (x좌표만 비교)
            foreach (Vector3 stopPosition in stopPositions)
            {
                if (Mathf.Abs(player.position.x - stopPosition.x) < 0.1f)
                {
                    isFollowing = false;
                    break;
                }
            }

            // 부드럽게 카메라 위치 업데이트
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            // 카메라가 다시 따라다닐 위치 확인 (x좌표만 비교)
            for (int i = 0; i < resumePositions.Length; i++)
            {
                if (Mathf.Abs(player.position.x - resumePositions[i].x) < 0.1f)
                {
                    isFollowing = true;

                    followOffsets[0] = followOffsets[i]; // 해당 위치의 followOffset으로 변경
                    break;
                }
            }
        }

        // 스페이스 키를 눌렀을 때 오프셋 변경
        if (Input.GetKeyDown(KeyCode.Space) && !isOffsetChanging)
        {
            StartCoroutine(ChangeFollowOffsetTemporarily());
        }

        // 아무 키나 눌렀을 때 isFollowing을 true로 설정하고 followOffset 변경
        if (Input.anyKeyDown)
        {
            isFollowing = true;
            followOffsets[0] = followOffsets[Random.Range(0, followOffsets.Length)]; // 랜덤한 followOffset으로 변경
        }
    }

        

    // followOffsets 배열을 수정하는 메서드
    public void SetFollowOffset(int index, Vector2 newOffset)
    {
        if (index >= 0 && index < followOffsets.Length)
        {
            followOffsets[index] = newOffset;
        }
        else
        {
            Debug.LogWarning("Invalid index for followOffsets array.");
        }
    }

    // followOffsets 배열을 초기화하는 메서드
    public void ResetFollowOffsets()
    {
        for (int i = 0; i < followOffsets.Length; i++)
        {
            followOffsets[i] = Vector2.zero; // 기본값으로 초기화
        }
    }

    // followOffsets의 y좌표를 잠깐 변경하는 코루틴
    private IEnumerator ChangeFollowOffsetTemporarily()
    {
        isOffsetChanging = true; // 오프셋 변경 중 상태로 설정
        Vector2 originalOffset = followOffsets[0];
        followOffsets[0] = new Vector2(followOffsets[0].x, followOffsets[0].y - 3);
        yield return new WaitForSeconds(0.5f); // 1초 후 원래 상태로 복원
        followOffsets[0] = originalOffset;
        isOffsetChanging = false; // 오프셋 변경 완료 상태로 설정
    }

    // followOffsets의 x좌표를 잠깐 변경하는 코루틴
    private IEnumerator ChangeFollowOffsetXTemporarily(float xChange)
    {
        Vector2 originalOffset = followOffsets[0];
        followOffsets[0] = new Vector2(followOffsets[0].x + xChange, followOffsets[0].y);
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
        followOffsets[0] = originalOffset;
    }
}
