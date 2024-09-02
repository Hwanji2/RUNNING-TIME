using UnityEngine;
using System.Collections;

public class Object1Script : MonoBehaviour
{
    public bool isButtonPressed = false;
    public float pressDistance = 0.2f; // 버튼이 눌릴 거리
    public float pressSpeed = 0.5f; // 버튼이 눌릴 속도
    public AudioClip buttonPressSound; // 버튼 눌림 소리

    private Vector3 originalPosition;
    private AudioSource audioSource; // 오디오 소스

    void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            // 플레이어의 윗 부분과 닿았는지 확인
            if (collision.contacts[0].normal.y < -0.5f)
            {
                // 버튼이 눌리는 순간 소리 재생
                if (audioSource != null && buttonPressSound != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(buttonPressSound);
                }
                isButtonPressed = true;
                StopAllCoroutines(); // 이전에 실행 중인 코루틴이 있으면 중지
                StartCoroutine(PressButton());
            }
        }
    }

    private IEnumerator PressButton()
    {
        Vector3 pressedPosition = originalPosition - new Vector3(0, pressDistance, 0);

        // 버튼을 아래로 이동
        while (Vector3.Distance(transform.position, pressedPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pressedPosition, pressSpeed * Time.deltaTime);
      


            yield return null;
        }

        yield return new WaitForSeconds(3f); // 3초 대기

        // 버튼을 원래 위치로 이동
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }

        isButtonPressed = false;
    }
}
