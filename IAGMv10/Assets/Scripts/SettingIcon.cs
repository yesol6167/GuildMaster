using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingIcon : MonoBehaviour
{
    public GameObject ESCWindow; //ESC ������ ������ â
    public GameObject CheckWindow; // �׸��ϱ� ������ ������ â
    public GameObject SettingWindow; // ESC Window ���� ���� ������ ������ ������ ���� â
    public GameObject NoTouch;
    public bool IsOpen = false;
    public bool ESCClick = false;
    TimeBtns Time;
   
    // Start is called before the first frame update
    void Start()
    {
        ESCWindow.SetActive(false);
        SettingWindow.SetActive(false);
        CheckWindow.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape) && ESCClick == true)
         {
            ESCWindow.SetActive(true);
            ESCClick = !ESCClick;
         }
        else if(Input.GetKeyDown(KeyCode.Escape) && ESCClick == false)
        {
            ESCWindow.SetActive(false);
            ESCClick = !ESCClick;
        }
       
    }
   
    public void OpenESCWindow() // ���� ui�� ���������� ������ â �߰��ϱ�
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            ESCWindow.SetActive(true);
        }
        else
        {
            ESCWindow.SetActive(false);
        }
    }

    public void OpenSettingWindow()  // ESC Window ���� ���� ������ ������ â �߰��ϱ�
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            SettingWindow.SetActive(true);
        }
        else
        {
            SettingWindow.SetActive(false);
        }
    }

    public void OpenReCheckWindow() // �׸��ϱ� ��ư ������ �ٽ� ����� â �߰�
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            CheckWindow.SetActive(true);
            NoTouch.SetActive(true);
            Time = FindObjectOfType<TimeBtns>();
            Time.OnPause(); //â �߸� �Ͻ�����

        }
        else
        {
            CheckWindow.SetActive(false);
        }

    }
    public void SettingExit() //���� â ������
    {
        SettingWindow.SetActive(false);
    }

    public void ESCExit() //�����ϱ� â ������
    {
        ESCWindow.SetActive(false);
    }

    public void ClickNo()
    {
        CheckWindow.SetActive(false);
        Time.OnPlay();
        NoTouch.SetActive(false);
    }
}
