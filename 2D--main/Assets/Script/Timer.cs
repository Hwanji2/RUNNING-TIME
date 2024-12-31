using UnityEngine;

public class Timer : MonoBehaviour
{
    private float currentTime;
    private bool isPaused;

    public void SetCurrentTime(float time)
    {
        currentTime = time;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }

    void Update()
    {
        if (!isPaused)
        {
            currentTime -= Time.deltaTime * 1000; // �и��� ������ ����
            if (currentTime < 0)
            {
                currentTime = 0;
            }
        }
    }
}
