using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI; // UI 슬라이더 사용을 위해 추가

public class VideoController3 : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 비디오 플레이어
    public Slider volumeSlider; // 인스펙터에서 지정할 볼륨 슬라이더

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("⚠️ VideoPlayer가 설정되지 않았습니다. Inspector에서 할당하세요.");
            return;
        }

        if (volumeSlider == null)
        {
            Debug.LogError("⚠️ VolumeSlider가 설정되지 않았습니다. Inspector에서 할당하세요.");
            return;
        }

        // 비디오에 오디오 트랙이 있는지 확인 후 볼륨 설정
        if (videoPlayer.audioTrackCount > 0)
        {
            volumeSlider.value = (float)videoPlayer.GetDirectAudioVolume(0);
        }
        else
        {
            Debug.LogWarning("⚠️ 비디오에 오디오 트랙이 없습니다. 볼륨 조정이 불가능합니다.");
            volumeSlider.interactable = false; // 슬라이더 비활성화
        }

        // 슬라이더 값이 변경될 때마다 OnVolumeChange 실행
        volumeSlider.onValueChanged.AddListener(OnVolumeChange);

        videoPlayer.loopPointReached += RestartVideo; // 비디오가 끝나면 다시 시작
        videoPlayer.Play(); // 비디오 시작
    }

    void OnVolumeChange(float volume)
    {
        // 비디오에 오디오 트랙이 있을 때만 볼륨 조정
        if (videoPlayer.audioTrackCount > 0)
        {
            videoPlayer.SetDirectAudioVolume(0, Mathf.Clamp01(volume)); // 0~1 범위 내에서 볼륨 설정
        }
    }

    void RestartVideo(VideoPlayer vp)
    {
        vp.Stop(); // 비디오 정지
        vp.Play(); // 다시 처음부터 재생
    }
}
