using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneToLoad; // Inspector에서 씬 이름 지정

    void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogError("⚠️ VideoPlayer가 설정되지 않았습니다. Inspector에서 할당하세요.");
            return;
        }

        videoPlayer.loopPointReached += OnVideoEnd; // 영상이 끝났을 때 이벤트 등록
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // Inspector에서 지정한 씬으로 이동
        }
        else
        {
            Debug.LogError("⚠️ SceneToLoad가 설정되지 않았습니다. Inspector에서 씬 이름을 입력하세요.");
        }
    }
}
