using UnityEngine;
using System.Collections;

public class Object1Script : MonoBehaviour
{
    public bool isButtonPressed = false;
    public float pressDistance = 0.2f; // ��ư�� ���� �Ÿ�
    public float pressSpeed = 0.5f; // ��ư�� ���� �ӵ�
    public AudioClip buttonPressSound; // ��ư ���� �Ҹ�

    private Vector3 originalPosition;
    private AudioSource audioSource; // ����� �ҽ�

    void Start()
    {
        originalPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            // �÷��̾��� �� �κа� ��Ҵ��� Ȯ��
            if (collision.contacts[0].normal.y < -0.5f)
            {
                // ��ư�� ������ ���� �Ҹ� ���
                if (audioSource != null && buttonPressSound != null && !audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(buttonPressSound);
                }
                isButtonPressed = true;
                StopAllCoroutines(); // ������ ���� ���� �ڷ�ƾ�� ������ ����
                StartCoroutine(PressButton());
            }
        }
    }

    private IEnumerator PressButton()
    {
        Vector3 pressedPosition = originalPosition - new Vector3(0, pressDistance, 0);

        // ��ư�� �Ʒ��� �̵�
        while (Vector3.Distance(transform.position, pressedPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pressedPosition, pressSpeed * Time.deltaTime);
      


            yield return null;
        }

        yield return new WaitForSeconds(3f); // 3�� ���

        // ��ư�� ���� ��ġ�� �̵�
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originalPosition, pressSpeed * Time.deltaTime);
            yield return null;
        }

        isButtonPressed = false;
    }
}
