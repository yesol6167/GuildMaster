using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;
using Unity.Jobs;
using UnityEngine.SearchService;
using static UnityEngine.GraphicsBuffer;
using System.Threading;
using UnityEngine.UI;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine.AI;

public class Host : MonoBehaviour
{
    GameObject Icon;

    public bool LineChk = false; // �����ϴ� Npc�� �����ϴ� Npc�� �浹�� �ð������ �����Ǵ� ���� ����
    public static Host inst = null;

    public GameObject Quest; // ������ ���� ���� >> ����Ʈ �Ϸ� >> �Ϸ�Ǿ����� quest �������� �ı�

    //NpcClock,QuestImoticon
    public Transform spawnPoints;
    public Transform myIconZone;
    public Transform OutPoint;
    //public GameObject obj = null;
    public GameObject objC = null;
    public GameObject myStaff;

    GameObject IconArea = null;
    public bool hostchk; // ��������� ���谡 ���п�
    public bool Firstchk = false;
    public bool IsFinishQuest = false; // ����Ʈ �Ϸ� ���� >> ������ �����
    public bool exitchk = false;
    public bool OnLine = false;
    public bool Clockchk = false;
    public bool IsQuest = false;
    int count; //�迭 üũ

    public bool onAngry = false;
    public bool onSmile = false;

    public int People; // ��� ����
    //Ai
    NavMeshAgent agent;
    NavMeshObstacle agent1; //�̰ɷ� ������ �������� ��ֹ��ǵ��� �ٲܲ���
    //spawnManager ���� ħ�� ���̺� �迭���� ������ @@@@@@@ �̰� ���� �ʹ� �����Ƽ� ���� ��ũ��Ʈ���� �ű⼭ �����ϰ����
    ChairBedChk bedchairvalue;
    //���� ������� ���Ͱ�
    [SerializeField] Vector3 res;
    [SerializeField] Vector3 inn;

    [SerializeField] Transform Exit;
    public int purpose;

    //FirstChecker

    public LayerMask layerMask;
   
    public enum STATE
    {
        Create, Idle, GoSide, Moving, Quest, Eat, Eating, Sleep, Sleeping, Exit
    }
    public STATE myState = default;
    
    public CharacterStat stat;
    
    //public CharState.NPC mystat; // ĳ���� �ɷ�ġ

    public Transform Line; //�ʱ� �� ���� �� ���� >> �տ� ��ǥ Ȯ�� >> X >> ���� ��ġ�� ���� >> �ݺ�

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Idle: // �κ񵥽�ũ�� �ͼ� ���̵���� ����
                StaffCheck();
                agent.enabled = false;//�׺�޽� ��Ȱ��ȭ
                agent1.enabled = true;//�׺� �ɽ�ŸŬ Ȱ��ȭ //�̰ɷ� ������ �������� ��ֹ��ǵ��� �ٲܲ���
                StopAllCoroutines(); // ��� �ڷ�ƾ ���߰� ���
                StartCoroutine(deskLine());
                //NpcClock�� ����
                if (!Clockchk)
                {
                    if (LineChk == true)
                    {
                        IconArea = GameObject.Find("IconArea");
                        objC = Instantiate(Resources.Load("IconPrefabs/ClockIcon"), IconArea.transform) as GameObject;
                        objC.GetComponent<ClockIcon>().myTarget = myIconZone;
                        objC.GetComponent<ClockIcon>().myHost = this.gameObject;
                        Clockchk = true;
                    }
                }
                break;
            case STATE.Moving: // ����
                StopAllCoroutines();
                agent1.enabled = false; //�׺� �ɽ�ŸŬ ��Ȱ��ȭ
                agent.enabled = true; //�׺� �׺�޽� Ȱ��ȭ
                agent.ResetPath();
                switch (purpose)
                {
                    case 0:
                        ChangeState(STATE.Quest);
                        break;
                    case 1:
                        OnLine = true;
                        GetComponent<Animator>().SetBool("IsMoving", true);
                        agent.SetDestination(res);
                        StartCoroutine(deskLine());
                        StartCoroutine(WalkTo(STATE.Eat));
                        break;
                    case 2:
                        OnLine = true;
                        GetComponent<Animator>().SetBool("IsMoving", true);
                        agent.SetDestination(inn);
                        StartCoroutine(deskLine());
                        StartCoroutine(WalkTo(STATE.Sleep)); //�����ϸ�
                        break;
                }
                break;
            case STATE.Quest: //����ũ���� �̵��ϴ� ����
                StartCoroutine(deskLine());
                StartCoroutine(deskmoving());
                break;
            case STATE.Eat:
                StaffCheck();
                transform.rotation = Quaternion.Euler(0, 180, 0);
                StopAllCoroutines();
                GetComponent<Animator>().SetBool("IsMoving", false);
                IconArea = GameObject.Find("IconArea");

