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

        // ����� BGM ���� �ҷ����� (������ �⺻�� 0.8 ���)
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.8f);

        // �����̴� �ʱ�ȭ �� ���� ����
        bgmSlider.value = savedBGMVolume;
        SetBGMVolume(savedBGMVolume);

        // �����̴� �̺�Ʈ ������ ���
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChange);

        // ��� �̺�Ʈ ������ ���
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);

        // ����� ��üȭ�� ���� �ҷ�����
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
        PlayerPrefs.SetFloat("BGMVolume", volume);  // BGM ���� ����
        SetBGMVolume(volume);                       // ��� BGM ���� ����
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

    // BGM ���� ���� �޼���
    private void SetBGMVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;  // AudioSource ���� ����
        }
        else
        {
            Debug.LogWarning("BGM AudioSource�� ������� �ʾҽ��ϴ�.");
        }

        if (gameManager != null)
        {
            gameManager.SetBGMVolume(volume);  // GameManager���� ���� ����
        }
    }
}
