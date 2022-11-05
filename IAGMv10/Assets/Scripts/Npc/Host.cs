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
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

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
    public GameObject myStaff;

    public GameObject IconArea = null;
    public bool VLchk; // ��������� ���谡 ���п�
    public bool Questing = false; // ���� ����Ʈ ������ = ��湮 �ؾ��ϴ� Npc
    public bool exitchk = false;
    public bool Clockchk = false;
    public bool IsQuest = false;
    int count; //�迭 üũ

    public bool onAngry = false;
    public bool onSmile = false;

    public int People; // ��� ����

    //�׺���̼�
    NavMeshAgent agent;
    [SerializeField] Vector3 lob; // �κ� ��������
    [SerializeField] Vector3 res; // �Ĵ� ��������
    [SerializeField] Vector3 mot; // ���� ��������

    [SerializeField] Transform Exit;

    ChairBedChk bedchairvalue;
    public int purpose; // �湮����
    public LayerMask layerMask;
   
    public enum STATE
    {
        Moving, Wait, Order, Eating, Sleeping, Exit
    }
    public STATE myState = default;
    public CharacterStat stat;

    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Moving: // ����
                StopAllCoroutines();
                agent.ResetPath(); // Wait ���¿��� Moving���� �ٲ� �������� �ʱ�ȭ
                GetComponent<Animator>().SetBool("IsMoving", true);
                switch (purpose)
                {
                    case 0:
                        agent.SetDestination(lob);
                        break;
                    case 1:
                        agent.SetDestination(res);
                        break;
                    case 2:
                        agent.SetDestination(mot);
                        break;
                }
                StartCoroutine(ForwardCheck());
                break;
            case STATE.Wait:
                StopAllCoroutines(); // ��� �ڷ�ƾ ���߰� ���
                agent.ResetPath();
                //�ð� ����
                if (LineChk == true && Clockchk == false) // �� ���ִ� ���� + �̹� ������ �ð谡 ���ٸ�
                {
                    IconArea = GameObject.Find("IconArea");
                    OnIcon("ClockIcon");
                    Clockchk = true;
                }
                StartCoroutine(ForwardCheck());
                break;
            case STATE.Order:
                StopAllCoroutines();
                LineChk = true; // �� ���ִ� ����
                if (Clockchk == false) // Wait ���� �ٷ� Order�� �����ߴٸ� �ð踦 ���� = �̹� ������ �ð谡 ���ٸ�
                {
                    IconArea = GameObject.Find("IconArea");
                    OnIcon("ClockIcon");
                }
                Destroy(Icon.GetComponent<ClockIcon>().myNotouch); // �ð� ����ġ ��Ȱ��ȭ
                break;
            case STATE.Eating:
                StopAllCoroutines();
                OnIcon("EatIcon");
                GetComponent<Animator>().SetTrigger("IsSeating");
                Invoke("GoExit", 5);
                break;
            case STATE.Sleeping:
                StopAllCoroutines();
                OnIcon("SleepIcon");
                GetComponent<Animator>().SetTrigger("IsLaying");
                Invoke("GoExit", 5);
                break;
            case STATE.Exit:
                StopAllCoroutines();
                LineChk = false; // �� ���ִ� ���°� �ƴ�
                // �����ϴ� ȣ��Ʈ�� �����ϴ� ȣ��Ʈ�� ���ذ� (�켱���� ����)
                agent.avoidancePriority = 51;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

                GetComponent<Animator>().SetBool("IsMoving", true);

                if (purpose != 0) // �湮������ ����or�Ĵ� �̶��
                {
                    Destroy(Icon); // �� �Ǵ� �Ļ� ������ ����
                }

                if(onAngry == false) // �ֹ��� �޾� �鿩���� ��(ȭ�� �ȳ��� ��)
                {
                    agent.ResetPath(); // �׺� �ʱ�ȭ

                    switch (purpose)
                    {
                        case 0:
                            break;
                        case 1: //ħ�� �ʱ�ȭ
                            bedchairvalue._chairSlot[count] = ChairBedChk.ChairSlot.None;
                            GetComponent<Animator>().SetBool("SitToStand", true);
                            break;
                        case 2: //���� �ʱ�ȭ
                            bedchairvalue._bedSlot[count] = ChairBedChk.BedSlot.None;
                            GetComponent<Animator>().SetBool("LayToStand", true);
                            break;
                    }
                }
                else // ȭ�� �� ���¶��
                {
                    OnIcon("AngryIcon");
                }

                if(onSmile == true)
                {
                    OnIcon("SmileIcon");
                }

                agent.SetDestination(Exit.position); // outpoint�� ���� �ڷ�ƾ
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Moving:
                if(purpose == 0)
                {
                    if (IsQuest) transform.SetAsFirstSibling();
                }
                break;
            case STATE.Wait:
                if (onAngry == true)
                {
                    ChangeState(STATE.Exit);
                }
                break;
            case STATE.Order:
                if(onAngry == true || onSmile == true)
                {
                    ChangeState(STATE.Exit);
                }
                break;
            case STATE.Exit:
                transform.SetAsLastSibling();
                break;
        }
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        bedchairvalue = FindObjectOfType<ChairBedChk>();
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
    IEnumerator ForwardCheck()// �տ� �����ִ��� �����Ͽ� ���¸� Wait �Ǵ� Order�� �ٲ���
    {
        while (true)
        {
            if (Physics.SphereCast(transform.position, 7.0f, transform.forward, out RaycastHit hitinfo, 7.0f, layerMask)) // �� �տ� ������ ���� ���
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero; //���ӵ� = 0
                GetComponent<Animator>().SetBool("IsMoving", false); // �ȴ� �ִ� ����

                if (hitinfo.collider.gameObject.layer == 6) // layer 6 = Host, layer 3 = Staff // �տ� Host�� ���� ���
                {
                    if(hitinfo.collider.gameObject.GetComponent<Host>().LineChk == true)
                    {
                        LineChk = true;
                    }
                    ChangeState(STATE.Wait);
                }
                else // �տ� Staff�� ���� ���
                {
                    myStaff = hitinfo.collider.gameObject; // myStaff�� �� ������ �°� �������� ( ������ ������ �ߵ� ��Ű�� ���� )
                    ChangeState(STATE.Order); // Order�� ���º�ȭ
                }
            }
            else // �տ� �ִ� �������� ������ ���
            {
                ChangeState(STATE.Moving);
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
    IEnumerator FinishQuest(float t) // ��湮
    {
        yield return new WaitForSeconds(t);
        SpawnManager.Instance.Teleport(gameObject);
        Clockchk = false;
        onSmile = false;
        ChangeState(STATE.Moving);
    }
    public void GoExit() //�Դ»��¿��� ������ ���·�
    {
        ChangeState(STATE.Exit);
    }
    public void StartFinishQuest()
    {
        StartCoroutine(FinishQuest(3.0f));
    }

    // ������ ���� �Լ� �Ǵ� �ڷ�ƾ
    IEnumerator CoIcon(string IconName, float WaitSeconds)
    {
        yield return new WaitForSeconds(WaitSeconds);

        OnIcon(IconName);

        StopCoroutine(CoIcon(IconName, WaitSeconds));
    }
    public void CorourineIcon(string IconName, float WaitSeconds)
    {
        StartCoroutine(CoIcon(IconName, WaitSeconds));
    }
    public void OnIcon(string IconName)
    {
        Icon = Instantiate(Resources.Load($"IconPrefabs/{IconName}"), IconArea.transform) as GameObject;

        switch (IconName)
        {
            case "ClockIcon":
                Icon.GetComponent<ClockIcon>().myIconZone = myIconZone;
                Icon.GetComponent<ClockIcon>().myHost = this.gameObject;
                Clockchk = true;
                break;
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
}