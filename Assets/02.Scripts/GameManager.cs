using System.Collections;
using TempleRun.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    [SerializeField]
    public InterstitialAd interstitialAd;

    public static GameManager instance;

	private GameObject PauseCanvas;
    public GameObject gameOverCanvas;
	
    public GameObject clearCanvas;
    bool Paused;
	private bool isPaused = false;
    [SerializeField]  private GameObject storeCanvas;

    public float destinationDistance = 15000f; // ��ǥ �Ÿ� ����
    public Slider distanceSlider; // �Ÿ� ǥ�ÿ� �����̴�
    public TextMeshProUGUI distanceText; // �Ÿ� ǥ�ÿ� �ؽ�Ʈ

    // ���� ����

    public GameObject coinManagerPrefab;   // ���� �Ŵ��� ������
    public GameObject playerStatsPrefab;   // �÷��̾� ���� ������

    private CoinManager coinManagerInstance;   // ������ ���� �Ŵ��� �ν��Ͻ�
    private PlayerStats playerStatsInstance;   // ������ �÷��̾� ���� �ν��Ͻ�


    private PlayerController playerController;
    private int currentHealth;
    private PlayerStats playerStats;
    public ShopManager shopManager;

    private CoinManager coinManager;

    //public Button pauseButton;

    [SerializeField] 
	private GameObject MenuUI;

    // ü�� UI ������Ʈ �Լ�

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        shopManager = FindObjectOfType<ShopManager>();

        // ���� �Ŵ����� �̹� �ִ� ��� �ߺ� ������ �����ϱ� ���� �˻�
        if (coinManagerInstance == null)
        {
            CreateCoinManager();
        }

        if (playerStatsInstance == null)
        {
            // �̹� ������ PlayerStats �ν��Ͻ��� �ִ��� Ȯ��
            PlayerStats existingPlayerStats = FindObjectOfType<PlayerStats>();
            if (existingPlayerStats != null)
            {
                playerStatsInstance = existingPlayerStats;
            }
            else
            {
                CreatePlayerStats();
            }
        }
        ShopManager existingShopManager = FindObjectOfType<ShopManager>();




        coinManager = CoinManager.Instance;
    }

    private void CreateCoinManager()
    {
        if (coinManagerPrefab != null)
        {
            coinManagerInstance = Instantiate(coinManagerPrefab).GetComponent<CoinManager>();
            DontDestroyOnLoad(coinManagerInstance.gameObject);
        }
        else
        {
            Debug.LogWarning("CoinManager prefab is missing!");
        }
    }

    private void CreatePlayerStats()
    {
        if (playerStatsPrefab != null)
        {
            playerStatsInstance = Instantiate(playerStatsPrefab).GetComponent<PlayerStats>();
            DontDestroyOnLoad(playerStatsInstance.gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerStats prefab is missing!");
        }
    }



    //�� ��ư�� ������
    public void RestartGame()
    {
        // ���� ���� �ʱ�ȭ
        // ù ����� �����´�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Debug.Log("���� �ٽ� ����!");
    }

	public void GoHome()  //���� UI���� Ȩ ȭ������ �Ѿ
	{
		SceneManager.LoadScene("Home");
	}

	public void GameStart()   //Ȩ ȭ�鿡�� Stage1 �ε��ϰ� �ð� �帣�� ��
	{
		SceneManager.LoadScene(1);
		Time.timeScale = 1;
	}


    public void UpgradeMaxHealth()
    {
        if (shopManager != null)
        {
            shopManager.BuyMaxHealthUpgrade(100, 1);
        }
        else
        {
            Debug.LogWarning("ShopManager not found.");
        }
    }


    public void Menu()   //���� �޴� ����
    {
       
        MenuUI.SetActive(true);
		PauseGame();

	}

    public void Exit()   //���� �޴� ����
    {

        MenuUI.SetActive(false);
		ResumeGame();

	}

    void Start()
	{
        playerController = FindObjectOfType<PlayerController>();
        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == "Stage1")
        {
           rewardedAdsButton._showAdButton.interactable = true;
        }
 
    }

    public void Skip()
    {
        // ���� ���� �ε��մϴ�.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    

    void Update()
    {
        if (playerController != null)
        {
            float playerDistance = playerController.GetDistanceTraveled();
            UpdateDistance(playerDistance);
            CheckDistanceReached(playerDistance);
        }
    }

	public void PauseGame()
	{
		Time.timeScale = 0f;
		isPaused = true;
		
	}

	public void ResumeGame()
	{
        MenuUI.SetActive(false);
        Time.timeScale = 1f;
		isPaused = false;
	}

    public void store()
    {
        storeCanvas.SetActive(true);
        coinManager.UpdateCoinText();
    }

    public void storeExit()
    {
        storeCanvas.SetActive(false);
     
    }

    public void HpPurchase()
    {

    }

    public void NextStage()
    {
        // ���� ���� �����Ͽ� ���� ������ ��ȯ
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void UpdateDistance(float playerDistance)
    {
        float remainingDistance = Mathf.Max(0f, destinationDistance - playerDistance);
        float sliderValue = 1f - (remainingDistance / destinationDistance);

        // �Ÿ� ǥ�ÿ� �����̴� ������Ʈ
        if (distanceSlider != null)
        {
            distanceSlider.value = sliderValue;
        }

        // �Ÿ� ǥ�ÿ� �ؽ�Ʈ ������Ʈ
        if (distanceText != null)
        {
            distanceText.text = $"{playerDistance:F1}m";
        }
    }
    private void CheckDistanceReached(float playerDistance)
    {
        if (playerDistance >= destinationDistance)
        {
            playerController.gameObject.SetActive(false);
            clearCanvas.SetActive(true);
        }
    }


    



}