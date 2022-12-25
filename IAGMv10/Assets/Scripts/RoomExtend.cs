using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomExtend : Singleton<RoomExtend>
{
    ChairBedChk _furniture = null;
    ChairBedChk Furniture
    {
        get
        {
            if (_furniture == null)
            {
                _furniture = GameObject.FindObjectOfType<ChairBedChk>();
            }
            return _furniture;
        }
    }
    public GameObject RoomExtendBtn; // �� ���� ��ư
    public GameObject PubChairBtn; // �Ĵ� ���� ���� ��ư
    public GameObject[] Rooms;
    public GameObject[] PubChairs;
    public int RoomsCount = 0;
    public int TableSetCount = 0;
    int Price = -1000; // ����

    public void RExtend() //�� ����
    {
        if (GameManager.Instance.Gold >= -Price)
        {
            if (Furniture._bedSlot.Count < 5)
            {
                Furniture._bedSlot.Add(ChairBedChk.BedSlot.None);
                Rooms[RoomsCount].SetActive(true);
                RoomsCount++;
                GameManager.Instance.ChangeGold(Price);
            }
        }
        else
        {
            UIManager.Inst.ExtendFail = true;
        }
    }

    public void PubExtend() //�Ĵ� ����
    {
        if (GameManager.Instance.Gold >= -Price)
        {
            if (Furniture._chairSlot.Count < 11)
            {
                for (int i = 0; i < 2; i++)
                {
                    Furniture._chairSlot.Add(ChairBedChk.ChairSlot.None);
                }
                PubChairs[TableSetCount].SetActive(true);
                TableSetCount++;
                GameManager.Instance.ChangeGold(Price);
            }
        }
        else
        {
            UIManager.Inst.ExtendFail = true;
        }
    }

    public void M_ExtendLoad() // �������� �ε� �Ҷ� ����Ǵ� �Լ�
    {
        Furniture._bedSlot.Add(ChairBedChk.BedSlot.None);
        Rooms[RoomsCount].SetActive(true);
        RoomsCount++;
    }

    public void P_ExtendLoad() // �Ĵ���� �ε� �Ҷ� ����Ǵ� �Լ�
    {
        for (int i = 0; i < 2; i++)
        {
            Furniture._chairSlot.Add(ChairBedChk.ChairSlot.None);
        }
        PubChairs[TableSetCount].SetActive(true);
        TableSetCount++;
    }
}