using UnityEngine;
using System.Collections;

public class Object3Script : MonoBehaviour
{
    public PlayerMove playerMove; // 플레이어 스크립트를 참조
    public float bounceForce = 10f; // 반동으로 튕겨 나가는 힘
    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerMove.running)
        {
            Debug.Log("Collision Detected with Player");
            Vector2 bounceDirection = (collision.transform.position - transform.position).normalized;
            rigidbody2D.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
            rigidbody2D.gravityScale = 1; // 중력 활성화
            collider2D.enabled = false; // 콜라이더 비활성화
            StartCoroutine(DestroyAfterFall());
        }
    }

    private IEnumerator DestroyAfterFall()
    {
        yield return new WaitForSeconds(5f); // 5초 후 오브제 삭제
        Destroy(gameObject);
    }
}
