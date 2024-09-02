using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Color startColor;
    private float lifeTime = 0.5f; // 잔상의 생명 시간

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Initialize(Sprite sprite, Vector3 position, bool flipX, Color color, Animator playerAnimator)
    {
        spriteRenderer.sprite = sprite;
        transform.position = position;
        spriteRenderer.flipX = flipX;
        startColor = color;
        spriteRenderer.color = startColor;

        // 애니메이터 상태 복사
        if (animator != null && playerAnimator != null)
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            animator.Play(stateInfo.fullPathHash, 0, stateInfo.normalizedTime);
        }

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < lifeTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / lifeTime);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
