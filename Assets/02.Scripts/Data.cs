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

    private string path;
    public int initialCoin = 250;
    public int initialMaxHealth = 4;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
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
            GameManager.instance.Coin = initialCoin;
            GameManager.instance.maxHealth = initialMaxHealth;
            CoinManager.Instance.Coin = initialCoin;
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                GameManager.instance.Coin = saveData.Coin;
                GameManager.instance.maxHealth = saveData.maxHealth;
                CoinManager.Instance.Coin = saveData.Coin;
            }
        }
    }

    public void JsonSave()
    {
        if (path != null)
        {
            SaveData saveData = new SaveData();

            saveData.Coin = GameManager.instance.Coin;
            saveData.maxHealth = GameManager.instance.maxHealth;

            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
        }
    }
}
