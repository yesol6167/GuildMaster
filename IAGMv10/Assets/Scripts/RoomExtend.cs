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

    // Start is called before the first frame update
    void Start()
    {
        Furniture = GameObject.FindObjectOfType<ChairBedChk>();
    }
    public void RExtend() //�� ����
    {
        if (Furniture._bedSlot.Count < 5)
        {
            Furniture._bedSlot.Add(ChairBedChk.BedSlot.None);
            Rooms[RoomsCount].SetActive(true);
            RoomsCount++;
        }


    }

    public void PubExtend() //�Ĵ� ����
    {
        if (Furniture._chairSlot.Count < 11)
        {
            for (int i = 0; i < 2; i++)
            {
                Furniture._chairSlot.Add(ChairBedChk.ChairSlot.None);
            }
            PubChairs[PubCount].SetActive(true);
            PubCount++;
        }
    }

}