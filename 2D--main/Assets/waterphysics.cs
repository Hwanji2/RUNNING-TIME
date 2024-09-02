using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour
{
    public AudioClip waterEnterSound; // ���� �� �� �Ҹ�
    public AudioClip waterExitSound; // ������ ���� �� �Ҹ�
    private AudioSource audioSource; // ����� �ҽ�

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 0.3f; // �� �ӿ��� �߷� ����
                playerRigidbody.drag = 3f; // �� �ӿ��� ���� ����

                // ���� �� �� �Ҹ� ���
                if (audioSource != null && waterEnterSound != null)
                {
                    audioSource.PlayOneShot(waterEnterSound);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
            if (playerRigidbody != null)
            {
                playerRigidbody.gravityScale = 5f; // ���� �߷����� ����
                playerRigidbody.drag = 0f; // ���� �������� ����

                // ������ ���� �� �Ҹ� ���
                if (audioSource != null && waterExitSound != null)
                {
                    audioSource.PlayOneShot(waterExitSound);
                }
            }
        }
    }
}
