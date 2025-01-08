using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalPoint = 0;
    public int stagePoint = 0;
    public int moneyPoint = 0;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public StageTransition tutow;
    public GameObject[] Stages;
    private bool isPulsing;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public Text UITimer;
    public GameObject RestartBtn;

    public float timer = 180000f; // 3분(180,000밀리초)
    private bool isGameOver = false;

    public int coffeeCount = 0;
    public int milkCount = 0;
    public int unknownItemCount = 0;
    public int a = 0;
    private AudioSource bgMusic;
    private float pausedBGMTime;

    private void Start()
    {
        SetTimerBasedOnScene();
        totalPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        moneyPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);

       
        bgMusic = GetComponent<AudioSource>();

        // 저장된 BGM 볼륨 불러오기 (없으면 기본값 0.8 사용)
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.8f);
        bgMusic.volume = savedBGMVolume;
        Invoke("PlayMusic", 7f);
    }

    void SetTimerBasedOnScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "Main":
                timer = 180000f; // 3분
                break;
            case "HIGH":
                timer = 540000f; // 9분
                break;
            case "Main2":
                timer = 90000f;
                break;
            case "Main 1":
                timer = 0f; // 9분
                break;
            default:
                timer = 180000f; // 기본값 3분
                break;
        }
    }


    private void PlayMusic()
    {
        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.Play();
        }
    }

    public void ResumeBGM()
    {
        // 현재 스테이지가 3이 아닌 경우에만 BGM 재생
        if (stageIndex != 2 && !bgMusic.isPlaying)
        {
            bgMusic.time = pausedBGMTime;
            bgMusic.Play();
        }
    }


    void Update()
    {
        if (((timer>10)&& (timer <= 5000) )||(!isGameOver&&bgMusic.isPlaying))
        {

            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "Main 1")
            {
                timer += Time.deltaTime * 1000; // 밀리초 단위로 증가
            }
            else
            {
                timer -= Time.deltaTime * 1000; // 밀리초 단위로 감소
            }

            if (timer <= 10 && sceneName != "Main 1")
            {
                timer = 0;
                UpdateTimerUI();
                GameOver();
            }

            UpdateTimerUI();

            moneyPoint = totalPoint + stagePoint;
            UIPoint.text = moneyPoint.ToString();
        }
    }

    void UpdateTimerUI()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        int minutes = Mathf.FloorToInt(timer / 60000);
        int seconds = Mathf.FloorToInt((timer % 60000) / 1000);
        int milliseconds = Mathf.FloorToInt(timer % 1000);
        UITimer.text = string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        if (timer <= 5000&&(sceneName == "Main"))
        {
            UITimer.color = Color.red;
        }
        else
        {
            UITimer.color = Color.white;
        }
    }
    public void PauseBGM()
    {
        if (bgMusic.isPlaying)
        {
            pausedBGMTime = bgMusic.time;
            bgMusic.Pause();
        }
    }
    public void NextStage()
    {
        Debug.Log("player.tuto: " + player.tuto);
        Debug.Log("player.tuto2: " + player.tuto2);

        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            if (player.tuto == true)
            {
                Debug.Log("Entering player.tuto == true block");
                stageIndex++;
                stageIndex++;
                if (bgMusic.isPlaying)
                {
                    pausedBGMTime = bgMusic.time;
                    bgMusic.Pause();
                }

                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                player.tuto = false;
                UIStage.text = "체육관";
            }
            else if (player.tuto2 == true)
            {
                Debug.Log("Entering player.tuto2 == true block");
                stageIndex--;
                stageIndex--;

                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                ResumeBGM();
                player.tuto2 = false;
                UIStage.text = "다시 교외";
            }
            else
            {
                Debug.Log("Entering default block");
                stageIndex++;
                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                UIStage.text = "공학관";
            }
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("게임 클리어!");
        }

        totalPoint += stagePoint;
        moneyPoint = totalPoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            player.OnDie();
            Debug.Log("죽었습니다!");
            RestartBtn.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (health > 1)
            {
                PlayerReposition();
            }

            HealthDown();
        }
    }

    void PlayerReposition()
    {
        if (player.tuto == true)
        {
            player.transform.position = new Vector3(-1, 0, -1);
        }
        else if (player.tuto2 == true)
        {
            player.transform.position = new Vector3(180, 1, -1);
        }
        else
        {
            player.transform.position = new Vector3(0, 0, -1);
        }
        player.VelocityZero();
    }

    private void GameOver()
    {
        Debug.Log("게임 오버!");

        bgMusic.Stop();

        // 이번 판에 얻은 돈과 총합 돈을 각각 저장
        PlayerPrefs.SetInt("StagePoint", stagePoint);  // 이번 판에 얻은 돈
        PlayerPrefs.SetInt("TotalPoint", totalPoint);  // 총합 돈

        PlayerPrefs.SetInt("MoneyPoint", moneyPoint);
        PlayerPrefs.SetInt("CoffeeCount", coffeeCount);
        PlayerPrefs.SetInt("MilkCount", milkCount);
        PlayerPrefs.SetInt("UnknownItemCount", unknownItemCount);
        PlayerPrefs.SetFloat("RemainingTime", timer);
        PlayerPrefs.SetInt("SecretCount", player.secretCount);

        SceneManager.LoadScene("ENDING");
        Time.timeScale = 1;
        a = player.secretCount;
    }

    public void Restart()
    {
        Time.timeScale = 1;

        // 이전에 저장된 총합 돈 불러오기
        totalPoint = PlayerPrefs.GetInt("TotalPoint", 0);

        // 이번 판에 얻은 돈 초기화
        stagePoint = 0;

        SceneManager.LoadScene(0);
    }

    public void SetBGMVolume(float volume)
    {
        if (bgMusic != null)
        {
            bgMusic.volume = volume;
        }
    }


    // 금액 차감 함수 추가
    public bool DeductMoney(int amount)
    {
        if (moneyPoint >= amount)
        {
            totalPoint -= amount;
     
            return true;
        }
        else
        {
            Debug.Log("Not enough money.");
            return false;
        }
    }
}
