using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 5f; // 점프 힘
    public float jumpInterval = 2f; // 점프 간격
    private Rigidbody2D rigid;
    private Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        InvokeRepeating("Jump", 0, jumpInterval); // 주기적으로 점프

    }

    void Jump()
    {
        // 점프 애니메이션 트리거
        anim.SetTrigger("Jump");

        gameObject.SetActive(true);
        // 점프 로직
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}

