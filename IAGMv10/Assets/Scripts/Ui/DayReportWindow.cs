using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayReportWindow : MonoBehaviour
{
    public GameObject SaveLoadWindow;
    public GameObject myNotoch;
    public TMP_Text myClosedDay;
    public TMP_Text NowGoldText;
    public TMP_Text NowFameText;
    string _S;
    public int NowGold;
    public int NowFame;

    private void Update()
    {
        int S = TimeManager.Instance.SeasonCount;
        int M = TimeManager.Instance.MonthCount;
        int D = TimeManager.Instance.DayCount;
        
        switch(S)
        {
            case 1:
                _S = "��";
                break;
            case 2:
                _S = "����";
                break;
            case 3:
                _S = "����";
                break;
            case 4:
                _S = "�ܿ�";
                break;
        }
        myClosedDay.text = _S + $" {M}�� {D - 1}�� ��������";

        // ���� ���/����
        NowGold = GameManager.Instance.Gold;
        NowFame = GameManager.Instance.Fame;
        NowGoldText.text = "���� ��� : " + NowGold.ToString();
        NowFameText.text = "���� ���� : " + NowFame.ToString();
    }

    public void OnSaveLoadWindow()
    {
        myNotoch.SetActive(true);
        SaveLoadWindow.SetActive(true);
    }

    public void OnContinue()
    {
        TimeBtns.Instance.OnPlay();
        TimeManager.Instance.GuildClose = false;
        gameObject.SetActive(false);
        TimeManager.Instance.temp = TimeManager.Instance.OneDay;
        TimeManager.Instance.GetComponent<AudioSource>().Play();
    }
}
