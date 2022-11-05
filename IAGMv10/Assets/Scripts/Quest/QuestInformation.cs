using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;

public class QuestInformation : MonoBehaviour // �ش� ��ũ��Ʈ�� ���谡�� ����Ʈ ��û���� ���� ����
{
    public GameObject ParentOBJ;
    public GameObject ChildOBJ;
    public GameObject NewsBArea; // NewsBalloon�� �ڽ����� �����Ǵ� UiCanvas���� �θ� ������Ʈ

    public Quest.QuestInfo myQuest;
    public TMP_Text Questname;
    public TMP_Text grade;
    public TMP_Text Name;
    public TMP_Text info;
    public TMP_Text reward;
    public TMP_Text Result;
    //string result;
    bool chk; // ���� ���� ���� üũ
    public GameObject myNpc; // ����Ʈ�� �� npc
    public int People;
    public bool IsQuestchk;
    public bool NpcChk = true; // �ش� ��ũ��Ʈ�� ���ε� �Ǿ��ִ� ������Ʈ�� ���谡���� �ƴϸ� Ui������ ���� ���� (false=��û��/true=���谡)

    GameObject QuestWindowArea; // AdDataWindow�� �ڽ����� �����Ǵ� UiCanvas���� �θ� ������Ʈ

    public void Start()
    {
        QuestWindowArea = GameObject.Find("QuestWindowArea");
        //UI�Ŵ���
        if(NpcChk == false) // Ui�������� ���
        {
            ParentOBJ = GameObject.Find("SpawnPointQ");
            ChildOBJ = ParentOBJ.transform.GetChild(0).gameObject;
        }
        else // ���谡�� ���
        {
            if(this.gameObject.GetComponent<Host>().purpose == 0) // �湮������ �κ��� ���
            {
                ParentOBJ = GameObject.Find("SpawnPointQ");
                ChildOBJ = ParentOBJ.transform.GetChild(0).gameObject;
            }
        }

        NewsBArea = GameObject.Find("NewsBArea");
    }
    public void ShowQuest(Quest.QuestInfo npc)
    {
        grade.text = npc.questgrade.ToString();
        Name.text = "[" + npc.questname + "]";
        if (IsQuestchk)
        {
            //���谡 ���� ��
            int rnd = Random.Range(0, 10);
            if (rnd > 4)
            {
                chk = true;
            }
            else
            {
                chk = false;
            }
            //���� ��
            Result.text = chk ? "[����]" : "[����]"; // ���� ���� ���� üũ�� ����
        }
        else
        {
            //����Ʈ ��û��
            info.text = "\"" + npc.information + "\"";
        }
        reward.text = "[��� : " + npc.rewardgold.ToString() + "G]" + "\n" + "[���� : " + npc.rewardfame.ToString() + "P]";
    }


    public void AddQuest() // �³�
    {
        if (People == 0) // �������
        {
            QuestManager.Instance.PostedQuest(myQuest);
        }
        else // ���谡
        {
            QuestManager.Instance.ProgressQuest(myQuest, ChildOBJ);
            ChildOBJ.GetComponent<Host>().Questing = true;
        }
        ChildOBJ.GetComponent<Host>().onSmile = true;
    }

    public void onDestroy()
    {
        Destroy(gameObject);
    }

    public void onQuestRfuse()
    {
        ChildOBJ.GetComponent<Host>().onAngry = true;
    }
    public void AddReward()
    {
        ChildOBJ.GetComponent<Host>().Questing = false;
        Destroy(ChildOBJ.GetComponent<Host>().Quest.gameObject);
        QuestManager.Instance.EndQuest(myQuest);
        if (chk)
        { // ������ ���� ����
            GameManager.Instance.Gold += myQuest.rewardgold;
            GameManager.Instance.Fame += myQuest.rewardfame;
            UIManager.Instance.GoldiIncrease.GetComponent<TMP_Text>().text = $"<color=red>+{myQuest.rewardgold}"; //GoldiIncrease
            UIManager.Instance.ChangeGold = true;
            UIManager.Instance.FameIncrease.GetComponent<TMP_Text>().text = $"<color=red>+{myQuest.rewardfame}"; //FameiIncrease
            UIManager.Instance.ChangeFame = true;
            ChildOBJ.GetComponent<Host>().onSmile = true;
        }
        else
        { // ���н� ���� ����
            GameManager.Instance.Gold -= myQuest.rewardgold;
            GameManager.Instance.Fame -= myQuest.rewardfame;
            UIManager.Instance.GoldiIncrease.GetComponent<TMP_Text>().text = $"<color=blue>+{myQuest.rewardgold}"; //GoldiIncrease
            UIManager.Instance.ChangeGold = true;
            UIManager.Instance.FameIncrease.GetComponent<TMP_Text>().text = $"<color=blue>+{myQuest.rewardfame}"; //FameiIncrease
            UIManager.Instance.ChangeFame = true;
            ChildOBJ.GetComponent<Host>().onAngry = true;
        }
        //���� ����Ʈ ����Ʈ�� �Ϸ� ����Ʈ ����Ʈ�� �߰�
        Destroy(gameObject); // Ȯ�ν� ���� �� ������ ����

    }
    public void onNewsBalloon() // ������ǳ�� ���� "���ο� ����Ʈ�� �߰��Ǿ����ϴ�."
    {
        GameObject obj = Instantiate(Resources.Load("UiPrefabs/NewsBalloon"), NewsBArea.transform) as GameObject;
    }
    public void onAdDataWindow() // ���谡 ����â ����
    {
        GameObject obj = Instantiate(Resources.Load("UiPrefabs/AdDataWindow"), QuestWindowArea.transform) as GameObject;
    }
}
