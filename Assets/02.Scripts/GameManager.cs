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

    public float destinationDistance = 15000f; // 목표 거리 설정
    public Slider distanceSlider; // 거리 표시용 슬라이더
    public TextMeshProUGUI distanceText; // 거리 표시용 텍스트

    // 게임 상태

    public GameObject coinManagerPrefab;   // 코인 매니저 프리팹
    public GameObject playerStatsPrefab;   // 플레이어 스탯 프리팹
    public GameObject shopManagerPrefad;   // 상점 매니저 프리펩

    private CoinManager coinManagerInstance;   // 생성된 코인 매니저 인스턴스
    private PlayerStats playerStatsInstance;   // 생성된 플레이어 스탯 인스턴스
    private ShopManager shopManagerInstance;   // 생성된 상점 매니저 인스턴스


    private PlayerController playerController;
    private int currentHealth;
    private PlayerStats playerStats;
    public ShopManager shopManager;
    public CoinManager coinManager;

   




    public static GameManager instance;
    //제이슨 데이터

    [SerializeField] public int coin;
    [SerializeField] public int maxHealth;


    [SerializeField] 
	private GameObject MenuUI;

    private Data data;

    // 체력 UI 업데이트 함수

    private void Awake()
    {
        // 싱글톤 인스턴스 생성
        if (instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        data = FindObjectOfType<Data>();

        if (data == null)
        {
            GameObject dataObject = new GameObject("Data");
            data = dataObject.AddComponent<Data>();
        }

        // 데이터 로드
        data.JsonLoad();

        // 게임 매니저가 이미 있는 경우 중복 생성을 방지하기 위해 검사
        /*
         if (coinManagerInstance == null)
         {
             CreateCoinManager();
         }
        
        if (coinManagerInstance == null)
        {
            CreateCoinManager();
        }

        if (coinManagerInstance == null)
        {
            // 이미 생성된 PlayerStats 인스턴스가 있는지 확인
            CoinManager existingCoinManager = FindObjectOfType<CoinManager>();
            if (existingCoinManager != null)
            {
                coinManagerInstance = existingCoinManager;
            }
            else
            {
                CreateCoinManager();
            }
        }

        if (playerStatsInstance == null)
        {
            // 이미 생성된 PlayerStats 인스턴스가 있는지 확인
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

        */
        coinManagerInstance = CoinManager.Instance;

     
        data.JsonLoad();

    }

      void Start()
	{
        playerController = FindObjectOfType<PlayerController>();
        Debug.Log(SceneManager.GetActiveScene().name);

        if (playerController != null)
        {
            playerController.maxHealth = 5; // 초기 최대 체력 값으로 설정
        }
        else
        {
            Debug.LogWarning("PlayerController not found.");
        }

        if (SceneManager.GetActiveScene().name == "Stage1")
        {
           rewardedAdsButton._showAdButton.interactable = true;
        }

        //shopManager = FindObjectOfType<ShopManager>();

        data.JsonLoad();

    }

    // 최대 체력 값이 변경될 때마다 데이터 제이슨에 저장
    public void UpdateMaxHealth(int increaseAmount)
    {
        if (playerController != null)
        {
            playerController.maxHealth += increaseAmount;
            Debug.Log("Max health updated. Current max health: " + playerController.maxHealth);
        }
        else
        {
            Debug.LogWarning("PlayerController not found. Cannot update max health.");
        }
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

    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            // 데이터 저장
            data.JsonSave();
        }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            maxHealth = value;
            // 데이터 저장
            data.JsonSave();
        }
    }



    //이 버튼을 누르면
    public void RestartGame()
    {
        // 게임 상태 초기화
        // 첫 장면을 가져온다
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Debug.Log("게임 다시 시작!");
    }

	public void GoHome()  //정지 UI에서 홈 화면으로 넘어감
	{
		SceneManager.LoadScene("Home");
	}

	public void GameStart()   //홈 화면에서 Stage1 로드하고 시간 흐르게 함
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


    public void Menu()   //게임 메뉴 실행
    {
       
        MenuUI.SetActive(true);
		PauseGame();

	}

    public void Exit()   //게임 메뉴 끄기
    {

        MenuUI.SetActive(false);
		ResumeGame();

	}

  

    public void Skip()
    {
        // 게임 씬을 로딩합니다.
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
        CoinManager.Instance.UpdateCoinText();
        Data.Instance.JsonSave();
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
        // 현재 씬을 측정하여 다음 씬으로 전환
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

        // 거리 표시용 슬라이더 업데이트
        if (distanceSlider != null)
        {
            distanceSlider.value = sliderValue;
        }

        // 거리 표시용 텍스트 업데이트
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
