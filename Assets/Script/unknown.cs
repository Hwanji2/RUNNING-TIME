using UnityEngine;
using UnityEngine.UI;

public class UnknownItemCountDisplay : MonoBehaviour
{
    public Text unknownItemCountText;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateUnknownItemCount();
    }

    void Update()
    {
        UpdateUnknownItemCount();
    }

    void UpdateUnknownItemCount()
    {
        unknownItemCountText.text =  gameManager.unknownItemCount.ToString();
    }
}
