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
        get { return coin; } // ���� ���� ��ȯ
        set { coin = value; } // ���� ���� ����
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
        // ���� �ؽ�Ʈ ���� ����
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
