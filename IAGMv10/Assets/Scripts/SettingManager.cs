using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public GameObject SettingWindow; //ESC 누르면 나오는 창
    public GameObject ReCheckWindow; // 그만하기 누르면 나오는 창
    public GameObject DetailSettingWindow; // ESC Window 안의 설정 아이콘 누르면 나오는 설정 창
    public GameObject SaveLodeWindow;
    public GameObject NoTouch;
    public bool IsOpen = false;
    public bool ESCClick = false;
    TimeBtns Time;
   
    // Start is called before the first frame update
    void Start()
    {
        SettingWindow.SetActive(false);
        DetailSettingWindow.SetActive(false);
        ReCheckWindow.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape) && ESCClick == true)
         {
            SettingWindow.SetActive(true);
            ESCClick = !ESCClick;
         }
        else if(Input.GetKeyDown(KeyCode.Escape) && ESCClick == false)
        {
            SettingWindow.SetActive(false);
            ESCClick = !ESCClick;
        }
       
    }
   
    public void OpenS_Window() // 설정 창 뜨게하기
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

    public void OpenDS_Window()  // 세부설정 창 뜨게하기
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            DetailSettingWindow.SetActive(true);
        }
        else
        {
            DetailSettingWindow.SetActive(false);
        }
    }

    public void OpenReCheckWindow() // 그만하기 버튼 누르면 다시 물어보는 창 뜨게
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            ReCheckWindow.SetActive(true);
            NoTouch.SetActive(true);
            Time = FindObjectOfType<TimeBtns>();
            Time.OnPause(); //창 뜨면 일시정지

        }
        else
        {
            ReCheckWindow.SetActive(false);
        }
    }
    public void OpenSL_Window()  // 세부설정 창 뜨게하기
    {
        SaveLodeWindow.SetActive(true);
    }

    public void DS_WindowExit() //세부설정 창 나가기
    {
        DetailSettingWindow.SetActive(false);
    }

    public void S_WindowExit() //세팅 창 나가기
    {
        SettingWindow.SetActive(false);
    }

    public void SL_WindowExit()
    {
        SaveLodeWindow.SetActive(false);
    }

    public void ClickNo()
    {
        ReCheckWindow.SetActive(false);
        Time.OnPlay();
        NoTouch.SetActive(false);
    }
}
