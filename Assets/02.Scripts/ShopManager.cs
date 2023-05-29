using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance { get; private set; }

    private CoinManager coinManager;
    private PlayerStats playerStats;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        coinManager = CoinManager.Instance;
        playerStats = FindObjectOfType<PlayerStats>();
        coinManager = FindObjectOfType<CoinManager>();

    }

    private void Start()
    {
        GameManager.instance.shopManager = this;
    }
    public void BuyMaxHealthUpgrade(int price, int maxHealthIncrease)
    {
        if (coinManager != null)
        {
            if (coinManager.Coin >= price && maxHealthIncrease > 0)
            {
                coinManager.AddCoin(-price);
                //playerStats.IncreaseMaxHealth(maxHealthIncrease);

                // �ִ� ü�� ���� �� ���� �Ŵ����� ������Ʈ ��û
                GameManager.instance.UpdateMaxHealth(maxHealthIncrease);

                // �������� �����ϴ� �ڵ� �ۼ�

                coinManager.SaveCoin();
            }
            else
            {
                // ���� �Ұ����� ó��
            }
        }
        else
        {
            Debug.LogWarning("CoinManager not found.");
        }
    }


}
