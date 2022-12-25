using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : Singleton<SpawnManager>
{
    
    public List<VLNpc.VLNPC> vlnpcs = new List<VLNpc.VLNPC>();
    public int hostCount;
    public int maxCount; // ���ȿ� �����ɼ� �ִ� �ִ� ĳ���� ��
    public float spawnTime; // ĳ���� ���� �ֱ�
    public float curTime;
    public float EndTime = 0.0f;
    public Transform[] spawnPoints;
    public GameObject[] VL; // Villager(�������)
    public GameObject[] AD; // Adventeur(���谡)
    bool Hostchk;
    public int hcount = 0;
    public int ADcount = 0;
    public GameObject Host;
    public VLNpc.VLNPC[] VLarray = new VLNpc.VLNPC[3];
    public int total;
    public bool first = true;
    bool deadlinechk = true;

    //����
    public MonsterStat.GRADE grade;
    public GameObject monster;
    public GameObject[] monsters;
    public Transform[] BattlePoint;

    //�ڷ���Ʈ ����Ʈ
    public GameObject TP;

    public bool BlockChk = false; // Npc ������ġ�� �̹� ������ Npc�� ���־ ������ �����ִ��� check
    private void Update()
    {
        EndTime += Time.deltaTime;
        if (EndTime < TimeManager.Instance.DeadLine) {

            if (BlockChk == false) // Npc ������ġ�� �ƹ��� ���ع��� ��� ������ ��
            {
                if (curTime >= spawnTime && hostCount < maxCount)
                {
                    Addvlnpc(); // ����ġ �� ����
                    if (first)
                    {
                        for (int i = 0; i < vlnpcs.Count; i++)
                        {
                            total += vlnpcs[i].weight;
                        }
                        first = false;
                    }
                    SpawnHost(); // �ݶ��̴����� �Ұ��� �����ϰ� ���������� �Ұ��� �˻���
                }
            }
        }
        else
        {
            if (deadlinechk == true)
            {
                InvokeRepeating("DeadLineMessage", 0.0f, 10.0f);
                deadlinechk = false;
            }
        }

        curTime += Time.deltaTime;
        if(EndTime >= TimeManager.Instance.OneDay)
        {
            QuestManager.Instance.EndQuestClear();
            EndTime = 0.0f;
            curTime = 0.0f;
            CancelInvoke("DeadLineMessage");
            deadlinechk = true;
        }
    }

    void DeadLineMessage()
    {
        GameObject obj = Instantiate(Resources.Load("UiPrefabs/NewsBalloon"), UIManager.Inst.NewsBArea.transform) as GameObject;
        obj.GetComponent<NewsBalloon>().SetText("�����ð� �Դϴ�!");
    }

    public void SpawnHost()
    {
        int VLnum; // VLnum(������� �� �ѹ�)�� 5���� �� �ѹ� �� �������� ������ // 0~1 ������� 2 ���� 3~4 ����
        VLNpc.VLNPC vl;
        vl.NpcJob = RandomVLNPC().NpcJob; // ���⼭ �ź��� ������
        //vl.NpcJob = VLNpc.NPCJOB.NOBILLITY;

        if (vl.NpcJob == VLNpc.NPCJOB.COMMONS)
        {  // 0~1
            VLnum = UnityEngine.Random.Range(0, 2);
        }
        else if (vl.NpcJob == VLNpc.NPCJOB.NOBILLITY)
        {
            VLnum = 2;
        }
        else
        {
            VLnum = UnityEngine.Random.Range(3, 5);
        }
        int ADnum = UnityEngine.Random.Range(0, 7); // ADnum(���谡 �� �ѹ�)�� 7���� �� �ѹ� �� �������� ������

        
        curTime = 0;
        hostCount++;
        if ( hcount > 0 )
        {
            int Nnum = UnityEngine.Random.Range(0, 2); // ����
            if (Nnum == 0) // ������� ����
            { 
                // VL[0]�� npc[0]�� �ִ´�.
                Host = Instantiate(VL[VLnum], spawnPoints[0]) as GameObject; // �����ɶ� VLnum�� Ȯ���� ���� ����
                Host.GetComponent<VLNpc>().job = vl.NpcJob;
                Host.GetComponent<Host>().VLchk = true; // = �ش� ĳ���ʹ� ��������̴�
                Host.GetComponent<Host>().purpose = 0; // = �湮 ������ �κ��̴�
            }
            else // ���谡 ����
            {
                int Purpose = TP.GetComponent<TeleportPoint>().AllChk() ? Random.Range(0, 3) : Random.Range(1, 3); // ����Ʈ ���� ���� á�� ��
                Host = Instantiate(AD[ADnum], spawnPoints[Purpose]) as GameObject;
                Host.GetComponent<Host>().purpose = Purpose; // = �湮 ������ �κ�(0)/��(1)/����(2) �߿��� �������� �־�����.
                if (Host.GetComponent<Host>().purpose == 0) //���谡�� �湮������ ����Ʈ �϶��� ����Ʈ�� �ο���
                {
                    int j;
                    do
                    {
                        j = UnityEngine.Random.Range(0, QuestManager.Instance.RQlist.Count); //RQ����Ʈ�� �ִ� ����Ʈ �߿��� �������� ���谡���� �ο�
                        Host.GetComponent<QuestInformation>().myQuest = QuestManager.Instance.RQlist[j];
                        if (Host.GetComponent<QuestInformation>().myQuest.questname == "ä��")
                        {
                            if (TP.GetComponent<TeleportPoint>().Quest2chk())
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (TP.GetComponent<TeleportPoint>().Quest1chk())
                            {
                                break;
                            }
                        }
                    } while (true);
                    QuestManager.Instance.RQlist.RemoveAt(j); // RQ����Ʈ ����
                    Destroy(QuestManager.Instance.RQuest.transform.GetChild(j).gameObject); // RQ������ ����
                    hcount--; // ? ���� : �ش� ��ɾ�� �� �ִ°�?
                }
                Host.GetComponent<ADNpc>().adtype = ADnum;
                Host.GetComponent<Host>().VLchk = false; // = �ش� ĳ���ʹ� ���谡�̴�
                //Host.GetComponent<QuestInformation>().NpcChk = true;
            }
            Host.GetComponent<Host>().People = Nnum;
        }
        else // if(hcount = 0) ���� ��峻�� ��������� ������(ó����) ���谡(�湮����:����/����)�� ��������� 50:50�� Ȯ���� ������
        {
            int Num = UnityEngine.Random.Range(0, 2); // ����
            switch(Num)
            {
                case 0: // �������
                    GameObject Host = Instantiate(VL[VLnum], spawnPoints[0]) as GameObject; // �����ɶ� VLnum�� Ȯ���� ���� ����
                    Host.GetComponent<VLNpc>().job = vl.NpcJob;
                    Host.GetComponent<Host>().VLchk = true; // = �ش� ĳ���ʹ� ��������̴�
                    Host.GetComponent<Host>().purpose = 0; // = �湮 ������ �κ��̴�
                    Host.GetComponent<Host>().People = 0;
                    break;
                case 1: // ���谡
                    int Purpose = Random.Range(1, 3); // �湮 ������ ��(1) �Ǵ� ����(2)
                    Host = Instantiate(AD[ADnum], spawnPoints[Purpose]) as GameObject;
                    Host.GetComponent<ADNpc>().adtype = ADnum;
                    Host.GetComponent<Host>().purpose = Purpose; // = �湮 ������ �κ�(0)/��(1)/����(2) �߿��� �������� �־�����.
                    Host.GetComponent<Host>().VLchk = false; // = �ش� ĳ���ʹ� ���谡�̴�
                    Host.GetComponent<Host>().People = 1;
                    //Host.GetComponent<QuestInformation>().NpcChk = true;
                    break;
            }
        }
    }
    public void Teleport(GameObject host)
    {
        host.GetComponent<NavMeshAgent>().Warp(spawnPoints[0].transform.position);
        host.transform.parent = spawnPoints[0].transform;
    }
    public void Addvlnpc() // ����ġ ���� �� ����
    {
        //����ġ 0���Ϸ� X
        if (GameManager.Instance.Fame >= 0)
        {
            VLarray[0].NpcJob = VLNpc.NPCJOB.COMMONS;
            VLarray[1].NpcJob = VLNpc.NPCJOB.NOBILLITY;
            VLarray[2].NpcJob = VLNpc.NPCJOB.ROYALTY;

            VLarray[0].weight = 100;
            VLarray[1].weight = 0;

            for (int i = 0; i < (GameManager.Instance.Fame / 100); i++)
            {
                if (VLarray[0].weight != 0)
                {
                    VLarray[0].weight -= 5;
                    VLarray[1].weight += ((VLarray[1].weight >= 20) ? 4 : 5);
                }
                else
                {
                    if (VLarray[1].weight != 0)
                    {
                        VLarray[1].weight -= 4;
                    }
                    else if (VLarray[1].weight == 0)
                    {
                        break;
                    }
                }
            }
            VLarray[2].weight = 100 - (VLarray[0].weight + VLarray[1].weight);
        }
        vlnpcs.Clear();
        for (int i = 0; i < 3; i++)
        {
            vlnpcs.Add(VLarray[i]);
        }
    }
    public VLNpc.VLNPC RandomVLNPC()//����ġ ����
    {
        int weight = 0;
        int selectNum = 0;

        selectNum = Mathf.RoundToInt(total * UnityEngine.Random.Range(0.0f, 1.0f));
        //Debug.Log(selectNum);
        for (int i = 0; i < vlnpcs.Count; i++)
        {
            weight += vlnpcs[i].weight;
            //Debug.Log(weight);
            if (selectNum <= weight)
            {
                VLNpc.VLNPC temp = new VLNpc.VLNPC(vlnpcs[i]);
                return temp;
            }
        }
        return default;
    }
    public void MonsterSpawn()
    {
        monster = Instantiate(monsters[Random.Range(0, 3)]) as GameObject;
        for (int i = 0; i < BattlePoint.Length;)
        {
            //�� á�� �� ����ó�� �ʿ�
            if (BattlePoint[i].transform.parent.GetComponent<SpawnChk>().chk == true)
            {
                monster.transform.position = BattlePoint[i].gameObject.transform.position;
                //���ڸ����� Ȯ��
                monster.transform.parent = BattlePoint[i].transform;
                break;
            }
            else
            {
                i++;
            }
        }
        if (monster.GetComponent<MonsterStat>())
        {
            monster.GetComponent<MonsterStat>().grade = grade;
        }
    }
}