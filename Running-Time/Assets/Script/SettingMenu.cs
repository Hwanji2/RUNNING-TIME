using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video; // VideoPlayer ���

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;  // SE ���� �����̴�
    public Toggle fullscreenToggle;
    public Text versionText;
    public Text playtimeText;
    public GameObject settingsPanel;  // ���� �г� ����

    public List<AudioSource> bgmSources;  // ���� BGM AudioSource ����Ʈ
    public List<AudioSource> seSources;   // ���� SE AudioSource ����Ʈ
    public List<VideoPlayer> videoPlayers; // ���� VideoPlayer ����Ʈ

    private float playtime = 0f;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // ����� BGM �� SE ���� �ҷ�����
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // �ʱ� BGM �� SE ���� ����
        SetBGMVolume(bgmSlider.value);
        SetSEVolume(seSlider.value);

        // �����̴� �̺�Ʈ ������ ���
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChange);
        seSlider.onValueChanged.AddListener(OnSEVolumeChange);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);

        // ����� ��üȭ�� ���� �ҷ�����
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        if (!isFullscreen)
        {
            Screen.SetResolution(1600, 900, false); // â ��� �ػ� ����
        }

        versionText.text = "Jihwan Lee";
        playtime = PlayerPrefs.GetFloat("Playtime", 0f);
        UpdatePlaytimeText();
    }

    void Update()
    {
        playtime += Time.unscaledDeltaTime;
        UpdatePlaytimeText();
    }

    public void OnBGMVolumeChange(float volume)
    {
        SetBGMVolume(volume);
        PlayerPrefs.SetFloat("BGMVolume", volume);  // ��� ����
    }

    public void OnSEVolumeChange(float volume)
    {
        SetSEVolume(volume);
        PlayerPrefs.SetFloat("SEVolume", volume);  // ��� ����
    }

    private void SetBGMVolume(float volume)
    {
        // BGM AudioSource ���� ����
        foreach (AudioSource bgm in bgmSources)
        {
            if (bgm != null)
            {
                bgm.volume = volume;
            }
        }

        // VideoPlayer ����� ���� ����
        foreach (VideoPlayer video in videoPlayers)
        {
            if (video != null)
            {
                if (video.audioOutputMode == VideoAudioOutputMode.AudioSource)
                {
                    // AudioSource�� ����ϴ� ���
                    foreach (AudioSource audioSource in video.GetComponents<AudioSource>())
                    {
                        audioSource.volume = volume;
                    }
                }
                else if (video.audioOutputMode == VideoAudioOutputMode.Direct)
                {
                    // Direct ��忡�� �� ����� Ʈ���� ���� ����
                    for (ushort track = 0; track < video.audioTrackCount; track++)
                    {
                        video.SetDirectAudioVolume(track, volume);
                    }
                }
            }
        }
    }

    private void SetSEVolume(float volume)
    {
        foreach (AudioSource se in seSources)
        {
            if (se != null)
            {
                se.volume = volume;
            }
        }
    }

    public void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if (isFullscreen)
        {
            // ��üȭ�� ���
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        {
            // â ��� �ػ� 1600x900 ����
            Screen.SetResolution(1600, 900, false);
        }

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);  // ��� ����
    }

    private void UpdatePlaytimeText()
    {
        int minutes = Mathf.FloorToInt(playtime / 60);
        int seconds = Mathf.FloorToInt(playtime % 60);
        playtimeText.text = $"Playtime  {minutes}m {seconds}s";
    }

    public void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Playtime", playtime);
    }

    // ����ϱ� ��ư ������ �� ȣ��Ǵ� �޼���
    public void OnResumeButton()
    {
        settingsPanel.SetActive(false);  // ���� �г� ��Ȱ��ȭ
        Time.timeScale = 1;  // ���� �ٽ� ����
    }

    // ���� ��ư ������ �� ȣ��Ǵ� �޼���
    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // �����Ϳ��� �÷��� ��� ����
#else
        Application.Quit();  // ���� ����� ���ø����̼� ����
#endif
    }
}
