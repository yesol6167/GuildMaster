using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoomExtend : MonoBehaviour
{
    ChairBedChk Furniture; //����
    public GameObject RoomExtendBtn; // �� ���� ��ư
    public GameObject PubChairBtn; // �Ĵ� ���� ���� ��ư
    public GameObject[] Rooms;
    public GameObject[] PubChairs;
    public int RoomsCount = 0;
    public int PubCount = 0;
    int Price = -1000; // ����

    void Start()
    {
        Furniture = GameObject.FindObjectOfType<ChairBedChk>();
    }
    public void RExtend() //�� ����
    {
        
        if(GameManager.Instance.Gold >= -Price)
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
            UIManager.Instance.ExtendFail = true;
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
                PubChairs[PubCount].SetActive(true);
                PubCount++;
                GameManager.Instance.ChangeGold(Price);
            }
        }
        else
        {
            UIManager.Instance.ExtendFail = true;
        }
    }
}