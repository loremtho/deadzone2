using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    private int coin;

    public TextMeshProUGUI coinText;


    private static CoinManager instance;
    public static CoinManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CoinManager>();
                if (instance == null)
                {
                    GameObject coinManagerObject = new GameObject("CoinManager");
                    instance = coinManagerObject.AddComponent<CoinManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        UpdateCoinText();
        LoadInitialCoin();
       
    }


    private void LoadInitialCoin()
    {
        string path = Path.Combine(Application.dataPath, "database.json");
        if (File.Exists(path))
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                coin = saveData.Coin;
            }
        }

        UpdateCoinText();
    }

    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            UpdateCoinText();
            SaveCoin();
        }
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoinText();
        SaveCoin();
    }

    public void UpdateCoinText()
    {
        coinText = GameObject.Find("CoinText").GetComponent<TextMeshProUGUI>();
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
    }


    public void SaveCoin()
    {
        string path = Path.Combine(Application.dataPath, "database.json");

        SaveData saveData = new SaveData();
        saveData.Coin = coin;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);

        UpdateCoinText();
    }
}
