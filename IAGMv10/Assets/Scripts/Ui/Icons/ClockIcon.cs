using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class ClockIcon : MonoBehaviour
{
    delegate void Delegate(); // �⺻ �Լ� ���� 
    delegate void DelegateS(string IconName,float WaitSeconds); // ���ڿ� �Լ� ����

    Delegate deStaffIcon;
    DelegateS deHostIcon;

    public GameObject myNotouch;

    public GameObject myHost;
    public Transform myIconZone;
    public Button myButton;

    public Image myTimeArea;
    public float LimitTime = 30.0f; // ���ѽð� 30��

    public AudioSource NotouchSound;

    // Start is called before the first frame update
    void Start()
    {
        deHostIcon = myHost.GetComponent<Host>().CorourineIcon;

        StartCoroutine(TimeChecking());
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(myIconZone.position);
        transform.position = pos;
    }
    IEnumerator TimeChecking()
    {
        myTimeArea.fillAmount = 0.0f;
        float speed = 1.0f / LimitTime;
        while (myTimeArea.fillAmount < 1.0f)
        {
            myTimeArea.fillAmount += speed * Time.deltaTime;
            if (myTimeArea.fillAmount == 1.0f)
            {
                myHost.GetComponent<Host>().onAngry = true;
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }
    public void OnRemveClock()
    {
        Destroy(gameObject);
    }

    public void OnIcon() 
    {
        //GameManager.Instance.GetComponent<AudioSource>().Play();
        switch(myHost.GetComponent<Host>().purpose)
        {
            case 0: // �湮 ������ �κ��϶�
                deHostIcon("QuestIcon",1.0f);
                break;
            case 1: // �湮 ������ �Ĵ��϶�
                deHostIcon("MeatIcon",1.0f);
                break;
            case 2: // �湮 ������ �����϶�
                deHostIcon("BedIcon",1.0f);
                break;
        }
    }
    public void OnStaffSmile()
    {
        deStaffIcon = myHost.GetComponent<Host>().myStaff.GetComponent<Staff>().OnSmileIcon;
        deStaffIcon();
    }

    public void NoTouchSound()
    {
        NotouchSound.Play();
    }
}