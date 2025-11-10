using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpeedBasedOverlapSound : MonoBehaviour
{
    [Header("🎮 플레이어 Rigidbody")]
    public Rigidbody2D playerRigidbody; // 플레이어 속도 가져올 용도

    [Header("🔊 오디오 설정")]
    public AudioClip soundClip;         // 재생할 효과음
    public bool anyKeyMode = true;      // true면 아무 키 누를 때 작동
    public string inputKey = "Fire1";   // anyKeyMode가 false일 때만 사용

    [Header("🎚️ 피치 조절")]
    public float minPitch = 1f;         // 최소 피치
    public float maxPitch = 2f;         // 최대 피치
    public float maxSpeed = 2000f;        // 이 속도에서 피치가 maxPitch에 도달

    [Header("⏱️ 쿨다운 (초당 몇 번까지 중복 허용할지)")]
    public float cooldown = 0.05f;

    private AudioSource audioSource;
    private float lastPlayTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    void Update()
    {
        if (playerRigidbody == null) return;

        bool keyPressed = anyKeyMode ? Input.anyKeyDown : Input.GetButtonDown(inputKey);

        if (keyPressed && Time.time - lastPlayTime > cooldown)
        {
            lastPlayTime = Time.time;

            float speed = playerRigidbody.velocity.magnitude;
            float normalized = Mathf.Clamp01(speed / maxSpeed);
            float pitch = Mathf.Lerp(minPitch, maxPitch, normalized);

            // 🔊 새로운 사운드 재생 (겹쳐서 들림, 기존 재생 안 끊김)
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(soundClip);
        }
    }
}
