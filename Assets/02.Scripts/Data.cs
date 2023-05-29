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
            GameManager.instance.Coin = 100; // 플레이어 골드를 코인으로 변경
            GameManager.instance.maxHealth = 4; // 플레이어 파워를 최대 체력으로 변경
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
                GameManager.instance.Coin = saveData.Coin; // 플레이어 골드를 코인으로 변경
                GameManager.instance.maxHealth = saveData.maxHealth; // 플레이어 파워를 최대 체력으로 변경
                CoinManager.Instance.Coin = saveData.Coin; // 코인 값을 코인 매니저에 설정
                
            }
        }
    }

    public void JsonSave()
    {
        SaveData saveData = new SaveData();

        for (int i = 0; i < 10; i++)
        {
            saveData.testDataA.Add("테스트 데이터 no " + i);
        }

        for (int i = 0; i < 10; i++)
        {
            saveData.testDataB.Add(i);
        }

        saveData.Coin = GameManager.instance.Coin; // 플레이어의 골드를 코인으로 저장
        saveData.maxHealth = GameManager.instance.maxHealth; // 플레이어의 파워를 최대 체력으로 저장

        // 저장
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(path, json);
    }
}
