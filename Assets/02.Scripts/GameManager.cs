using System.Collections;
using TempleRun.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] RewardedAdsButton rewardedAdsButton;
    [SerializeField]
    public InterstitialAd interstitialAd;


  

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
    public GameObject shopManagerPrefad;   // ���� �Ŵ��� ������

    private CoinManager coinManagerInstance;   // ������ ���� �Ŵ��� �ν��Ͻ�
    private PlayerStats playerStatsInstance;   // ������ �÷��̾� ���� �ν��Ͻ�
    private ShopManager shopManagerInstance;   // ������ ���� �Ŵ��� �ν��Ͻ�

    private PlayerController playerController;
    private int currentHealth;
    private PlayerStats playerStats;
    public ShopManager shopManager;
    public CoinManager coinManager;

    

    public static GameManager instance;
    private Data data;
    //���̽� ������
    public List<string> testDataA = new List<string>();
    public List<int> testDataB = new List<int>();
    public int Coin;  
    public int maxHealth;
   


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
                //CreatePlayerStats();
            }
        }
    

        if (shopManagerInstance == null)
        {
            //CreateShopManager();
        }



        coinManager = CoinManager.Instance;
    }

      void Start()
	{
        playerController = FindObjectOfType<PlayerController>();
        Debug.Log(SceneManager.GetActiveScene().name);
        if(SceneManager.GetActiveScene().name == "Stage1")
        {
           rewardedAdsButton._showAdButton.interactable = true;
        }

        //shopManager = FindObjectOfType<ShopManager>();

        Data data = GetComponent<Data>();
        data.JsonLoad();

    }

    // �ִ� ü�� ���� ����� ������ ������ ���̽��� ����
    public void UpdateMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;

        // ������ ���̽��� ����
        data.JsonSave();
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

    /*
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

    
    private void CreateShopManager()
    {
        if (shopManagerPrefad != null)
        {
            shopManagerInstance = Instantiate(shopManagerPrefad).GetComponent<ShopManager>();
            DontDestroyOnLoad(shopManagerInstance.gameObject);
        }
        else
        {
            Debug.LogWarning("ShopManager prefab is missing!");
        }
    }
    */
    

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
