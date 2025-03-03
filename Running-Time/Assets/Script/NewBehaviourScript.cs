using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedScript : MonoBehaviour
{
    // EnemyMove 관련 변수들
    Rigidbody2D rigid;
    public int nextMove;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsulecollider;

    // Object1Script (Buttons 역할)
    public Object1Script object1Script;
    public float moveHeight = 5f;
    public float moveSpeed = 2f;
    public bool what = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    void Awake()
    {
        // EnemyMove 초기화
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsulecollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think", 5);

        // Object2Script 초기화
        originalPosition = transform.position;
        targetPosition = originalPosition;

        // object1Script (실제 Buttons 역할) 자동 할당
        if (object1Script == null)
        {
            object1Script = FindObjectOfType<Object1Script>(); // Buttons가 아니라 Object1Script로 변경!
            if (object1Script == null)
            {
                Debug.LogError("⚠️ Object1Script (버튼 역할) 를 찾을 수 없습니다. 씬에 Object1Script가 있는지 확인하세요.");
            }
        }
    }

    void FixedUpdate()
    {
        // EnemyMove 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        // Object1Script 이동 (Buttons 역할)
        if (object1Script != null && object1Script.isButtonPressed)
        {
            anim.SetBool("what", true);
        }
        else
        {
            anim.SetBool("what", false);
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
