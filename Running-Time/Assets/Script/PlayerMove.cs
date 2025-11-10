using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioSlide;
    public AudioClip dam;
    public AudioClip audiosecret;
    private float seVolume = 1.0f; // SE 볼륨 변수 (초기값 1.0f)

    public AudioClip audioFinish;
    public int secretCount = 0;
    public bool itemEat = false;
    public bool isFalling = false;
    public bool isDon = false;
    public bool tuto = false;
    public bool tuto2 = false;
    public bool finish = false;

    public float maxSpeed;
    public float jumpPower;
    public float accel = 1;
    public bool running = false; // running 상태 변수

    private bool isInvincible = false;
    private float originalSpeed;
    private float originalJumpPower;
    private bool isjump = false;
    private bool isswap = false;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsulecollider;
    AudioSource audioSource;
    private Vector2 originalOffset;
    private Vector2 slidingOffset = new Vector2(0.1f, 0.9f);
    private Vector2 originalSize;
    private Vector2 slidingSize = new Vector2(3f, 1.9f);

    public GameObject afterImagePrefab; // 잔상 오브제 프리팹
    private float afterImageInterval = 0.1f; // 잔상 생성 간격
    private float afterImageTimer = 0f;
    private Vector3 previousPosition; // 이전 위치 저장 변수



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsulecollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();

        originalSpeed = maxSpeed;
        originalJumpPower = jumpPower;

        originalOffset = capsulecollider.offset;
        originalSize = capsulecollider.size;

        previousPosition = transform.position; // 초기 위치 설정
                                               // 저장된 SE 볼륨 불러오기
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);
        audioSource.volume = seVolume;
    }

    void Update()
    {
        if (Input.GetButtonDown("RUN") || (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
    Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) ||
    Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L) ||
    Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C) ||
    Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.N) ||
    Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.Period) ||
    Input.GetKeyDown(KeyCode.Slash)))
        {
            if (isjump)
            {
                accel = accel + 10;
            }
            else
            {
                accel = accel + 1;
            }
        }
        else
        {
            if (accel >= 1.1f && !anim.GetBool("isWalking") && !isjump && !isswap)
                accel = accel - 1;
        }
        // Jump
        if ((Input.GetButtonDown("Jump") || Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow)) && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower * 0.8f, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            isjump = true;
            isswap = false;
            itemEat = false;

            PlaySound("JUMP");
        }
        if ((Input.GetButtonDown("Sliding") || Input.GetKey(KeyCode.S)) && (!anim.GetBool("isSliding") || !anim.GetBool("slide Bool")))
        {
            if (accel > 10)
            {
                anim.SetBool("isSliding", true);
                PlaySound("JUMP");
                StartCoroutine(StopSliding());
                SetSlidingCollider();
                itemEat = false;

            }
            else
            {
                anim.SetBool("slide Bool", true);
                StartCoroutine(StopSlide());
                SetSlidingCollider();
            }
            capsulecollider.direction = CapsuleDirection2D.Horizontal;
        }
        else if (Input.GetButtonUp("Sliding"))
        {
            ResetCollider();
            capsulecollider.direction = CapsuleDirection2D.Vertical;
        }
      
        IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("isSliding", false);
        ResetCollider();
    }

    IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("slide Bool", false);
        ResetCollider();
    }

      
        if (Input.GetButtonUp("Sliding") && anim.GetBool("isSliding"))
        {
            anim.SetBool("isSliding", false);

            PlaySound("JUMP");
        }
        // Stop Speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.8f, rigid.velocity.y);
            if (accel == 0)
            {
                isswap = false;
            }
        }

        // Direction Sprite
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
    

        // Animation
        if (Mathf.Abs(rigid.velocity.x) >= 0.5 && !anim.GetBool("isJumping"))
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        // Running 상태 업데이트
        running = Mathf.Abs(rigid.velocity.x) > 30;
        if (accel > 100)
        {
            afterImageTimer += Time.deltaTime;
            if (afterImageTimer >= afterImageInterval)
            {
                CreateAfterImage();
                afterImageTimer = 0f;
            }
        }
    }
    void CreateAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, previousPosition, Quaternion.identity); // 이전 위치에 잔상 생성
        Color afterImageColor = Color.Lerp(Color.white, Color.red, (accel - 100) / 100f);
        afterImage.GetComponent<AfterImage>().Initialize(spriteRenderer.sprite, previousPosition, spriteRenderer.flipX, afterImageColor, anim);
        previousPosition = transform.position; // 현재 위치를 이전 위치로 업데이트
    }
    void SetSlidingCollider()
    {
        if (anim.GetBool("slide Bool")|| anim.GetBool("isSliding"))
        {
            PlaySound("slide");
            capsulecollider.offset = slidingOffset;
            capsulecollider.size = slidingSize;
        }
    }

    void ResetCollider()
    {
        capsulecollider.offset = originalOffset;
        capsulecollider.size = originalSize;
    }

    void FixedUpdate()
    {

        if(accel>30)
        {
            anim.SetBool("Run one", true);
            if (accel > 100)
            {
                afterImageTimer += Time.deltaTime;
                if (afterImageTimer >= afterImageInterval)
                {
                    CreateAfterImage();
                    afterImageTimer = 0f;
                }
            }
        }
        else
        {
            anim.SetBool("Run one", false);

        }
        if (Input.GetButtonDown("RUN") || Input.anyKeyDown)
        {
            if (isjump)
            {
                accel = accel + 10;
            }
            else
            {
                accel = accel + 1;
            }
        }
        else
        {
            if (accel >= 1.1f && !anim.GetBool("isWalking") && !isjump && !isswap)
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
                    anim.SetBool("isJumping", false);
                    isjump = false;
                }
            }
            if (rigid.velocity.y < -15)
                isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (accel <=30 )
            {

                // Attack
                if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                {
                    OnAttack(collision.transform);
                    PlaySound("ATTACK");
                }
                else
                {
                    OnDamaged(collision.transform.position);
                    PlaySound("DAMAGED");
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            StartCoroutine(HandleItemCollision(collision));
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage
            gameManager.NextStage();

            // Sound
            PlaySound("FINISH");
        }
        else if (collision.gameObject.tag == "tuto")
        {
            tuto = true;
            gameManager.NextStage();
        }
        else if (collision.gameObject.tag == "tuto2")
        {
            tuto2 = true;
            gameManager.NextStage();
        }
        else if (collision.gameObject.tag == "Finish")
        {
            finish = true;
            gameManager.NextStage();
        }
        else if (collision.gameObject.tag == "what")
        {
            // 특정 좌표로 이동
            transform.position = new Vector2(10f, 5f); // 원하는 좌표로 변경
        }
    }

    private IEnumerator HandleItemCollision(Collider2D collision)
    {
       

        // Point
        bool isBronze = collision.gameObject.name.Contains("Bronze");
        bool isSilver = collision.gameObject.name.Contains("Silver");
        bool isGold = collision.gameObject.name.Contains("Gold");
        bool isDia = collision.gameObject.name.Contains("Dia");

        bool isSecret = collision.gameObject.name.Contains("시크릿");

        if (isBronze)
        {
            isDon = true;

            gameManager.stagePoint += 10;
            PlaySound("ITEM");
        }
        else if (isSilver)
        {
            isDon = true;

            gameManager.stagePoint += 100;
            PlaySound("ITEM");
        }
        else if (isGold)
        {
            isDon = true;
            gameManager.stagePoint += 500;
            PlaySound("ITEM");
        }
        else if (isSecret)
        {
            itemEat = true;
            secretCount = secretCount + 1;
            PlaySound("secret");
        }
        else if (isDia)
        {
            isDon = true;

            gameManager.stagePoint += 999999;
            PlaySound("ITEM");
        }
        // Deactivate Item
        collision.gameObject.SetActive(false);


        // 1초 대기
        yield return new WaitForSeconds(1.0f);

        itemEat = false;
        isDon = false;

    }
    void OnAttack(Transform enemy)
    {
        // Point
        gameManager.stagePoint += 100;

        // Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

    }

    public void OnDamaged(Vector2 targetPos)
    {
        if (isInvincible) return;

        // Change Layer (Immortal Active)
        gameObject.layer = 11;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = spriteRenderer.flipX ? 0 : -1; // 플레이어가 향하고 있는 방향의 반대 방향으로 설정

        // Animation
        isswap = true;
        float direction = spriteRenderer.flipX ? -1f : 1f; // 플레이어의 방향 확인

        // Rigidbody2D 컴포넌트 참조
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null&&!(accel==0))
        {
            rb.AddForce(new Vector2(-20f * direction, 10f), ForceMode2D.Impulse); // 방향에 따라 힘을 추가
        }

        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);
    }
    public void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        // Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Sprite Flip Y
        spriteRenderer.flipY = true;

        // Collider Disable
        capsulecollider.enabled = false;

        // Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    public void SetSpeed(float newSpeed)
    {
        maxSpeed = newSpeed;
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }

    public void SetJumpPower(float newJumpPower)
    {
        jumpPower = newJumpPower;
    }
    void PlaySound(string action)
    {
        AudioClip clipToPlay = null;
        bool allowOverlap = true; // 기본값: 중첩 허용

        switch (action)
        {
            case "JUMP":
                clipToPlay = audioJump;
                allowOverlap = false;
                break;
            case "ATTACK":
                clipToPlay = audioAttack;
                allowOverlap = false;
                break;
            case "DAMAGED":
                clipToPlay = audioDamaged;
                break;
            case "ITEM":
                clipToPlay = audioItem;
                allowOverlap = false;
                break;
            case "DIE":
                clipToPlay = audioDie;
                break;
            case "FINISH":
                clipToPlay = audioFinish;
                break;
            case "dam":
                clipToPlay = dam;
                allowOverlap = false;
                break;
            case "slide":
                clipToPlay = audioSlide;
                allowOverlap = false;
                break;
            case "secret":
                clipToPlay = audiosecret;
                break;
        }

        if (clipToPlay == null) return;

        // ✅ 중첩 여부에 따라 재생 방식 분기
        if (allowOverlap)
        {
            audioSource.PlayOneShot(clipToPlay, seVolume);
        }
        else
        {
            // 이미 같은 사운드가 재생 중이면 새로 재생 안 함
            if (audioSource.isPlaying && audioSource.clip == clipToPlay) return;

            audioSource.clip = clipToPlay;
            audioSource.volume = seVolume;
            audioSource.Play();
        }
    }

    public void SetSEVolume(float volume)
    {
        seVolume = volume;
        audioSource.volume = seVolume;
        PlayerPrefs.SetFloat("SEVolume", seVolume); // SE 볼륨 저장
    }

}
