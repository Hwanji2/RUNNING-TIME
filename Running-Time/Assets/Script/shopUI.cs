using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] shopUIs; // 5개의 상점 UI
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

        // 각 버튼에 클릭 이벤트 추가
        for (int i = 0; i < shopUIs.Length; i++)
        {
            int index = i; // 로컬 변수로 인덱스 저장
            closeButtons[i].onClick.AddListener(() => CloseShop());
            coffeeButtons[i].onClick.AddListener(() => BuyCoffee());
            milkButtons[i].onClick.AddListener(() => BuyMilk());
            unknownItemButtons[i].onClick.AddListener(() => BuyUnknownItem());
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
        bool isActive = !shopUIs[currentShopIndex].activeSelf;
        foreach (GameObject shopUI in shopUIs)
        {
            shopUI.SetActive(isActive);
        }
    }

    public void CloseShop()
    {
        foreach (GameObject shopUI in shopUIs)
        {
            shopUI.SetActive(false);
        }
    }

    public void BuyCoffee()
    {
        if (gameManager.DeductMoney(1100)) // 커피 가격
        {
            gameManager.coffeeCount++;
            PlayPurchaseSound();
            Debug.Log("Coffee purchased. Total: " + gameManager.coffeeCount);
        }
    }

    public void BuyMilk()
    {
        if (gameManager.DeductMoney(2300)) // 우유 가격
        {
            gameManager.milkCount++;
            PlayPurchaseSound();
            Debug.Log("Milk purchased. Total: " + gameManager.milkCount);
        }
    }

    public void BuyUnknownItem()
    {
        if (gameManager.DeductMoney(999999)) // ??? 가격
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
