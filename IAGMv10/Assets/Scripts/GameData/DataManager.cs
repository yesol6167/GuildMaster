using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerData
{
    public int Gold;
}

public class DataManager : Singleton<DataManager>
{
    public PlayerData nowData = new PlayerData();

    public string path;
    public int nowSlot;

    private void Awake()
    {
        path = Application.persistentDataPath + "/Save"; // ���� ���
    }

    public void SaveData(int i)
    {
        nowData.Gold = i;
        string data = JsonUtility.ToJson(nowData); // ���̽����� ��ȯ
        File.WriteAllText(path + nowSlot.ToString(), data); // ���̽��� ����
    }
    public void LoadData()
    {
        string data = File.ReadAllText(path + nowSlot.ToString()); // ���̽��� �ҷ�����
        nowData = JsonUtility.FromJson<PlayerData>(data); // ���̽� -> PlayerData �������� ��ȯ
    }

    public void DataClear()
    {
        nowSlot = -1;
    }
}
