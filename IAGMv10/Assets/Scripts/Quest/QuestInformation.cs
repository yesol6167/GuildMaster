using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using UnityEngine.SceneManagement;

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

    GameObject WindowArea; // AdDataWindow�� �ڽ����� �����Ǵ� UiCanvas���� �θ� ������Ʈ

    AdDataWindow ADW;

    public void Start()
    {
            WindowArea = GameObject.Find("WindowArea");
            //UI�Ŵ���
            if (NpcChk == false) // Ui�������� ���
            {
                ParentOBJ = GameObject.Find("SpawnPointQ");
                ChildOBJ = ParentOBJ.transform.GetChild(0).gameObject;
            }
            else // ���谡�� ���
            {
                if (this.gameObject.GetComponent<Host>().purpose == 0) // �湮������ �κ��� ���
                {
                    ParentOBJ = GameObject.Find("SpawnPointQ");
                    ChildOBJ = ParentOBJ.transform.GetChild(0).gameObject;
                }
            }
            NewsBArea = GameObject.Find("NewsBArea");
        /*WindowArea = UIManager.Inst.WindowArea;
        //UI�Ŵ���
        if(NpcChk == false || this.gameObject.GetComponent<Host>().purpose == 0) // Ui�������� ���
        {
            ParentOBJ = UIManager.Inst.ParentOBJ;
            if (ParentOBJ.transform.childCount > 0)
            {
                ChildOBJ = ParentOBJ.transform.GetChild(0).gameObject;
            }
        }
        NewsBArea = UIManager.Inst.NewsBArea;*/

    }
    public void ShowQuest(Quest.QuestInfo npc, GameObject host)
    {
        grade.text = npc.questgrade.ToString();
        Name.text = "[" + npc.questname + "]";
        if (IsQuestchk)
        {
            //���谡 ���� ��
            int rnd = Random.Range(0, 10);
            if (host.GetComponent<QuestInformation>().myQuest.questname == "ä��" ? (rnd > 5) : host.GetComponent<Host>().myStat.HP != 0)
            {
                chk = true;
            }
            else
            {
                chk = false;
            }
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Tutorial2")
            {
                chk = true;
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
        GameManager.Instance.GetComponent<AudioSource>().Play();
        if (SpawnManager.Instance.EndTime < TimeManager.Instance.DeadLine)
        {
            if (People == 0) // �������
            {
                QuestManager.Instance.PostedQuest(myQuest);
                GameObject obj = Instantiate(Resources.Load("UiPrefabs/NewsBalloon"), NewsBArea.transform) as GameObject;
                obj.GetComponent<NewsBalloon>().SetText("���ο� ����Ʈ�� \n�߰��Ǿ����ϴ�.");
            }
            else // ���谡
            {
                QuestManager.Instance.ProgressQuest(myQuest, ChildOBJ);
                ChildOBJ.GetComponent<Host>().Questing = true;
            }
            ChildOBJ.GetComponent<Host>().onSmile = true;
        }
        else
        {
            if(People != 0)
            {
                QuestManager.Instance.PostedQuest(myQuest);
            }
            //�޽��� ���
            GameObject obj = Instantiate(Resources.Load("UiPrefabs/NewsBalloon"), NewsBArea.transform) as GameObject;
            obj.GetComponent<NewsBalloon>().SetText("�����ð����� \n���ο� ����Ʈ�� \n�߰��� �� �����ϴ�.");
            ChildOBJ.GetComponent<Host>().onAngry = true;
        }
    }

    public void onDestroy()
    {
        if (ADW != null)
        {
            Destroy(ADW.gameObject);
        }
        Destroy(gameObject);
    }

    public void onQuestRfuse()
    {
        GameManager.Instance.GetComponent<AudioSource>().Play();
        ChildOBJ.GetComponent<Host>().onAngry = true;
        if(People != 0)
        {
            QuestManager.Instance.PostedQuest(myQuest);
            GameObject obj = Instantiate(Resources.Load("UiPrefabs/NewsBalloon"), NewsBArea.transform) as GameObject;
            obj.GetComponent<NewsBalloon>().SetText("����Ʈ�� \n��ȯ�Ǿ����ϴ�.");
        }
    }
    public void AddReward()
    {
        ChildOBJ.GetComponent<Host>().Questing = false;
        Destroy(ChildOBJ.GetComponent<Host>().Quest.gameObject);
        QuestManager.Instance.EndQuest(myQuest);
        if (chk)
        { // ������ ���� ����
            //DataManager.Instance.GetComponent<AudioSource>().Play();
            GameManager.Instance.ChangeGold(myQuest.rewardgold);
            GameManager.Instance.ChangeFame(myQuest.rewardfame);
            ChildOBJ.GetComponent<Host>().onSmile = true;
        }
        else
        { // ���н� ���� ����
            GameManager.Instance.ChangeGold(-myQuest.rewardgold);
            if (SpawnManager.Instance.EndTime >= TimeManager.Instance.DeadLine)
            {
                GameManager.Instance.ChangeFame(-myQuest.rewardfame); // �����ð��� ����Ʈ �����ϰ� ���ƿԴµ� ���� �ȱ��� - ����(�ӽ÷� ��ħ)
            }
            ChildOBJ.GetComponent<Host>().onAngry = true;
        }
        //���� ����Ʈ ����Ʈ�� �Ϸ� ����Ʈ ����Ʈ�� �߰�
        Destroy(gameObject); // Ȯ�ν� ���� �� ������ ����

    }
    /*public void onNewsBalloon() // ������ǳ�� ���� "���ο� ����Ʈ�� �߰��Ǿ����ϴ�."
    {
        GameObject obj = Instantiate(Resources.Load("UiPrefabs/NewsBalloon"), NewsBArea.transform) as GameObject;
        obj.GetComponent<NewsBalloon>().SetText("���ο� ����Ʈ�� \n�߰��Ǿ����ϴ�.");
    }*/
    public void onAdDataWindow() // ���谡 ����â ����
    {
        GameManager.Instance.GetComponent<AudioSource>().Play();
        if (ADW == null)
        {
            GameObject obj = Instantiate(Resources.Load("UiPrefabs/AdDataWindow"), WindowArea.transform) as GameObject;
            ADW = obj.GetComponent<AdDataWindow>();
        }
    }
}
