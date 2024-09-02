using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics : MonoBehaviour
{
    public AudioClip waterEnterSound; // 물에 들어갈 때 소리
    public AudioClip waterExitSound; // 물에서 나올 때 소리
    private AudioSource audioSource; // 오디오 소스

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
                playerRigidbody.gravityScale = 0.3f; // 물 속에서 중력 감소
                playerRigidbody.drag = 3f; // 물 속에서 저항 증가

                // 물에 들어갈 때 소리 재생
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
                playerRigidbody.gravityScale = 5f; // 원래 중력으로 복구
                playerRigidbody.drag = 0f; // 원래 저항으로 복구

                // 물에서 나올 때 소리 재생
                if (audioSource != null && waterExitSound != null)
                {
                    audioSource.PlayOneShot(waterExitSound);
                }
            }
        }
    }
}
