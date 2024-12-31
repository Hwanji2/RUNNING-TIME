using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;  // 행동지표를 결정할 변수 하나를 생성
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsulecollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsulecollider = GetComponent<CapsuleCollider2D>();

        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        // Move
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        // 이동하는 경로의 상태를 예측해야 하므로 앞을 체크해야 함.
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

    }

    //재귀함수 : 자신을 스스로 호출하는 함수
    void Think() // 행동지표를 바꿔줄 함수 하나를 생성
    {

        // Set Next Active
        // Random : 랜덤 수를 생성하는 로직 관련 클래스
        nextMove = Random.Range(-1, 2);
        
     
        // Sprite Animation
        anim.SetInteger("WalkSpeed", nextMove);

        // Flip Sprite
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == 1;
        }
        
        // Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);     // 딜레이 없이 재귀함수를 사용하는 것은 아주 위험.!!  ==>> 우리는 시간이 조금 지난 뒤 이 함수를 실행시키고 싶다.

    }

    void Turn()
    {
        nextMove *= -1;
        // CancelInvoke() : 현재 작동 중인 모든 Invoke 함수를 멈추는 함수
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDamaged()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite Flip Y
        spriteRenderer.flipY = true;

        // Collider Disable
        capsulecollider.enabled = false;

        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Destory          비활성화 로직은 시간차를 두어 실행시킨다.
        Invoke("DeActive", 5);

    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

}
