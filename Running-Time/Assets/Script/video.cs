using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.isLooping = true; // ���� �ݺ� ��� ����
        videoPlayer.Play();
    }
}
