using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] shopUIs; // 7개의 상점 UI
    public Button[] closeButtons; // 각 상점 UI의 X 버튼
    public Button[] coffeeButtons; // 각 상점 UI의 커피 버튼
    public Button[] milkButtons; // 각 상점 UI의 우유 버튼
    public Button[] unknownItemButtons; // 각 상점 UI의 ??? 버튼
    public AudioClip purchaseSound; // 구입 사운드 클립

    private int currentShopIndex = 0;
    private AudioSource audioSource; // 오디오 소스

    public GameManager gameManager; // Reference to GameManager

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 가장 작은 배열 길이를 찾아서 사용
        int minCount = Mathf.Min(
            shopUIs.Length,
            closeButtons.Length,
            coffeeButtons.Length,
            milkButtons.Length,
            unknownItemButtons.Length
        );

        if (minCount < shopUIs.Length)
        {
            Debug.LogWarning($"⚠️ 일부 버튼 배열의 길이가 부족합니다. shopUIs: {shopUIs.Length}, " +
                             $"closeButtons: {closeButtons.Length}, coffeeButtons: {coffeeButtons.Length}, " +
                             $"milkButtons: {milkButtons.Length}, unknownItemButtons: {unknownItemButtons.Length}");
        }

        for (int i = 0; i < minCount; i++) // 배열 중 최소 길이까지만 반복
        {
            int index = i; // 로컬 변수로 인덱스 저장

            if (closeButtons[index] != null)
                closeButtons[index].onClick.AddListener(() => CloseShop());

            if (coffeeButtons[index] != null)
                coffeeButtons[index].onClick.AddListener(() => BuyCoffee());

            if (milkButtons[index] != null)
                milkButtons[index].onClick.AddListener(() => BuyMilk());

            if (unknownItemButtons[index] != null)
                unknownItemButtons[index].onClick.AddListener(() => BuyUnknownItem());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // 특정 키를 눌러 상점 열기
        {
            ToggleShopUI();
        }
    }

    void ToggleShopUI()
    {
        if (shopUIs.Length == 0)
        {
            Debug.LogWarning("⚠️ shopUIs 배열이 비어 있습니다. Unity에서 확인하세요.");
            return;
        }

        bool isActive = !shopUIs[currentShopIndex].activeSelf;
        foreach (GameObject shopUI in shopUIs)
        {
            if (shopUI != null)
                shopUI.SetActive(isActive);
        }
    }

    public void CloseShop()
    {
        foreach (GameObject shopUI in shopUIs)
        {
            if (shopUI != null)
                shopUI.SetActive(false);
        }
    }

    public void BuyCoffee()
    {
        if (gameManager != null && gameManager.DeductMoney(1100)) // 커피 가격
        {
            gameManager.coffeeCount++;
            PlayPurchaseSound();
            Debug.Log("Coffee purchased. Total: " + gameManager.coffeeCount);
        }
    }

    public void BuyMilk()
    {
        if (gameManager != null && gameManager.DeductMoney(2300)) // 우유 가격
        {
            gameManager.milkCount++;
            PlayPurchaseSound();
            Debug.Log("Milk purchased. Total: " + gameManager.milkCount);
        }
    }

    public void BuyUnknownItem()
    {
        if (gameManager != null && gameManager.DeductMoney(999999)) // ??? 가격
        {
            gameManager.unknownItemCount++;
            PlayPurchaseSound();
            Debug.Log("Unknown item purchased. Total: " + gameManager.unknownItemCount);
        }
    }

    private void PlayPurchaseSound()
    {
        if (audioSource != null && purchaseSound != null)
        {
            audioSource.PlayOneShot(purchaseSound);
        }
    }
}
