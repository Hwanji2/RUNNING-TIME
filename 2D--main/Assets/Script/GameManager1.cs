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
    private bool isPulsing; // isPulsing ���� ����


    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public Text UITimer; // Ÿ�̸� UI �ؽ�Ʈ �߰�
    public GameObject RestartBtn;

    public float timer = 180000f; // 3��(180,000�и���)
    private bool isGameOver = false;

    // ������ ���� ���� �߰�
    public int coffeeCount = 0;
    public int milkCount = 0;
    public int unknownItemCount = 0;
    public int a = 0;
    // AudioSource for background music
    private AudioSource bgMusic;
    private float pausedBGMTime; // BGM ���� �ð�

    private void Start()
    {
        // ������ �ҷ�����
        totalPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        moneyPoint = PlayerPrefs.GetInt("MoneyPoint", 0);
        coffeeCount = PlayerPrefs.GetInt("CoffeeCount", 0);
        milkCount = PlayerPrefs.GetInt("MilkCount", 0);
        unknownItemCount = PlayerPrefs.GetInt("UnknownItemCount", 0);

        // Get the AudioSource component
        bgMusic = GetComponent<AudioSource>();

        // Set the volume (0.0 to 1.0)
        bgMusic.volume = 0.8f; // Adjust the volume level as needed

        // 3�� �Ŀ� ���� ���
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
            bgMusic.time = pausedBGMTime; // ����� �ð����� ����
            bgMusic.Play();
        }
    }
    void Update()
    {
        if (!isGameOver)
        {
            // Ÿ�̸� ������Ʈ
            if (bgMusic.isPlaying ||(timer<=10000))
            {
              timer -= Time.deltaTime * 1000; // �и��� ������ ����
            }
            else
            {

            }
            
            if (timer <= 0)
            {
                timer = 0;
                GameOver();
            }

            // Ÿ�̸� UI ������Ʈ
            UpdateTimerUI();

            // ����Ʈ UI ������Ʈ
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
                    pausedBGMTime = bgMusic.time; // ���� ��� �ð� ����
                    bgMusic.Pause();
                }

                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                player.tuto = false;
                UIStage.text = "ü����";
            }
            else if(player.tuto2 == true)
            {
                stageIndex--;
                stageIndex--;

                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                ResumeBGM();
                player.tuto2 = false;
                UIStage.text = "�ٽ� ����";

            }
            else
            {
                stageIndex++;
                Stages[stageIndex].SetActive(true);
                PlayerReposition();
                UIStage.text = "���а�";
            }
            

       
        }
        else    // Game Clear
        {
            // Player Control Lock
            Time.timeScale = 0;

            // Result UI
            Debug.Log("���� Ŭ����!");

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
            Debug.Log("�׾����ϴ�!");

            // Retry Button UI
            RestartBtn.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Player Reposition
            if (health > 1) // ������ ü�¿��� ���������� ��, ����ġ ���� �ʱ�.
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
        Debug.Log("���� ����!");

        // Stop the background music
        bgMusic.Stop();

        // Player Control Lock
        Time.timeScale = 1; // �� ��ȯ ���� Ÿ�ӽ������� ������� �����ϴ�.
        a = player.secretCount;
        // ������ ����
        PlayerPrefs.SetInt("TotalPoint", totalPoint);
        PlayerPrefs.SetInt("MoneyPoint", moneyPoint);
        PlayerPrefs.SetInt("CoffeeCount", coffeeCount);
        PlayerPrefs.SetInt("MilkCount", milkCount);
        PlayerPrefs.SetInt("UnknownItemCount", unknownItemCount);
        PlayerPrefs.SetFloat("RemainingTime", timer);
        PlayerPrefs.SetInt("SecretCount", player.secretCount);

        // ���� ���� ������ �̵�
        SceneManager.LoadScene("ENDING");
    }

    // �ݾ� ���� �Լ� �߰�
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
