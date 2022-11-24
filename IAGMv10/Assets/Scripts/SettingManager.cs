using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingManager : MonoBehaviour
{
    public GameObject SettingWindow; //ESC ������ ������ â
    public GameObject ReCheckWindow; // �׸��ϱ� ������ ������ â
    public GameObject DetailSettingWindow; // ESC Window ���� ���� ������ ������ ������ ���� â
    public GameObject SaveLodeWindow;
    public GameObject NoTouch;
    public bool IsOpen = false;
    public bool ESCClick = false;
    TimeBtns Time;
   
    // Start is called before the first frame update
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "Tutorial2")
        {
            SettingWindow.SetActive(false);
            DetailSettingWindow.SetActive(false);
            ReCheckWindow.SetActive(false);
            DetailSettingWindow.GetComponent<SettingWindow>().BGMvolume.value = PlayerPrefs.GetFloat("volume");
            DetailSettingWindow.GetComponent<SettingWindow>().BGM.volume = PlayerPrefs.GetFloat("volume");
            DetailSettingWindow.GetComponent<SettingWindow>().Effvolume.value = PlayerPrefs.GetFloat("eff");
            foreach (AudioSource source in DetailSettingWindow.GetComponent<SettingWindow>().Eff)
            {
                source.volume = PlayerPrefs.GetFloat("eff");
            }
        }
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
   
    public void OpenS_Window() // ���� â �߰��ϱ�
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

    public void OpenDS_Window()  // ���μ��� â �߰��ϱ�
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            DetailSettingWindow.SetActive(true);
        }
        else
        {
            DetailSettingWindow.SetActive(false);
            Soundset();
        }
    }

    public void OpenReCheckWindow() // �׸��ϱ� ��ư ������ �ٽ� ����� â �߰�
    {
        IsOpen = !IsOpen;
        if (IsOpen)
        {
            ReCheckWindow.SetActive(true);
            NoTouch.SetActive(true);
            Time = FindObjectOfType<TimeBtns>();
            Time.OnPause(); //â �߸� �Ͻ�����

        }
        else
        {
            ReCheckWindow.SetActive(false);
        }
    }
    public void OpenSL_Window()  // ���μ��� â �߰��ϱ�
    {
        SaveLodeWindow.SetActive(true);
    }

    public void S_WindowExit() //���� â ������
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
    void Soundset()
    {
        DetailSettingWindow.GetComponent<SettingWindow>().BGM.volume = PlayerPrefs.GetFloat("volume");
        DetailSettingWindow.GetComponent<SettingWindow>().BGMvolume.value = PlayerPrefs.GetFloat("volume");
        foreach (AudioSource source in DetailSettingWindow.GetComponent<SettingWindow>().Eff)
        {
            source.volume = PlayerPrefs.GetFloat("eff");
        }
        DetailSettingWindow.GetComponent<SettingWindow>().Effvolume.value = PlayerPrefs.GetFloat("eff");
        DetailSettingWindow.transform.localPosition = DetailSettingWindow.GetComponent<SettingWindow>().orgPos;
    }
}
