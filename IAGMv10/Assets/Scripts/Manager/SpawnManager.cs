using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : Singleton<SpawnManager>
{
    public List<VLNpc.VLNPC> vlnpcs = new List<VLNpc.VLNPC>();
    public int hostCount;
    public int maxCount; // 길드안에 생성될수 있는 최대 캐릭터 수
    public float spawnTime; // 캐릭터 생성 주기
    float curTime;
    public float EndTime = 0.0f;
    public float DeadLine = 555.0f;
    public float Day = 600.0f;
    public Transform[] spawnPoints;
    public GameObject[] VL; // Villager(마을사람)
    public GameObject[] AD; // Adventeur(모험가)
    bool Hostchk;
    public int hcount = 0;
    public int ADcount = 0;
    GameObject Host;
    public VLNpc.VLNPC[] VLarray = new VLNpc.VLNPC[3];
    public int total;
    public bool first = true;
    bool deadlinechk = true;

    //몬스터
    public MonsterStat.GRADE grade;
    public GameObject monster;
    public GameObject[] monsters;
    public Transform[] BattlePoint;

    public bool BlockChk = false; // Npc 생성위치에 이미 생성된 Npc가 서있어서 생성을 막고있는지 check
    private void Update()
    {
        EndTime += Time.deltaTime;
        if (EndTime < DeadLine) {

            if (BlockChk == false) // Npc 생성위치에 아무런 방해물이 없어서 정상일 때
            {
                if (curTime >= spawnTime && hostCount < maxCount)
                {
                    Addvlnpc(); // 가중치 값 세팅
                    if (first)
                    {
                        for (int i = 0; i < vlnpcs.Count; i++)
                        {
                            total += vlnpcs[i].weight;
                        }
                        first = false;
                    }
                    SpawnHost(); // 콜라이더에서 불값을 전달하고 이프문에서 불값을 검사함
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
        if(EndTime >= Day)
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
        obj.GetComponent<NewsBalloon>().SetText("마감시간 입니다!");
    }

    public void SpawnHost()
    {
        int VLnum; // VLnum(마을사람 모델 넘버)는 5개의 모델 넘버 중 랜덤으로 결정됨 // 0~1 마을사람 2 귀족 3~4 왕족
        VLNpc.VLNPC vl;
        vl.NpcJob = RandomVLNPC().NpcJob; // 여기서 신분이 정해짐
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
        int ADnum = UnityEngine.Random.Range(0, 7); // ADnum(모험가 모델 넘버)는 7개의 모델 넘버 중 랜덤으로 결정됨

        
        curTime = 0;
        hostCount++;
        if ( hcount > 0 )
        {
            int Nnum = UnityEngine.Random.Range(0, 3);
            if (Nnum == 0) // 마을사람 생성
            { 
                // VL[0]을 npc[0]에 넣는다.
                Host = Instantiate(VL[VLnum], spawnPoints[0]) as GameObject; // 생성될때 VLnum를 확률에 따라 조정
                Host.GetComponent<VLNpc>().job = vl.NpcJob;
                Host.GetComponent<Host>().VLchk = true; // = 해당 캐릭터는 마을사람이다
                Host.GetComponent<Host>().purpose = 0; // = 방문 구역이 로비이다
            }
            else // 모험가 생성
            { 
                int Purpose = Random.Range(0, 2);
                Host = Instantiate(AD[ADnum], spawnPoints[Purpose]) as GameObject;
                Host.GetComponent<Host>().purpose = Purpose; // = 방문 구역이 로비(0)/펍(1)/모텔(2) 중에서 랜덤으로 주어진다.
                if (Host.GetComponent<Host>().purpose == 0) //모험가의 방문목적이 퀘스트 일때만 퀘스트를 부여함
                {
                    int j = UnityEngine.Random.Range(0, QuestManager.Instance.RQlist.Count); //RQ리스트에 있는 퀘스트 중에서 랜덤으로 모험가에게 부여
                    Host.GetComponent<QuestInformation>().myQuest = QuestManager.Instance.RQlist[j];
                    QuestManager.Instance.RQlist.RemoveAt(j); // RQ리스트 제거
                    Destroy(QuestManager.Instance.RQuest.transform.GetChild(j).gameObject); // RQ프리팹 삭제
                    hcount--; // ? 주현 : 해당 명령어는 왜 있는가?
                }
                Host.GetComponent<ADNpc>().adtype = ADnum;
                Host.GetComponent<Host>().VLchk = false; // = 해당 캐릭터는 모험가이다
                //Host.GetComponent<QuestInformation>().NpcChk = true;
            }
            Host.GetComponent<Host>().People = Nnum;
        }
        else // if(hcount = 0) 현재 길드내에 마을사람이 없으면(처음에) 모험가(방문목적:모텔/여관)와 마을사람이 50:50의 확률로 생성됨
        {
            int Num = UnityEngine.Random.Range(0, 2);
            switch(Num)
            {
                case 0: // 마을사람
                    GameObject Host = Instantiate(VL[VLnum], spawnPoints[0]) as GameObject; // 생성될때 VLnum를 확률에 따라 조정
                    Host.GetComponent<VLNpc>().job = vl.NpcJob;
                    Host.GetComponent<Host>().VLchk = true; // = 해당 캐릭터는 마을사람이다
                    Host.GetComponent<Host>().purpose = 0; // = 방문 구역이 로비이다
                    Host.GetComponent<Host>().People = 0;
                    break;
                case 1: // 모험가
                    int Purpose = Random.Range(0, 2); // 방문 목적이 펍(1) 또는 모텔(2)
                    Host = Instantiate(AD[ADnum], spawnPoints[Purpose]) as GameObject;
                    Host.GetComponent<ADNpc>().adtype = ADnum;
                    Host.GetComponent<Host>().purpose = Purpose; // = 방문 구역이 로비(0)/펍(1)/모텔(2) 중에서 랜덤으로 주어진다.
                    Host.GetComponent<Host>().VLchk = false; // = 해당 캐릭터는 모험가이다
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
    public void Addvlnpc() // 가중치 랜덤 값 세팅
    {
        //가중치 0이하로 X
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
    public VLNpc.VLNPC RandomVLNPC()//가중치 랜덤
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
            //꽉 찼을 때 예외처리 필요
            if (BattlePoint[i].transform.parent.GetComponent<SpawnChk>().chk == true)
            {
                monster.transform.position = BattlePoint[i].gameObject.transform.position;
                //빈자리인지 확인
                monster.transform.parent = BattlePoint[i].transform;
                break;
            }
            else
            {
                i++;
            }
        }
        monster.GetComponent<MonsterStat>().grade = grade;
    }
}