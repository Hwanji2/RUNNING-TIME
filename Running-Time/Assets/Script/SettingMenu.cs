using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;
    public GameObject settingsPanel;

    public List<AudioSource> bgmSources = new List<AudioSource>(); // 기본값 할당
    public List<AudioSource> seSources = new List<AudioSource>();

    private GameManager gameManager;
    private PlayerMove playerMove;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerMove = FindObjectOfType<PlayerMove>();

        Debug.Log($"gameManager: {gameManager}");
        Debug.Log($"playerMove: {playerMove}");

        if (bgmSlider == null) Debug.LogError("⚠️ bgmSlider가 할당되지 않았습니다.");
        if (seSlider == null) Debug.LogError("⚠️ seSlider가 할당되지 않았습니다.");
        if (settingsPanel == null) Debug.LogError("⚠️ settingsPanel이 할당되지 않았습니다.");

        // PlayerPrefs에서 불러온 볼륨 값 가져오기
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        float seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // 슬라이더가 할당되어 있을 때만 값 설정
        if (bgmSlider != null) bgmSlider.value = bgmVolume;
        if (seSlider != null) seSlider.value = seVolume;

        // 볼륨 설정 (BGM / SE)
        SetBGMVolume(bgmVolume);
        SetSEVolume(seVolume);

        // 슬라이더 이벤트 등록 (null 체크 추가)
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(OnBGMVolumeChange);
        }
        else
        {
            Debug.LogError("⚠️ bgmSlider가 설정되지 않아 이벤트 리스너를 추가할 수 없습니다.");
        }

        if (seSlider != null)
        {
            seSlider.onValueChanged.AddListener(OnSEVolumeChange);
        }
        else
        {
            Debug.LogError("⚠️ seSlider가 설정되지 않아 이벤트 리스너를 추가할 수 없습니다.");
        }
    }

    public void OnBGMVolumeChange(float volume)
    {
        SetBGMVolume(volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void OnSEVolumeChange(float volume)
    {
        SetSEVolume(volume);

        if (playerMove != null)
        {
            playerMove.SetSEVolume(volume);
        }
        else
        {
            Debug.LogWarning("⚠️ PlayerMove가 할당되지 않아 SE 볼륨을 적용할 수 없습니다.");
        }

        PlayerPrefs.SetFloat("SEVolume", volume);
    }

    private void SetBGMVolume(float volume)
    {
        if (bgmSources == null || bgmSources.Count == 0)
        {
            Debug.LogWarning("⚠️ bgmSources가 비어 있습니다. BGM 볼륨을 적용할 수 없습니다.");
            return;
        }

        foreach (AudioSource bgm in bgmSources)
        {
            if (bgm != null)
            {
                bgm.volume = volume;
            }
        }
    }

    private void SetSEVolume(float volume)
    {
        if (seSources == null || seSources.Count == 0)
        {
            Debug.LogWarning("⚠️ seSources가 비어 있습니다. SE 볼륨을 적용할 수 없습니다.");
            return;
        }

        foreach (AudioSource se in seSources)
        {
            if (se != null)
            {
                se.volume = volume;
            }
        }
    }

    public void OnResumeButton()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
