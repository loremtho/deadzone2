using TempleRun.Player;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 5;
    private PlayerController playerController;
    private CoinManager coinManager;
    private ShopManager shopManager;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController != null)
        {
            maxHealth = playerController.maxHealth;
        }
        else
        {
            Debug.LogWarning("PlayerController not found.");
        }

        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
    }
}
