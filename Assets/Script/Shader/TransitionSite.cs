using System.Collections;
using UnityEngine;

public class TransitionSite : MonoBehaviour
{
    public TransitionShaderController transitionController; // 트랜지션 컨트롤러
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        transitionController = FindObjectOfType<TransitionShaderController>();
    }

    public void ChangeStageWithTransition()
    {
        if (transitionController != null && gameManager != null)
        {
            StartCoroutine(TransitionAndChangeStage());
        }
        else
        {
            gameManager?.NextStage();
        }
    }

    private IEnumerator TransitionAndChangeStage()
    {

        yield return new WaitForSeconds(transitionController.transitionDuration); // 트랜지션 지속 시간 대기
        // 트랜지션이 끝날 때까지 대기
        yield return new WaitForSeconds(transitionController.transitionDuration);

        // 🔻 기존 GameManager의 NextStage 실행 (씬 변경)
        gameManager.NextStage();

        // 🔻 약간의 대기 후 페이드 아웃 (화면 열림)
        yield return new WaitForSeconds(0.5f); // 씬 로딩이 자연스럽게 되도록 약간의 대기
        transitionController.StartFadeIn(); // 화면 닫힘 (페이드 인)

    }
}
