using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<string> testDataA = new List<string>();
    public List<int> testDataB = new List<int>();

    public int Coin;
    public int maxHealth;
}

public class Data : MonoBehaviour
{
    string path;


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
            GameManager.instance.Coin = 100; // �÷��̾� ��带 �������� ����
            GameManager.instance.maxHealth = 4; // �÷��̾� �Ŀ��� �ִ� ü������ ����
            JsonSave();
        }
        else
        {
            string loadJson = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(loadJson);

            if (saveData != null)
            {
                GameManager.instance.testDataA = saveData.testDataA;
                GameManager.instance.testDataB = saveData.testDataB;
                GameManager.instance.Coin = saveData.Coin; // �÷��̾� ��带 �������� ����
                GameManager.instance.maxHealth = saveData.maxHealth; // �÷��̾� �Ŀ��� �ִ� ü������ ����
                CoinManager.Instance.Coin = saveData.Coin; // ���� ���� ���� �Ŵ����� ����
                
            }
        }
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        for (int i = 0; i < 10; i++)
        {
            saveData.testDataA.Add("�׽�Ʈ ������ no " + i);
        }

        for (int i = 0; i < 10; i++)
        {
            saveData.testDataB.Add(i);
        }

        saveData.Coin = GameManager.instance.Coin; // �÷��̾��� ��带 �������� ����
        saveData.maxHealth = GameManager.instance.maxHealth; // �÷��̾��� �Ŀ��� �ִ� ü������ ����

        // ����
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
