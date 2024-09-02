using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWater : MonoBehaviour
{
    public float shockDuration = 2f; // ���� ���� �ð�
    public float shockInterval = 0.5f; // ���� ����
    public int damageAmount = 10; // ���� �� �Դ� ������
    public AudioClip shockSound; // ���� ���� Ŭ��
    private AudioSource audioSource; // ����� �ҽ�
    public bool gamjeon = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShockPlayer(collision.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines(); // ���� ����
            gamjeon = false;
        }
    }

    private IEnumerator ShockPlayer(GameObject player)
    {
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        if (playerRigidbody != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < shockDuration)
            {
                playerRigidbody.gravityScale = 0.1f; // �� �ӿ��� �߷� ����

                // ���� ������ ���� (����)
                player.SendMessage("OnDamaged", Vector2.zero);
                gamjeon = true;

                // ������ ���
                if (audioSource != null && shockSound != null)
                {
                    audioSource.PlayOneShot(shockSound);
                }

                elapsedTime += shockInterval;
                yield return new WaitForSeconds(shockInterval);
            }
            gamjeon = false;
            playerRigidbody.gravityScale = 0.3f; // �� �ӿ��� �߷� ����
        }
    }
}
