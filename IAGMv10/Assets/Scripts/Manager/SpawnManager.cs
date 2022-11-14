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
    float curTime;
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

    [SerializeField]
    public GameObject BlockChkZone;

    new private void Awake()
    {

    }

    private void Update()
    {
        if(BlockChkZone.GetComponent<BlockCheckZone>().BlockChk == false) // Npc ������ġ�� �ƹ��� ���ع��� ��� ������ ��
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

        curTime += Time.deltaTime;
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
            int Nnum = UnityEngine.Random.Range(0, 3);
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
                int Purpose = Random.Range(0, 3);
                Host = Instantiate(AD[ADnum], spawnPoints[Purpose]) as GameObject;
                Host.GetComponent<Host>().purpose = Purpose; // = �湮 ������ �κ�(0)/��(1)/����(2) �߿��� �������� �־�����.
                if (Host.GetComponent<Host>().purpose == 0) //���谡�� �湮������ ����Ʈ �϶��� ����Ʈ�� �ο���
                {
                    int j = UnityEngine.Random.Range(0, QuestManager.Instance.RQlist.Count); //RQ����Ʈ�� �ִ� ����Ʈ �߿��� �������� ���谡���� �ο�
                    Host.GetComponent<QuestInformation>().myQuest = QuestManager.Instance.RQlist[j];
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
            int Num = UnityEngine.Random.Range(0, 2);
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
}