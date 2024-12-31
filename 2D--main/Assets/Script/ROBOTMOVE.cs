using UnityEngine;

public class ROBOTMOVE : MonoBehaviour
{
    public Object1Script object1Script;
    public Rigidbody2D rigid;
    public Animator anim;
    public SpriteRenderer spriteRenderer;
    public float jumpPower = 10f;
    public float maxSpeed = 5f;
    public CapsuleCollider2D capsulecollider;
    public Vector2 slidingOffset;
    public Vector2 slidingSize;
    public Vector2 originalOffset;
    public Vector2 originalSize;
    public bool isjump = false;
    public bool isswap = false;
    public float accel = 0f;

    public Transform player; // 플레이어의 Transform을 할당하세요.
    public float detectionRadius = 10.0f; // 반경을 설정하세요.
    public bool isLinked = false; // 로봇 연동 상태

    void Update()
    {
        // 플레이어와 연동 로봇 간의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 플레이어가 반경 내에 있는지 확인
        if (distanceToPlayer <= detectionRadius && object1Script.isButtonPressed)
        {
            isLinked = true;
            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                rigid.AddForce(Vector2.up * jumpPower * 0.8f, ForceMode2D.Impulse);
                isjump = true;
                isswap = false;
                anim.SetBool("New Bool", false);
            }

            // Stop Speed
            if (Input.GetButtonUp("Horizontal"))
            {
                rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.8f, rigid.velocity.y);
                if (accel == 0)
                {
                    anim.SetBool("New Bool", false);
                    isswap = false;
                }
            }

            // Direction Sprite
            if (Input.GetButton("Horizontal"))
            {
                anim.SetBool("New Bool", true);
                spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
            }
        }
        else
        {
            isLinked = false;
        }
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if ((object1Script.isButtonPressed)&& (distanceToPlayer <= detectionRadius && object1Script.isButtonPressed))
        {
            if (Input.GetButtonDown("RUN"))
            {
                if (isjump && Input.GetButtonDown("RUN"))
                {
                    accel = accel + 10;
                    anim.SetBool("New Bool", true);
                }
                else
                {
                    accel = accel + 1;
                }
            }
            else
            {
                if (accel >= 1.1f && !isjump && !isswap)
                    accel = accel - 1;
            }

            // Move Speed
            float h = Input.GetAxisRaw("Horizontal");
            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            // Max Speed
            if (rigid.velocity.x > accel * maxSpeed) // Right Max Speed
                rigid.velocity = new Vector2(accel * maxSpeed, rigid.velocity.y);
            else if (rigid.velocity.x < accel * maxSpeed * (-1)) // Left Max Speed
                rigid.velocity = new Vector2(accel * maxSpeed * (-1), rigid.velocity.y);

            // Landing Platform
            if (rigid.velocity.y < 0)
            {
                Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
                RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

                if (rayHit.collider != null)
                {
                    if (rayHit.distance < 0.5f)
                    {
                        isjump = false;
                    }
                }
            }
        }
    }



    void SetSlidingCollider()
    {
        capsulecollider.offset = slidingOffset;
        capsulecollider.size = slidingSize;
    }

    void ResetCollider()
    {
        capsulecollider.offset = originalOffset;
        capsulecollider.size = originalSize;
    }
}
