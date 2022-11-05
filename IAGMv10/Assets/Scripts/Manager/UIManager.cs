using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    //10����� ���
    public TMP_Text Gold;
    public TMP_Text Fame;

    public bool ChangeGold = false; // ����� ��ȭ�� ����
    public bool ChangeFame = false;
    public GameObject GoldiIncrease; // ����� ����
    public GameObject FameIncrease; // ���� ����

    float time = 2.0f; // �������� �ð�
    float orgtime = 2.0f; // �������� ���� �ʱ�ȭ �Ǵ� ��
    //time��orgtime�� ���� ���ƾ���


    void Update()
    {
        if(ChangeGold) // ����� ���� ���ϸ� ���� ���� ���� ���� �����ְ� �����
        {
            GoldiIncrease.SetActive(true);

            time -= Time.deltaTime;
            if (time < 0f) // ���� ���� ���� ���� �����ְ� �����
            {
                GoldiIncrease.SetActive(false);
                ChangeGold = false;
                time = orgtime;
            }
        }

        if (ChangeFame) // ����� ���� ���ϸ� ���� ���� ���� ���� �����ְ� �����
        {
            FameIncrease.SetActive(true);

            time -= Time.deltaTime;
            if (time < 0f) // ���� ���� ���� ���� �����ְ� �����
            {
                GoldiIncrease.SetActive(false);
                ChangeFame = false;
                time = orgtime;
            }
        }
    }
}
