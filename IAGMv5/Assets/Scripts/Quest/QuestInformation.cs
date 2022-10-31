using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestInformation : MonoBehaviour
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
    string result;
    bool chk; // ���� ���� ���� üũ
    public GameObject myNpc; // ����Ʈ�� �� npc
    public int People;
    public bool IsQuestchk;

    GameObject QuestWindowArea; // AdDataWindow�� �ڽ����� �����Ǵ� UiCanvas���� �θ� ������Ʈ

    public void Start()
    {
        QuestWindowArea = GameObject.Find("QuestWindowArea");
        //UI�Ŵ���
        ParentOBJ = GameObject.Find("SpawnPointQ");
        /*
        switch (this.gameObject.GetComponent<Host>().purpose)
        {
            case 0:
                ParentOBJ = GameObject.Find("SpawnPointQ");
                break;
            case 1:
                ParentOBJ = GameObject.Find("SpawnPointP");
                break;
            case 2:
                ParentOBJ = GameObject.Find("SpawnPointM");
                break;
        }
        */
        ChildOBJ = ParentOBJ.transform.GetChild(0).gameObject;

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
        if (People == 0)
        {
            //�������
            QuestManager.Instance.PostedQuest(myQuest);
        }
        else
        {
            //'�κ� �̿��ϴ�' ���谡 // ���� ���̳� ������ �̿��ϴ� ���谡�� ����Ʈ�� ������ -> ���� �ʿ�
            QuestManager.Instance.ProgressQuest(myQuest, ChildOBJ);
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
        Destroy(ChildOBJ.GetComponent<Host>().Quest.gameObject);
        QuestManager.Instance.EndQuest(myQuest);
        if (chk)
        { // ������ ���� ����
            GameManager.Instance.Gold += myQuest.rewardgold;
            GameManager.Instance.Fame += myQuest.rewardfame;
            ChildOBJ.GetComponent<Host>().onSmile = true;
        }
        else
        { // ���н� ���� ����
            GameManager.Instance.Gold -= myQuest.rewardgold;
            GameManager.Instance.Fame -= myQuest.rewardfame;
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
