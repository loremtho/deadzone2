using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int Coin;
    public int maxHealth;
}

public class Data : MonoBehaviour
{
    string path;

    public int initialCoin = 250;
    public int initialMaxHealth = 4;

    private static Data instance;
    public static Data Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Data>();
                if (instance == null)
                {
                    GameObject dataObject = new GameObject("Data");
                    instance = dataObject.AddComponent<Data>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        path = Path.Combine(Application.dataPath, "database.json");
        JsonLoad();

    }

    public void JsonLoad()
    {
        if (!File.Exists(path))
        {

            GameManager.instance.Coin = initialCoin; // 플레이어 골드를 초기값으로 변경
            GameManager.instance.maxHealth = initialMaxHealth; // 플레이어 파워를 초기값으로 변경
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                GameManager.instance.Coin = saveData.Coin; // 플레이어 골드를 코인으로 변경
                GameManager.instance.maxHealth = saveData.maxHealth; // 플레이어 파워를 최대 체력으로 변경
                CoinManager.Instance.Coin = saveData.Coin; // 코인 값을 코인 매니저에 설정
            }
        }
    }

    public void JsonSave()
    {

        if (path != null)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
           
            SaveData saveData = new SaveData();

            saveData.Coin = GameManager.instance.Coin; // 플레이어의 골드를 코인으로 저장
            saveData.maxHealth = GameManager.instance.maxHealth; // 플레이어의 파워를 최대 체력으로 저장

            // 저장
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
        }
    }
}
