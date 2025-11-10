using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagers : MonoBehaviour
{

    public int totalPoint=0;
    public int stagePoint=0;
    public int moneyPoint = 0;
    public int stageIndex;
    public int health;
    public PlayerMove player;
    public StageTransition tuto;
    public GameObject[] Stages;
    private bool isPulsing; // isPulsing 변수 선언


    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public Text UITimer; // 타이머 UI 텍스트 추가
    public GameObject RestartBtn;

    public float timer = 180000f; // 3분(180,000밀리초)
    private bool isGameOver = false;

    // 아이템 갯수 변수 추가
    public int coffeeCount = 0;
    public int milkCount = 0;
    public int unknownItemCount = 0;
    public int a = 0;
    // AudioSource for background music
    private AudioSource bgMusic;
    private float pausedBGMTime; // BGM 멈춘 시간

    private void Start()
    {
        // 데이터 불러오기
        totalPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        moneyPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);

        // Get the AudioSource component
        bgMusic = GetComponent<AudioSource>();

        // Set the volume (0.0 to 1.0)
        bgMusic.volume = 0.8f; // Adjust the volume level as needed

        // 3초 후에 음악 재생
        Invoke("PlayMusic", 7f);
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
        if (!bgMusic.isPlaying)
        {
            bgMusic.time = pausedBGMTime; // 저장된 시간으로 설정
            bgMusic.Play();
        }
    }
    void Update()
    {
        if (!isGameOver)
        {
            // 타이머 업데이트
            if (bgMusic.isPlaying ||(timer<=10000))
            {
              timer -= Time.deltaTime * 1000; // 밀리초 단위로 감소
            }
            else
            {

            }
            
            if (timer <= 0)
            {
                timer = 0;
                GameOver();
            }

            // 타이머 UI 업데이트
            UpdateTimerUI();

            // 포인트 UI 업데이트
            moneyPoint = totalPoint + stagePoint;
            UIPoint.text = moneyPoint.ToString();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60000);
        int seconds = Mathf.FloorToInt((timer % 60000) / 1000);
        int milliseconds = Mathf.FloorToInt(timer % 1000);
        UITimer.text = string.Format("{0:0}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        if (timer <= 5000) 
        {
            UITimer.color = Color.red;
        }
        else
        {
            UITimer.color = Color.white;
        }
    }



    public void NextStage()
    {
        //Change Stage
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            if (player.tuto == true)
            {
                stageIndex++;
                stageIndex++;
                if (bgMusic.isPlaying)
                {
                    pausedBGMTime = bgMusic.time; // 현재 재생 시간 저장
                    bgMusic.Pause();
                }

                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                player.tuto = false;
                UIStage.text = "체육관";
            }
            else if(player.tuto2 == true)
            {
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
                stageIndex++;
                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                UIStage.text = "공학관";
            }
            

       
        }
        else    // Game Clear
        {
            // Player Control Lock
            Time.timeScale = 0;

            // Result UI
            Debug.Log("게임 클리어!");

            // Restart Button UI
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            RestartBtn.SetActive(true);
        }

        // Calculate Point
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
            // All Health UI off
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);

            // Player Die Effect
            player.OnDie();

            // Result Ui
            Debug.Log("죽었습니다!");

            // Retry Button UI
            RestartBtn.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player Reposition
            if (health > 1) // 마지막 체력에서 낭떨어지일 땐, 원위치 하지 않기.
            {
                PlayerReposition();
            }

            // Health Down
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        if(player.tuto==true)
        {
            player.transform.position = new Vector3(-1, 0, -1);

        }
        else if(player.tuto2==true)
   
        {
            player.transform.position = new Vector3(180, 1, -1);

        }
        else
        {
            player.transform.position = new Vector3(0, 0, -1);

        }
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        Debug.Log("게임 오버!");

        // Stop the background music
        bgMusic.Stop();

        // Player Control Lock
        Time.timeScale = 1; // 씬 전환 전에 타임스케일을 원래대로 돌립니다.
        a = player.secretCount;
        // 데이터 저장
        PlayerPrefs.SetInt("TotalPoint", totalPoint);
        PlayerPrefs.SetInt("MoneyPoint", moneyPoint);
        PlayerPrefs.SetInt("CoffeeCount", coffeeCount);
        PlayerPrefs.SetInt("MilkCount", milkCount);
        PlayerPrefs.SetInt("UnknownItemCount", unknownItemCount);
        PlayerPrefs.SetFloat("RemainingTime", timer);
        PlayerPrefs.SetInt("SecretCount", player.secretCount);

        // 게임 오버 씬으로 이동
        SceneManager.LoadScene("ENDING");
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
