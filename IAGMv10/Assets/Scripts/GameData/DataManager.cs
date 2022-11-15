using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public int SlotNum;
    public string pathSave;
    public string pathLoad;

    void Awake()
    {
        pathSave = Path.Combine(Application.dataPath, "Save", "Save");
        pathLoad = Path.Combine(Application.dataPath, "Load", "Load.bin");
    }

    private void Start()
    {
        if (File.Exists(pathLoad))// �ε� 
        {
            SaveData save = Load_pathLoad();
            Data_to_Game(save); // [LODE] �����͸� ���ӿ� ����
        }
    }


    

    public void Save(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(pathSave + $"{SlotNum}.bin");
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public SaveData Load()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.OpenRead(pathSave + $"{SlotNum}.bin");
            SaveData data = (SaveData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        catch // ����� ������ ���� ��
        {
            return default;
        }
    }

    public void Save_pathLoad()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Create(pathLoad);
        formatter.Serialize(stream, Load());
        stream.Close();
    }

    public SaveData Load_pathLoad()
    {
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = File.OpenRead(pathLoad);
            SaveData data = (SaveData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        catch // ����� ������ ���� ��
        {
            return default;
        }
    }

    public void Data_to_Game(SaveData save) // �����͸� ���ӿ� ����
    {
        GameManager.Instance.Gold = save.Gold;
        GameManager.Instance.Fame = save.Fame;
        TimeManager.Instance.DayCount = save.Day;
        TimeManager.Instance.MonthCount = save.Month;
        TimeManager.Instance.SeasonCount = save.Season;

        ExtendLoad(save);
        RQListLoad(save);
    }

    public void RQListLoad(SaveData save)
    {
        Quest.QuestInfo[] RQarray = save.RQList.ToArray();
        for (int i = 0; i < RQarray.Length; i++)
        {
            QuestManager.Instance.PostedQuest(RQarray[i]);
        }
    }

    public void ExtendLoad(SaveData save)
    {
        //����
        for (int i = 0; i < save.RoomsCount; ++i)
        {
            RoomExtend.Instance.M_ExtendLoad();
        }

        //����
        for (int i = 0; i < save.TableSetCount; ++i)
        {
            RoomExtend.Instance.P_ExtendLoad();
        }
    }
}
