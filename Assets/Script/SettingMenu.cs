using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;
    public GameObject settingsPanel;

    public List<AudioSource> bgmSources = new List<AudioSource>();
    public List<AudioSource> seSources = new List<AudioSource>();

    private GameManager gameManager;
    private PlayerMove playerMove;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerMove = FindObjectOfType<PlayerMove>();

        // 올바른 null 체크 방법 적용
        Debug.Log($"gameManager: {gameManager}");
        Debug.Log($"playerMove: {(playerMove != null ? playerMove.ToString() : "PlayerMove 없음")}");

        if (bgmSlider == null) Debug.LogError("⚠️ bgmSlider가 할당되지 않았습니다.");
        if (seSlider == null) Debug.LogError("⚠️ seSlider가 할당되지 않았습니다.");
        if (settingsPanel == null) Debug.LogError("⚠️ settingsPanel이 할당되지 않았습니다.");

        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        float seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        if (bgmSlider != null) bgmSlider.value = bgmVolume;
        if (seSlider != null) seSlider.value = seVolume;

        SetBGMVolume(bgmVolume);
        SetSEVolume(seVolume);

        if (bgmSlider != null) bgmSlider.onValueChanged.AddListener(OnBGMVolumeChange);
        if (seSlider != null) seSlider.onValueChanged.AddListener(OnSEVolumeChange);
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
            Debug.LogWarning("⚠️ PlayerMove가 없으므로 SE 볼륨 적용을 생략합니다.");
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
