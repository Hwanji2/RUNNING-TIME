using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu1 : MonoBehaviour
{
    public GameObject settingsPanel;  // ���� �г� UI
    public Button openSettingsButton; // ���� ���� ��ư
    public Button closeSettingsButton; // ���� �ݱ� ��ư

    // Start �޼���� �� ���� �����ؾ� �մϴ�.
    void Start()
    {
        // ���� �� ���� �г� ��Ȱ��ȭ
        settingsPanel.SetActive(false);

        // ��ư Ŭ�� �̺�Ʈ ���
        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
    }

    // ���� �г� ����
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0; // ���� �Ͻ� ����
    }

    // ���� �г� �ݱ�
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1; // ���� �簳
    }
}
