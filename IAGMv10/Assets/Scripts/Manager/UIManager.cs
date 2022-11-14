using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.ComponentModel;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[Serializable]
public class UIManager : MonoBehaviour
{
    public static UIManager Inst = null;

    //10����� ���
    public TMP_Text Gold;
    public TMP_Text Fame;

    public bool ChangeGold = false; // ����� ��ȭ�� ����
    public bool ChangeFame = false;
    public bool ExtendFail = false;
   
    public GameObject GoldIncrease; // ����� ����
    public GameObject FameIncrease; // ���� ����
    public GameObject NoticeWindow;

    //QuestInfo���� �ű�
    //���ε�
    public GameObject ParentOBJ;
    public GameObject NewsBArea; // NewsBalloon�� �ڽ����� �����Ǵ� UiCanvas���� �θ� ������Ʈ
    public GameObject WindowArea; // AdDataWindow�� �ڽ����� �����Ǵ� UiCanvas���� �θ� ������Ʈ

    float time = 2.0f; // �������� �ð�
    float E_time = 2.0f; // ����
    bool timecheck = false;
    float orgtime = 2.0f; // �������� ���� �ʱ�ȭ �Ǵ� ��
    //time��orgtime�� ���� ���ƾ���

    //����ü
    public struct USERDATA
    {
        [SerializeField] int gold;
        [SerializeField] int fame;

        public int Gold
        {
            get => GameManager.Instance.Gold;
            set
            {
                GameManager.Instance.Gold = value;
            }
        }
        public int Fame
        {
            get => GameManager.Instance.Fame;
            set
            {
                GameManager.Instance.Fame = value;
            }
        }
    }

    private void Awake()
    {
        Inst = this;
    }

    void Update()
    {
        Gold.text = GameManager.Instance.Gold.ToString();
        Fame.text = GameManager.Instance.Fame.ToString();


        if (timecheck)
        {
            time -= Time.unscaledDeltaTime;
            if(time < 0)
            {
                GoldIncrease.SetActive(false);
                FameIncrease.SetActive(false);
                time = orgtime;
            }
        }

        if (ExtendFail) // ��尡 ���ڶ� ���࿡ �������� ��
        {
            NoticeWindow.SetActive(true);

            E_time -= Time.deltaTime;
            if (E_time < 0f) // ���� ���� ���� ���� �����ְ� �����
            {
                NoticeWindow.SetActive(false);
                ExtendFail = false;
                E_time = orgtime;
            }
        }
    }

    public void UiChangeGold(int Price) // ������ ��� ǥ��
    {
        timecheck = true; // �ð����

        if (Price > 0) // ��� -> ���������� ǥ��
        {
            GoldIncrease.GetComponent<TMP_Text>().text = $"<color=red>+{Price}";
        }
        else // ���� -> �Ķ������� ǥ��
        {
            GoldIncrease.GetComponent<TMP_Text>().text = $"<color=blue>{Price}";
        }
        GoldIncrease.SetActive(true);
    }

    public void UiChangeFame(int Price) // ������ �� ǥ��
    {
        timecheck = true; // �ð����

        if (Price > 0) // ��� -> ���������� ǥ��
        {
            FameIncrease.GetComponent<TMP_Text>().text = $"<color=red>+{Price}";
        }
        else // ���� -> �Ķ������� ǥ��
        {
            FameIncrease.GetComponent<TMP_Text>().text = $"<color=blue>{Price}";
        }
        FameIncrease.SetActive(true);
    }
}