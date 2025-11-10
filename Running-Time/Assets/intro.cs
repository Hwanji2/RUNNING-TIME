using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;  // DateTime, Environment
using UnityEngine.UI;

public class MultiIntroText : MonoBehaviour
{
    [Header("📝 출력할 TMP 텍스트")]
    public TMP_Text displayText;

    [Header("💬 표시할 문장 리스트 (순서대로)")]
    [TextArea(3, 10)]
    public List<string> sentences = new List<string>();

    [Header("⌛ 타이핑 속도 (초당 글자 간격)")]
    public float typingSpeed = 0.05f;

    [Header("🔊 타이핑 사운드 (랜덤 재생)")]
    public List<AudioClip> typingClips = new List<AudioClip>();
    public AudioSource audioSource;
    [Range(0f, 1f)] public float typingVolume = 0.5f;

    [Header("🖼️ 타이핑 중 교체할 이미지")]
    public Image targetImage;                   // 바꿀 이미지 오브젝트
    public List<Sprite> spriteList = new List<Sprite>(); // 순환할 스프라이트들
    private int spriteIndex = 0;

    [Header("🎯 모든 문장 후 이동할 씬 이름")]
    public string nextSceneName;

    [Header("⚙ 최초 실행 감지 키 (PlayerPrefs)")]
    public string firstRunKey = "HasRunIntro";

    [Header("💤 $ 태그 대기 시간 (초)")]
    public float pauseDuration = 0.7f;

    private int currentIndex = 0;
    private bool isTyping = false;
    private bool isWaitingForKey = false;

    void Start()
    {
        // 오디오소스 자동 할당
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        bool isFirstRun = !PlayerPrefs.HasKey(firstRunKey);

        if (isFirstRun)
        {
            PlayerPrefs.SetInt(firstRunKey, 1);
            PlayerPrefs.Save();
            StartCoroutine(TypeSentence(ProcessTags(sentences[currentIndex])));
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    void Update()
    {
        if (isWaitingForKey && Input.anyKeyDown)
        {
            isWaitingForKey = false;
            currentIndex++;

            if (currentIndex < sentences.Count)
                StartCoroutine(TypeSentence(ProcessTags(sentences[currentIndex])));
            else
                SceneManager.LoadScene(nextSceneName);
        }
    }

    // 🔹 문장 내 특수 태그 처리 함수
    private string ProcessTags(string original)
    {
        string processed = original;

        // (Y) → 현재년도 - 2024
        if (processed.Contains("(Y)"))
        {
            int diff = DateTime.Now.Year - 2024;
            processed = processed.Replace("(Y)", diff.ToString());
        }

        // (Name) → PC 사용자 이름
        if (processed.Contains("(Name)"))
        {
            string user = Environment.UserName;
            processed = processed.Replace("(Name)", user);
        }

        return processed;
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        displayText.text = "";

        foreach (char c in sentence)
        {
            if (c == '$')
            {
                yield return new WaitForSeconds(pauseDuration);
                continue;
            }

            displayText.text += c;

            // 🔊 타이핑 사운드 랜덤 재생
            if (typingClips.Count > 0)
            {
                AudioClip clip = typingClips[UnityEngine.Random.Range(0, typingClips.Count)];
                if (clip != null && audioSource != null)
                    audioSource.PlayOneShot(clip, typingVolume);
            }

            // 🖼️ 스프라이트 순환
            if (targetImage != null && spriteList.Count > 0)
            {
                targetImage.sprite = spriteList[spriteIndex];
                spriteIndex = (spriteIndex + 1) % spriteList.Count;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        isWaitingForKey = true;
    }
}
