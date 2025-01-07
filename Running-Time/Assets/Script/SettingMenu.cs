using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video; // VideoPlayer 사용

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider seSlider;  // SE 볼륨 슬라이더
    public Toggle fullscreenToggle;
    public Text versionText;
    public Text playtimeText;
    public GameObject settingsPanel;  // 설정 패널 참조

    public List<AudioSource> bgmSources;  // 여러 BGM AudioSource 리스트
    public List<AudioSource> seSources;   // 여러 SE AudioSource 리스트
    public List<VideoPlayer> videoPlayers; // 여러 VideoPlayer 리스트

    private float playtime = 0f;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // 저장된 BGM 및 SE 볼륨 불러오기
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        seSlider.value = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        // 초기 BGM 및 SE 볼륨 설정
        SetBGMVolume(bgmSlider.value);
        SetSEVolume(seSlider.value);

        // 슬라이더 이벤트 리스너 등록
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChange);
        seSlider.onValueChanged.AddListener(OnSEVolumeChange);
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);

        // 저장된 전체화면 설정 불러오기
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        if (!isFullscreen)
        {
            Screen.SetResolution(1600, 900, false); // 창 모드 해상도 설정
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
        PlayerPrefs.SetFloat("BGMVolume", volume);  // 즉시 저장
    }

    public void OnSEVolumeChange(float volume)
    {
        SetSEVolume(volume);
        PlayerPrefs.SetFloat("SEVolume", volume);  // 즉시 저장
    }

    private void SetBGMVolume(float volume)
    {
        // BGM AudioSource 볼륨 설정
        foreach (AudioSource bgm in bgmSources)
        {
            if (bgm != null)
            {
                bgm.volume = volume;
            }
        }

        // VideoPlayer 오디오 볼륨 설정
        foreach (VideoPlayer video in videoPlayers)
        {
            if (video != null)
            {
                if (video.audioOutputMode == VideoAudioOutputMode.AudioSource)
                {
                    // AudioSource를 사용하는 경우
                    foreach (AudioSource audioSource in video.GetComponents<AudioSource>())
                    {
                        audioSource.volume = volume;
                    }
                }
                else if (video.audioOutputMode == VideoAudioOutputMode.Direct)
                {
                    // Direct 모드에서 각 오디오 트랙의 볼륨 설정
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
            // 전체화면 모드
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        {
            // 창 모드 해상도 1600x900 설정
            Screen.SetResolution(1600, 900, false);
        }

        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);  // 즉시 저장
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

    // 계속하기 버튼 눌렀을 때 호출되는 메서드
    public void OnResumeButton()
    {
        settingsPanel.SetActive(false);  // 설정 패널 비활성화
        Time.timeScale = 1;  // 게임 다시 진행
    }

    // 종료 버튼 눌렀을 때 호출되는 메서드
    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 에디터에서 플레이 모드 종료
#else
        Application.Quit();  // 실제 빌드된 애플리케이션 종료
#endif
    }
}
