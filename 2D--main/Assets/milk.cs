using UnityEngine;
using UnityEngine.UI;

public class MilkCountDisplay : MonoBehaviour
{
    public Text milkCountText;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateMilkCount();
    }

    void Update()
    {
        UpdateMilkCount();
    }

    void UpdateMilkCount()
    {
        milkCountText.text = gameManager.milkCount.ToString();
    }
}
