using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveLodeWindow : MonoBehaviour
{
    public TMP_Text[] Slots;
    public TMP_Text SelectedSlot;
    public string SavedSlot = "����� ����";

    bool[] savefiles = new bool[3];

    private void Start()
    {
        // ����� ������ �ִ��� �˻� -> ������ ������ �̸� �ٲ��ֱ�
        for(int i = 0; i < Slots.Length; i++)
        {
            if(File.Exists(DataManager.Instance.path + $"{i}")) // ���� ���ڷ� �� ������ �̸��� ���� ���
            {
                savefiles[i] = true;
                DataManager.Instance.nowSlot = i;
                DataManager.Instance.LoadData();
                GameManager.Instance.Gold = DataManager.Instance.nowData.Gold;
                Slots[i].text = SavedSlot;
            }
        }
        DataManager.Instance.DataClear();
        // �ҷ����� ���� �Ϸ� -> �������: 3���� ����� ������ ��� -> 3��° ������ ���� �ҷ����� -> �ذ��� ���� ���������� ����� ������ ����?
    }

    public void SelecteSlot(int num)
    {
        DataManager.Instance.nowSlot = num;
        SelectedSlot = Slots[num];
    }

    public void OnSave()
    {
        DataManager.Instance.SaveData(GameManager.Instance.Gold);
        SelectedSlot.text = SavedSlot;
    }

    public void OnLoad()
    {
        SceneManager.LoadScene(1);
    }


}
