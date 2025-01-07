using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.isLooping = true; // 무한 반복 재생 설정
        videoPlayer.Play();
    }
}
