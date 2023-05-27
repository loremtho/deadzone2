using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    private CoinManager coinManager;
    private PlayerStats playerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        coinManager = CoinManager.Instance;
        playerStats = FindObjectOfType<PlayerStats>();
        coinManager = FindObjectOfType<CoinManager>();
    }

    public void BuyMaxHealthUpgrade(int price, int maxHealthIncrease)
    {
        if (coinManager != null && playerStats != null)
        {
            if (coinManager.Coin >= price && maxHealthIncrease > 0)
            {
                Debug.LogWarning("yes");
                coinManager.AddCoin(-price);
                playerStats.IncreaseMaxHealth(maxHealthIncrease);
                // �������� �����ϴ� �ڵ� �ۼ�
            }
            else
            {
                Debug.LogWarning("no");
                // ���� �Ұ����� ó��
            }
        }
        else
        {
            Debug.LogWarning("CoinManager or PlayerStats not found.");
        }
    }

}