                if (!Clockchk)
                {
                        IconArea = GameObject.Find("IconArea");
                        objC = Instantiate(Resources.Load("IconPrefabs/ClockIcon"), IconArea.transform) as GameObject;
                        objC.GetComponent<ClockIcon>().myTarget = myIconZone;
                        objC.GetComponent<ClockIcon>().myHost = this.gameObject;
                        Clockchk = true;
                }

                //OnIcon("MealIcon");
                break;
            case STATE.Eating:
                IconArea = GameObject.Find("IconArea");
                OnIcon("EatIcon");

                StopAllCoroutines();
                GetComponent<Animator>().SetTrigger("IsSeating");
                Invoke("GoExit", 5);
                //Invoke("SeatToWalk",11);
                break;
            case STATE.Sleep:
                StaffCheck();
                transform.rotation = Quaternion.Euler(0, 180, 0);
                StopAllCoroutines();
                GetComponent<Animator>().SetBool("IsMoving", false);
                IconArea = GameObject.Find("IconArea");

                if (!Clockchk)
                {
                    IconArea = GameObject.Find("IconArea");
                    objC = Instantiate(Resources.Load("IconPrefabs/ClockIcon"), IconArea.transform) as GameObject;
                    objC.GetComponent<ClockIcon>().myTarget = myIconZone;
                    objC.GetComponent<ClockIcon>().myHost = this.gameObject;
                    Clockchk = true;
                }
                
                break;

            case STATE.Sleeping:
                IconArea = GameObject.Find("IconArea");
                OnIcon("SleepIcon");

                StopAllCoroutines();
                GetComponent<Animator>().SetTrigger("IsLaying");
                Invoke("GoExit", 5);
                break;
            case STATE.Exit:
                if (purpose != 0) // �湮������ ����or�Ĵ� �̶��
                {
                    if(onAngry == false)
                    {
                        Destroy(Icon);
                    }
                }
                StopAllCoroutines();
                GetComponent<Animator>().SetBool("IsMoving", true);
                if(onAngry == false) // ȭ�� �ȳ��� ��
                {
                    switch (purpose)
                    {
                        case 0:
                            OnLine = false;
                            agent1.enabled = false;
                            agent.enabled = true;
                            break;
                        case 1:
                            bedchairvalue._chairSlot[count] = ChairBedChk.ChairSlot.None;
                            GetComponent<Animator>().SetBool("SitToStand", true);
                            agent1.enabled = false;
                            agent.enabled = true;
                            agent.ResetPath();
                            break;
                        case 2:
                            bedchairvalue._bedSlot[count] = ChairBedChk.BedSlot.None;
                            GetComponent<Animator>().SetBool("LayToStand", true);
                            agent1.enabled = false;
                            agent.enabled = true;
                            agent.ResetPath();
                            break;
                    }
                }
                else // ȭ�� ���� ��
                {
                    StartCoAi();
                }

