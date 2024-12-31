using UnityEngine;
using System.Collections;

public class Object3Script : MonoBehaviour
{
    public PlayerMove playerMove; // �÷��̾� ��ũ��Ʈ�� ����
    public float bounceForce = 10f; // �ݵ����� ƨ�� ������ ��
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
            rigidbody2D.gravityScale = 1; // �߷� Ȱ��ȭ
            collider2D.enabled = false; // �ݶ��̴� ��Ȱ��ȭ
            StartCoroutine(DestroyAfterFall());
        }
    }

    private IEnumerator DestroyAfterFall()
    {
        yield return new WaitForSeconds(5f); // 5�� �� ������ ����
        Destroy(gameObject);
    }
}
