using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject[] shopUIs; // 5���� ���� UI
    public Button[] closeButtons; // �� ���� UI�� X ��ư
    public Button[] coffeeButtons; // �� ���� UI�� Ŀ�� ��ư
    public Button[] milkButtons; // �� ���� UI�� ���� ��ư
    public Button[] unknownItemButtons; // �� ���� UI�� ??? ��ư
    public AudioClip purchaseSound; // ���� ���� Ŭ��

    private int currentShopIndex = 0;
    private AudioSource audioSource; // ����� �ҽ�

    public GameManager gameManager; // Reference to GameManager

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // �� ��ư�� Ŭ�� �̺�Ʈ �߰�
        for (int i = 0; i < shopUIs.Length; i++)
        {
            int index = i; // ���� ������ �ε��� ����
            closeButtons[i].onClick.AddListener(() => CloseShop());
            coffeeButtons[i].onClick.AddListener(() => BuyCoffee());
            milkButtons[i].onClick.AddListener(() => BuyMilk());
            unknownItemButtons[i].onClick.AddListener(() => BuyUnknownItem());
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Ư�� Ű�� ���� ���� ����
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
        if (gameManager.DeductMoney(1100)) // Ŀ�� ����
        {
            gameManager.coffeeCount++;
            PlayPurchaseSound();
            Debug.Log("Coffee purchased. Total: " + gameManager.coffeeCount);
        }
    }

    public void BuyMilk()
    {
        if (gameManager.DeductMoney(2300)) // ���� ����
        {
            gameManager.milkCount++;
            PlayPurchaseSound();
            Debug.Log("Milk purchased. Total: " + gameManager.milkCount);
        }
    }

    public void BuyUnknownItem()
    {
        if (gameManager.DeductMoney(999999)) // ??? ����
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
