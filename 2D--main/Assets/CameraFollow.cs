using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public Vector3 preInitialCameraPosition; // �ʱ� ��ġ �� �ʱ� ��ġ
    public Vector3 initialCameraPosition; // �ʱ� ī�޶� ��ġ
    public float smoothTime = 0.3f; // �ε巯�� �̵��� ���� �ð�
    public Vector3[] stopPositions; // ī�޶� ���� ��ġ �迭
    public Vector3[] resumePositions; // ī�޶� �ٽ� ����ٴ� ��ġ �迭
    public Vector2[] followOffsets; // �� resumePosition�� ���� followOffset �迭

    private Vector3 velocity = Vector3.zero;
    private bool isFollowing = true;
    private bool isMovingToInitial = true;
    private bool isOffsetChanging = false; // ������ ���� ������ ����
    private float transitionDuration = 1.0f; // �ʱ� ��ġ�� �̵��ϴ� �� �ɸ��� �ð�

    void Start()
    {
        // �ʱ� ��ġ �� �ʱ� ��ġ�� ����
        transform.position = preInitialCameraPosition;
        isFollowing = false;
    }

    void LateUpdate()
    {
        if (isMovingToInitial)
        {
            // �ʱ� ��ġ�� �ε巴�� �̵�
            transform.position = Vector3.SmoothDamp(transform.position, initialCameraPosition, ref velocity, smoothTime);

            if (Vector3.Distance(transform.position, initialCameraPosition) < 0.1f)
            {
                isMovingToInitial = false;
            }
        }
        else if (isFollowing)
        {
            // �÷��̾��� ��ġ�� ���� ī�޶��� ���ο� ��ġ ���
            Vector3 targetPosition = player.position + (Vector3)followOffsets[0]; // �⺻ followOffset ���
            targetPosition.z = transform.position.z;

            // ī�޶� ���� ��ġ Ȯ�� (x��ǥ�� ��)
            foreach (Vector3 stopPosition in stopPositions)
            {
                if (Mathf.Abs(player.position.x - stopPosition.x) < 0.1f)
                {
                    isFollowing = false;
                    break;
                }
            }

            // �ε巴�� ī�޶� ��ġ ������Ʈ
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            // ī�޶� �ٽ� ����ٴ� ��ġ Ȯ�� (x��ǥ�� ��)
            for (int i = 0; i < resumePositions.Length; i++)
            {
                if (Mathf.Abs(player.position.x - resumePositions[i].x) < 0.1f)
                {
                    isFollowing = true;

                    followOffsets[0] = followOffsets[i]; // �ش� ��ġ�� followOffset���� ����
                    break;
                }
            }
        }

        // �����̽� Ű�� ������ �� ������ ����
        if (Input.GetKeyDown(KeyCode.Space) && !isOffsetChanging)
        {
            StartCoroutine(ChangeFollowOffsetTemporarily());
        }

        // �ƹ� Ű�� ������ �� isFollowing�� true�� �����ϰ� followOffset ����
        if (Input.anyKeyDown)
        {
            isFollowing = true;
            followOffsets[0] = followOffsets[Random.Range(0, followOffsets.Length)]; // ������ followOffset���� ����
        }
    }

        

    // followOffsets �迭�� �����ϴ� �޼���
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

    // followOffsets �迭�� �ʱ�ȭ�ϴ� �޼���
    public void ResetFollowOffsets()
    {
        for (int i = 0; i < followOffsets.Length; i++)
        {
            followOffsets[i] = Vector2.zero; // �⺻������ �ʱ�ȭ
        }
    }

    // followOffsets�� y��ǥ�� ��� �����ϴ� �ڷ�ƾ
    private IEnumerator ChangeFollowOffsetTemporarily()
    {
        isOffsetChanging = true; // ������ ���� �� ���·� ����
        Vector2 originalOffset = followOffsets[0];
        followOffsets[0] = new Vector2(followOffsets[0].x, followOffsets[0].y - 3);
        yield return new WaitForSeconds(0.5f); // 1�� �� ���� ���·� ����
        followOffsets[0] = originalOffset;
        isOffsetChanging = false; // ������ ���� �Ϸ� ���·� ����
    }

    // followOffsets�� x��ǥ�� ��� �����ϴ� �ڷ�ƾ
    private IEnumerator ChangeFollowOffsetXTemporarily(float xChange)
    {
        Vector2 originalOffset = followOffsets[0];
        followOffsets[0] = new Vector2(followOffsets[0].x + xChange, followOffsets[0].y);
        yield return new WaitUntil(() => !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift));
        followOffsets[0] = originalOffset;
    }
}
