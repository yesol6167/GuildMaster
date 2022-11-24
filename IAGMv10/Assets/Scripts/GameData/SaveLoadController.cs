
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class SaveLoadController : MonoBehaviour
{
    SaveLoadController inst = null;
    public int NowSave = 0;
    public string SaveName = "Save_";
    public TMP_Text[] SaveBtn;
    public Color changecolor;
    public TMP_Text[] ChooseFile;
    public int num;

    bool[] SaveFile = new bool[3]; //���̺����� �������� ����

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        for (int i = 0; i < SaveBtn.Length; ++i)
        {
            SaveBtn[i].text = "�������";
        }
    }

    public void LoadClick()
    {
        ADNpc.ADNPC LoadData = SaveSystem.Load<ADNpc.ADNPC>(SaveName + NowSave.ToString());
        UIManager.USERDATA uIManager = SaveSystem.Load<UIManager.USERDATA>(SaveName + NowSave.ToString());

        string temp = JsonUtility.ToJson(uIManager);
        string statinfo = JsonUtility.ToJson(LoadData);
        Debug.Log("adnpc ���: " + statinfo);
        Debug.Log("uimanager ����: " + temp);
    }

    public void ChoiceFile(int number)
    {
        SaveBtn[number].GetComponentInParent<UnityEngine.UI.Image>().color = changecolor;
        for(int i = 0; i < SaveBtn.Length; i++)
        {
            if(i != number)
            {
                SaveBtn[i].GetComponentInParent<UnityEngine.UI.Image>().color = Color.white;
            }
        }
        ChooseFile[number] = SaveBtn[number];
        num = number;
    }

    public void OnSaveFile()
    {
        inst.NowSave = num;
        //�����ϱ� ��ư �ѹ� ���������� save_���� �þ��

        ADNpc.ADNPC adnpc = new ADNpc.ADNPC();
        UIManager.USERDATA uIManager = new UIManager.USERDATA();

        SaveSystem.Save<ADNpc.ADNPC>(adnpc, SaveName + NowSave.ToString());
        SaveSystem.Save<UIManager.USERDATA>(uIManager, SaveName + NowSave.ToString());

        Debug.Log(SaveName + NowSave.ToString());
        if (File.Exists(SaveSystem.SavePath + $"{SaveName} + 0")) //�����Ͱ� �ִ� ���
        {
            LoadClick();
            SaveBtn[num].text = SaveName + NowSave.ToString();
        }
        else
        {
            SaveBtn[num].text = SaveName + NowSave.ToString();
        }

        ChooseFile[num].GetComponentInParent<UnityEngine.UI.Image>().color = Color.white;
        //Debug.Log("���� ����");
    }

    public void OnLodeFile()
    {
        //SceneManager.LoadScene("Main");
    }
}
