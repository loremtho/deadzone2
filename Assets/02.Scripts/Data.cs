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

            GameManager.instance.Coin = initialCoin; // �÷��̾� ��带 �ʱⰪ���� ����
            GameManager.instance.maxHealth = initialMaxHealth; // �÷��̾� �Ŀ��� �ʱⰪ���� ����
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                GameManager.instance.Coin = saveData.Coin; // �÷��̾� ��带 �������� ����
                GameManager.instance.maxHealth = saveData.maxHealth; // �÷��̾� �Ŀ��� �ִ� ü������ ����
                CoinManager.Instance.Coin = saveData.Coin; // ���� ���� ���� �Ŵ����� ����
            }
        }
    }

    public void JsonSave()
    {

        if (path != null)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
           
            SaveData saveData = new SaveData();

            saveData.Coin = GameManager.instance.Coin; // �÷��̾��� ��带 �������� ����
            saveData.maxHealth = GameManager.instance.maxHealth; // �÷��̾��� �Ŀ��� �ִ� ü������ ����

            // ����
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(path, json);
        }
    }
}
