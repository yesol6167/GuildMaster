using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class PubMotelIcon : MonoBehaviour // Bed + Meat + Sleep + Eat ������
{
    public Transform myIconZone;
    public GameObject myHost;
    public int Price = 100; // ����
    TMP_Text text;

    // Chair&Bed Check
    ChairBedChk bedchairvalue;
    public bool Pullchk;

    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(myIconZone.position);
        transform.position = pos;
    }
    public void delete()
    {
        Destroy(gameObject);
    }

    public void AddGold()
    {
        GameManager.Instance.ChangeGold(Price);
    }

    public void ChairChk()
    {
        bedchairvalue = FindObjectOfType<ChairBedChk>();

        
        for(int i = 0; i < bedchairvalue.chairSlot.Count; i++)
        { 
            if(bedchairvalue.chairSlot[i] == ChairBedChk.ChairSlot.Check) // �ڸ��� ���� �� á�� ��
            {
                Pullchk = true;
            }
            else 
            {
                Pullchk = false;
                break;
            }
        }
        if(Pullchk == true)
        {
            myHost.GetComponent<Host>().onAngry = true;
        }
    }

    public void BedChk()
    {
        bedchairvalue = FindObjectOfType<ChairBedChk>();

        for (int i = 0; i < bedchairvalue.bedSlot.Count; i++)
        {
            if (bedchairvalue.bedSlot[i] == ChairBedChk.BedSlot.Check) 
            {
                Pullchk = true;
            }
            else 
            {
                Pullchk = false;
                break;
            }
        }
        if (Pullchk == true)
        {
            myHost.GetComponent<Host>().onAngry = true;
        }
    }
}
