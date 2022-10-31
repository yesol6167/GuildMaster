using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

public class Extend : MonoBehaviour
{
    SpawnManager Furniture; //����
    public TMP_Text ChairCountTxt;
    public TMP_Text BedCountTxt;
    int ChairCount;
    int RoomCount;
    public GameObject ExtendIcon;
    public GameObject ExtendWindow;
    public bool ExtendActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        Furniture = GameObject.FindObjectOfType<SpawnManager>();
        ExtendWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ChairCount = Furniture._chairSlot.Count; //���� ���� ��������
        RoomCount = Furniture._bedSlot.Count; //ħ�� ���� ��������
        ChairCountTxt.text = "���� ���̺� ���� : " + (ChairCount * 0.5).ToString() + "��";
        BedCountTxt.text = "���� �� ���� : " + RoomCount.ToString() + "��";
    }

    //���� ������ ������ ���� â �߰�
    public void TryOpenExtendWindow()
    {
        ExtendActivated = !ExtendActivated;

        if (ExtendActivated) OpenExtendWindow();
        else CloseExtendWindow();
    }

    public void OpenExtendWindow()
    {
        ExtendWindow.SetActive(true);
    }

    public void CloseExtendWindow()
    {
        ExtendWindow.SetActive(false);
    }
}
