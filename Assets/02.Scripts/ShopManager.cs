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
        if (coinManager != null && playerStats != null)
        {
            if (coinManager.Coin >= price && maxHealthIncrease > 0)
            {
                Debug.LogWarning("yes");
                coinManager.AddCoin(-price);
                playerStats.IncreaseMaxHealth(maxHealthIncrease);
                // 아이템을 구매하는 코드 작성
            }
            else
            {
                Debug.LogWarning("no");
                // 구매 불가능한 처리
            }
        }
        else
        {
            Debug.LogWarning("CoinManager or PlayerStats not found.");
        }
    }

}
