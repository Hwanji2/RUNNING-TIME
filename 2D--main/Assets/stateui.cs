

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombinedUI : MonoBehaviour
{
    public PlayerMove playerMove;
    public ElectricWater electronic;
    public Animator uiAnimator;
    public RectTransform uiRectTransform;
    public float shakeAmount = 5f;
    public float shakeDuration = 0.1f;
    public bool drink = false;
    public bool walk = false;
    public bool fall = false;
    public bool anxiety = false;
    public AudioClip drinkSound; // 음료수 마시는 소리

    private GameManager gameManager;
    public ROBOTMOVE robotMove;
    private Vector3 originalPosition;
    private float shakeTimer;
    private float previousYPosition;
    private bool isAnxious = false;
    private bool run = false;
    private AudioSource audioSource; // 오디오 소스

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerMove = FindObjectOfType<PlayerMove>();

        originalPosition = uiRectTransform.localPosition;
        previousYPosition = playerMove.transform.position.y;
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    void Update()
    {
        UpdateAccelUI();
        CheckAnxiety();
        CheckFalling();
        HandleShake();

        // 로봇 연동 상태에 따른 애니메이션 변경
        uiAnimator.SetBool("isLinked", robotMove.isLinked);



        if (playerMove.itemEat)
        {
            StartCoroutine(ShowItemAcquiredAnimation());
        }
        if (electronic.gamjeon)
        {
            StartCoroutine(ShowGam());
        }
        if (playerMove.isDon)
        {
            StartCoroutine(ShowDonAnimation());
        }

        // accel이 30 이상일 때 애니메이션 변경
        if (playerMove.accel >= 30 && !drink && !walk && !anxiety && !fall && !run)
        {
            StartCoroutine(HandleHighAccel());
        }
    }

    private IEnumerator HandleHighAccel()
    {
        run = true;
        uiAnimator.SetBool("isHighAccel", true); // accel이 30 이상이면 애니메이션 변경
        shakeTimer = shakeDuration; // 흔들림 효과 지속 시간 설정

        while (shakeTimer > 0)
        {
            ShakeUI();
            shakeTimer -= Time.deltaTime;
            yield return null;
        }

        uiRectTransform.localPosition = originalPosition;
        uiAnimator.SetBool("isHighAccel", false);
        run = false;
    }

    void UpdateAccelUI()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && gameManager.coffeeCount > 0)
        {
            UseCoffee();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && gameManager.milkCount > 0)
        {
            UseMilk();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && gameManager.unknownItemCount > 0)
        {
            UseUnknownItem();
        }
        if (playerMove.accel >= 30)
        {
            uiAnimator.SetBool("isHighAccel", true); // accel이 30 이상이면 애니메이션 변경
            shakeTimer = shakeDuration; // 흔들림 효과 지속 시간 설정
        }
        else
        {
            uiAnimator.SetBool("isHighAccel", false); // 그렇지 않으면 기본 애니메이션 유지
            shakeTimer = 0; // 흔들림 효과 중지
            uiRectTransform.localPosition = originalPosition;
            uiAnimator.SetBool("isHighAccel", false); // 그렇지 않으면 기본 애니메이션 유지
        }
        // 키 입력에 따른 애니메이션 변경
        if (Input.anyKeyDown)
        {
            uiAnimator.SetBool("isUnknown", false);
            uiAnimator.SetBool("isWalking", true);
            walk = true;
        }
        else
        {
            uiAnimator.SetBool("isWalking", false);
            walk = false;
        }
    }

    void HandleShake()
    {
        if (shakeTimer > 0)
        {
            ShakeUI();
            shakeTimer -= Time.deltaTime;
        }
        else if (!drink)
        {
            uiAnimator.SetBool("isHighAccel", false); // 그렇지 않으면 기본 애니메이션 유지
            uiRectTransform.localPosition = originalPosition;
        }
    }

    void ShakeUI()
    {
        Vector3 shakePosition = originalPosition + Random.insideUnitSphere * shakeAmount;
        uiRectTransform.localPosition = shakePosition;
    }

    public void UseCoffee()
    {
        if (gameManager.coffeeCount > 0)
        {
            gameManager.coffeeCount--;
            float originalAccel = playerMove.accel;
            Rigidbody2D rb = playerMove.GetComponent<Rigidbody2D>();
            uiAnimator.SetBool("isCoffe", true);
            PlayDrinkSound(); // 음료수 마시는 소리 재생
            if (rb != null)
            {
                float direction = playerMove.spriteRenderer.flipX ? -1f : 1f; // 플레이어의 방향 확인
                rb.AddForce(new Vector2(20f * direction, 10f), ForceMode2D.Impulse); // 방향에 따라 힘을 추가

                Debug.Log("Used Coffee. Rigidbody x velocity increased.");
                playerMove.SetSpeed(playerMove.accel + 20f); // 속도 증가
                playerMove.accel += 30;
                Debug.Log("Used Coffee. Speed increased.");
            }
            drink = true;
            StartCoroutine(ResetAccelAfterTime(originalAccel, 5f)); // 5초 후 원래 속도로 되돌림
        }
        else
        {
            Debug.Log("No coffee left.");
        }
    }

    private IEnumerator ResetAccelAfterTime(float originalAccel, float duration)
    {
        yield return new WaitForSeconds(duration);
        playerMove.accel = originalAccel;
        Debug.Log("Speed reset to original value.");
        uiAnimator.SetBool("isCoffe", false);
    }

    public void UseMilk()
    {
        if (gameManager.milkCount > 0)
        {
            Color originalColor = playerMove.GetComponent<Renderer>().material.color;
            uiAnimator.SetBool("isMilk", true);
            PlayDrinkSound(); // 음료수 마시는 소리 재생
            playerMove.gameObject.layer = 11;
            gameManager.milkCount--;
            playerMove.SetInvincible(true); // 무적 상태
            Debug.Log("Used Milk. Player is now invincible.");
            StartCoroutine(RemoveInvincibility());
            drink = true;
        }
        else
        {
            Debug.Log("No milk left.");
        }
    }

    public void UseUnknownItem()
    {
        if (gameManager.unknownItemCount > 0)
        {
            uiAnimator.SetBool("isUnknown", true);
            PlayDrinkSound(); // 음료수 마시는 소리 재생
            gameManager.unknownItemCount--;
            playerMove.SetJumpPower(playerMove.jumpPower + 100f); // 점프력 증가
            Debug.Log("Used Unknown Item. Jump power increased.");
            drink = true;
       

        }
        else
        {
            Debug.Log("No unknown items left.");
        }
    }

    private void PlayDrinkSound()
    {
        if (audioSource != null && drinkSound != null)
        {
            audioSource.PlayOneShot(drinkSound);
        }
    }


    private void CheckAnxiety()
    {
        if (gameManager.timer < 30000 && !Input.anyKey && playerMove.accel < 30)
        {
            anxiety = true;
            isAnxious = true;
            uiAnimator.SetBool("isAnxious", true);
        }
        else
        {
            isAnxious = false;
            anxiety = false;
            uiAnimator.SetBool("isAnxious", false);
            Debug.Log("No check anxious.");

        }
    }

    private void CheckFalling()
    {
        float currentYPosition = playerMove.transform.position.y;
        if(playerMove.isFalling)
        {
            fall = true;
            StartCoroutine(ShowFallingAnimation());
        }
        else
        {
            fall = false;
            Debug.Log("No check fall.");

        }
        previousYPosition = currentYPosition;
    }

    private IEnumerator ShowFallingAnimation()
    {
        fall = true;
        uiAnimator.SetBool("isFalling", true);
        yield return new WaitForSeconds(1f);
        uiAnimator.SetBool("isFalling", false);
    }

    private IEnumerator ShowGam()
    {
        uiAnimator.SetBool("isWak", true);
        yield return new WaitForSeconds(2f);
        uiAnimator.SetBool("isWak", false);
    }

    private IEnumerator ShowItemAcquiredAnimation()
    {
        uiAnimator.SetBool("isItemAcquired", true);
        yield return new WaitForSeconds(1f);
        uiAnimator.SetBool("isItemAcquired", false);
    }
    private IEnumerator ShowDonAnimation()
    {
        uiAnimator.SetBool("isGood", true);
        yield return new WaitForSeconds(1f);
        uiAnimator.SetBool("isGood", false);
    }

    private IEnumerator RemoveInvincibility()
    {
       

        yield return new WaitForSeconds(5f); // 5초 후 무적 상태 해제
        playerMove.SetInvincible(false);

    Debug.Log("Invincibility removed.");
        uiAnimator.SetBool("isMilk", false);
        playerMove.gameObject.layer = 10;


    }
}
