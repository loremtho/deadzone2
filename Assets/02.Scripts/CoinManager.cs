using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    private static CoinManager instance;
    public static CoinManager Instance { get { return instance; } }

    private int coin = 500;

    public int Coin
    {
        get { return coin; } // 코인 값을 반환
        set { coin = value; } // 코인 값을 설정
    }

    private TextMeshProUGUI coinText;
    private bool coinTextInitialized = false;
    
    private bool isTrackingCoinText = false;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        GameManager.instance.coinManager= this;
    }
    public void StartTrackingCoinText()
    {
        // 코인 텍스트 추적 시작
        isTrackingCoinText = true;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindCoinText();
        InitializeCoinText();
    }

    private void Update()
    {
        if (!coinTextInitialized)
        {
            FindCoinText();
            InitializeCoinText();
        }

        if (isTrackingCoinText)
        {
            if (!coinTextInitialized)
            {
                FindCoinText();
                InitializeCoinText();
            }
            else
            {
                UpdateCoinText();
            }
        }
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        if (coinText != null && coinText.gameObject.activeInHierarchy)
        {
            coinText.text = "Coin: " + coin.ToString();
        }
    }

    private void FindCoinText()
    {
        coinText = GameObject.Find("CoinText")?.GetComponent<TextMeshProUGUI>();
    }

    private void InitializeCoinText()
    {
        if (coinText != null)
        {
            coinTextInitialized = true;
            coinText.text = "Coin: " + coin.ToString();
        }
    }
}
