using UnityEngine;
using UnityEngine.UI;

public class CoffeeCountDisplay : MonoBehaviour
{
    public Text coffeeCountText;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateCoffeeCount();
    }

    void Update()
    {
        UpdateCoffeeCount();
    }

    void UpdateCoffeeCount()
    {
        coffeeCountText.text = gameManager.coffeeCount.ToString();
    }
}
