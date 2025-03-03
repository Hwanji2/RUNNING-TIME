using UnityEngine;
using UnityEngine.Video;

public class VideoController2 : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("⚠️ VideoPlayer가 설정되지 않았습니다. Inspector에서 할당하세요.");
            return;
        }

        videoPlayer.loopPointReached += RestartVideo; // 비디오가 끝나면 RestartVideo 실행
        videoPlayer.Play(); // 비디오 시작
    }

    void RestartVideo(VideoPlayer vp)
    {
        vp.Stop(); // 비디오 정지
        vp.Play(); // 다시 처음부터 재생
    }
}
