using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricWater : MonoBehaviour
{
    public float shockDuration = 2f; // 감전 지속 시간
    public float shockInterval = 0.5f; // 감전 간격
    public int damageAmount = 10; // 감전 시 입는 데미지
    public AudioClip shockSound; // 감전 사운드 클립
    private AudioSource audioSource; // 오디오 소스
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
            StopAllCoroutines(); // 감전 중지
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
                playerRigidbody.gravityScale = 0.1f; // 물 속에서 중력 감소

                // 감전 데미지 적용 (예시)
                player.SendMessage("OnDamaged", Vector2.zero);
                gamjeon = true;

                // 감전음 재생
                if (audioSource != null && shockSound != null)
                {
                    audioSource.PlayOneShot(shockSound);
                }

                elapsedTime += shockInterval;
                yield return new WaitForSeconds(shockInterval);
            }
            gamjeon = false;
            playerRigidbody.gravityScale = 0.3f; // 물 속에서 중력 감소
        }
    }
}