                agent.SetDestination(Exit.position); // outpoint�� ���� �ڷ�ƾ
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Idle:
                if (onAngry == true) // ����Ʈ�� �����ϸ� �Ҹ� ������ ����
                {
                    StartCoroutine(onAngryIcon());
                    onAngry = false;
                }
                else if (onSmile == true)
                {
                    StartCoroutine(onSmileIcon());
                    onSmile = false;
                }
                break;
            case STATE.Quest:
                if (IsQuest) transform.SetAsFirstSibling();
                break;
            case STATE.Eat:
                if(onAngry == true)
                {
                    ChangeState(STATE.Exit);
                }
                break;
            case STATE.Eating:
                break;
            case STATE.Sleep:
                if (onAngry == true)
                {
                    ChangeState(STATE.Exit);
                }
                break;
            case STATE.Sleeping:
                break;
            case STATE.Exit:
                transform.SetAsLastSibling();
                break;
        }
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent1 = GetComponent<NavMeshObstacle>();

        bedchairvalue = FindObjectOfType<ChairBedChk>();//GetComponent<SpawnManager>();
        inst = this;
    }
    void Start()
    {
        ChangeState(STATE.Moving);
    }
    void Update()
    {
        StateProcess();
    }
    public void GoBed() //�ֹ��ϰ� ħ��� �̵�
    {
        for (count = 0; count < bedchairvalue._bedSlot.Count; count++)
        {
            if (bedchairvalue._bedSlot[count] == ChairBedChk.BedSlot.None)
            {
                GetComponent<Animator>().SetBool("IsMoving", true);
                agent.SetDestination(bedchairvalue._gobed[count]);
                StartCoroutine(BedToSleeping());
                bedchairvalue.bedSlot[count] = ChairBedChk.BedSlot.Check;
                break;
            }
        }
    }
    public void GoTable() //�ֹ��ϰ� ���̺�� �̵�
    {
        for (count = 0; count <bedchairvalue._chairSlot.Count; count++)
        {
            if (bedchairvalue._chairSlot[count] == ChairBedChk.ChairSlot.None)
            {
                GetComponent<Animator>().SetBool("IsMoving", true);
                agent.SetDestination(bedchairvalue._gotable[count]);
                StartCoroutine(EatToEating());
                bedchairvalue._chairSlot[count] = ChairBedChk.ChairSlot.Check;
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 7.0f, 7.0f);
    }
    IEnumerator deskLine()// �ټ����
    {
        while (true)
        {
            if (OnLine)
            {
                if (Physics.SphereCast(transform.position, 7.0f, transform.forward , out RaycastHit hitinfo, 7.0f, layerMask))
                {
                    if (hitinfo.collider.gameObject.layer == 6) // layer 6 = Host, layer 3 = Staff // Host�� �ɷ��� ���
                    {
                        if (hitinfo.collider.GetComponent<Host>().LineChk == true) //  �ټ��ִ� ����� �ε����� ��� 
                        {
                            LineChk = true;
                        }
                        GetComponent<Animator>().SetBool("IsMoving", false);
                        if (purpose == 1 || purpose == 2)
                        {
                            agent.velocity = Vector3.zero;
                        }
                        ChangeState(STATE.Idle);
                    }
                    else if(hitinfo.collider.gameObject.layer == 3) // Staff�� �ɷ��� ���
                    {
                        if(purpose == 0) // �κ��� ���
                        {
                            GetComponent<Animator>().SetBool("IsMoving", false);
                            ChangeState(STATE.Idle);
                        }
                    } 
                }
                else
                {
                    if(purpose != 0)
                    {
                        ChangeState(STATE.Moving);
                    }
                    else if(purpose == 0)
                    {
                        ChangeState(STATE.Quest);
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator EatToEating() //���������� �����ϸ� Eat���¿��� Eating���·�
    {
        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= 1.0f)
                {
                    agent.velocity = Vector3.zero;
                    ChangeState(STATE.Eating);
                    agent.ResetPath();
                    if (count % 2 == 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 270, 0);
                    }
                    else if (count % 2 == 1)
                    {
                        transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator deskmoving()
    {
        OnLine = true;
        Vector3 dir = Line.position - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        GetComponent<Animator>().SetBool("IsMoving", true);
        while (dist > 0.0f)
        {
            float delta = stat.moveSpeed * Time.deltaTime;

            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
    }
    IEnumerator BedToSleeping() //���������� �����ϸ� Eat���¿��� Eating���·�
    {
        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= 1.0f)
                {
                    agent.velocity = Vector3.zero;
                    ChangeState(STATE.Sleeping);
                    agent.ResetPath();
                    transform.rotation = Quaternion.Euler(0, 270, 0);
                    if (count > 3)
                    {
                        transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator WalkTo(STATE HostState) // �����ϸ� Walk���¿��� Eat�̳� ���� ���·�
    {
        while (true)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance == 0)//�����Ÿ���0�̸�
                {
                    agent.velocity = Vector3.zero;
                    ChangeState(HostState);
                    agent.ResetPath();
                }
            }
            yield return null;
        }
    }

    IEnumerator onQuestIcon()
    {
        yield return new WaitForSeconds(1.5f);

        OnIcon("QuestIcon");
    }

    IEnumerator onAngryIcon()
    {
        yield return new WaitForSeconds(1.0f);

        OnIcon("AngryIcon");

        IsFinishQuest = true; // �Ҹ��� �����Բ�
        StopAllCoroutines();
        IsQuest = false;
        ChangeState(STATE.Exit);
    }

    IEnumerator onSmileIcon() // ������ ������
    {
        yield return new WaitForSeconds(1.0f);

        OnIcon("SmileIcon");

        if (hostchk)
        {
            IsFinishQuest = true;
        }
        StopCoroutine(deskLine());
        IsQuest = false;
        ChangeState(STATE.Exit);
    }

    IEnumerator onMandBIcon(GameObject Icon,string IconName) // �Ļ� & ħ�� ������
    {
        yield return new WaitForSeconds(1.0f);

        OnIcon(IconName);
    }

    IEnumerator FinishQuest(float t) //����Ʈ ���� ���� ����Ʈ�� �ϵ���
    {
        yield return new WaitForSeconds(t);
        SpawnManager.Instance.Teleport(gameObject);
        IsFinishQuest = true;
        OnLine = false;
        Clockchk = false;
        ChangeState(STATE.Quest);
    }
    public void GoExit() //�Դ»��¿��� ������ ���·�
    {
        ChangeState(STATE.Exit);
        IsFinishQuest = true;
    }

    public void StartFinishQuest()
    {
        StartCoroutine(FinishQuest(3.0f));
    }

    public void StartCoQi() // = StartCoroutineQuestIcon
    {
        StartCoroutine(onQuestIcon());
    }

    public void StartCoAi() // = StartCoroutineAngryIcon
    {
        StartCoroutine(onAngryIcon());
    }

    public void StartCoMandB(string IconName)
    {
        StartCoroutine(onMandBIcon(Icon,IconName));
    }

    public void OnIcon(string IconName)
    {
        Icon = Instantiate(Resources.Load($"IconPrefabs/{IconName}"), IconArea.transform) as GameObject;

        switch (IconName)
        {
            case "SmileIcon":
                Icon.GetComponent<MoodIcon>().myIconZone = myIconZone;
                break;
            case "AngryIcon":
                Icon.GetComponent<MoodIcon>().myIconZone = myIconZone;
                break;
            case "QuestIcon":
                Icon.GetComponent<QuestIcon>().myIconZone = myIconZone;
                break;
            case "BedIcon":
                Icon.GetComponent<PubMotelIcon>().myIconZone = myIconZone;
                Icon.GetComponent<PubMotelIcon>().myHost = this.gameObject;
                Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GoBed);
                break;
            case "MeatIcon":
                Icon.GetComponent<PubMotelIcon>().myIconZone = myIconZone;
                Icon.GetComponent<PubMotelIcon>().myHost = this.gameObject;
                Icon.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(GoTable);
                break;
            case "SleepIcon":
                Icon.GetComponent<PubMotelIcon>().myIconZone = myIconZone;
                break;
            case "EatIcon":
                Icon.GetComponent<PubMotelIcon>().myIconZone = myIconZone;
                break;
        }
    }
    public void StaffCheck() // Host�� �տ� ���ִ� Staff ��� ������ Staff���� �����ϴ� �Լ�
    {
        if (Physics.SphereCast(transform.position, 7.0f, transform.forward, out RaycastHit hitinfo, 7.0f, layerMask))
        {
            myStaff = hitinfo.collider.gameObject;
        }
    }
}