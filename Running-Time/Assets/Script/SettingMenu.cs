using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Toggle fullscreenToggle;
    public Text versionText;
    public Text playtimeText;
    public AudioSource bgmSource;  // BGM AudioSource

    private float playtime = 0f;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // 저장된 BGM 볼륨 불러오기 (없으면 기본값 0.8 사용)
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.8f);

        // 슬라이더 초기화 및 볼륨 설정
        bgmSlider.value = savedBGMVolume;
        SetBGMVolume(savedBGMVolume);

        // 슬라이더 이벤트 리스너 등록
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChange);

        // 토글 이벤트 리스너 등록
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);

        // 저장된 전체화면 설정 불러오기
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
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
        PlayerPrefs.SetFloat("BGMVolume", volume);  // BGM 볼륨 저장
        SetBGMVolume(volume);                       // 즉시 BGM 볼륨 적용
    }

    public void OnFullscreenToggle(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    private void UpdatePlaytimeText()
    {
        int minutes = Mathf.FloorToInt(playtime / 60);
        int seconds = Mathf.FloorToInt(playtime % 60);
        playtimeText.text = $"Playtime: {minutes}m {seconds}s";
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("Playtime", playtime);
    }

    // BGM 볼륨 설정 메서드
    private void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;  // AudioSource 볼륨 설정
        }
        else
        {
            Debug.LogWarning("BGM AudioSource가 연결되지 않았습니다.");
        }

        if (gameManager != null)
        {
            gameManager.SetBGMVolume(volume);  // GameManager에도 볼륨 적용
        }
    }
}
