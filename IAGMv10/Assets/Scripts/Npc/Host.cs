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

    public bool LineChk = false; // 퇴장하는 Npc와 입장하는 Npc의 충돌시 시계아이콘 생성되는 오류 수정
    public static Host inst = null;

    public GameObject Quest; // 프리팹 둘은 연결 >> 퀘스트 완료 >> 완료되었으면 quest 프리팹을 파괴

    //NpcClock,QuestImoticon
    public Transform spawnPoints;
    public Transform myIconZone;
    public Transform OutPoint;
    public GameObject myStaff;

    public GameObject IconArea = null;
    public bool VLchk; // 마을사람과 모험가 구분용
    public bool Questing = false; // 현재 퀘스트 진행중 = 재방문 해야하는 Npc
    public bool exitchk = false;
    public bool Clockchk = false;
    public bool IsQuest = false;
    int count; //배열 체크

    public bool onAngry = false;
    public bool onSmile = false;

    public int People; // 사람 구분

    //네비게이션
    NavMeshAgent agent;
    [SerializeField] Vector3 lob; // 로비 도착지점
    [SerializeField] Vector3 res; // 식당 도착지점
    [SerializeField] Vector3 mot; // 여관 도착지점

    [SerializeField] Transform Exit;

    ChairBedChk bedchairvalue;
    public int purpose; // 방문목적
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
            case STATE.Moving: // 윤섭
                StopAllCoroutines();
                agent.ResetPath(); // Wait 상태에서 Moving으로 바뀔때 목적지를 초기화
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
                StopAllCoroutines(); // 모든 코루틴 멈추고 대기
                agent.ResetPath();
                //시계 생성
                if (LineChk == true && Clockchk == false) // 줄 서있는 상태 + 이미 생성된 시계가 없다면
                {
                    IconArea = GameObject.Find("IconArea");
                    OnIcon("ClockIcon");
                    Clockchk = true;
                }
                StartCoroutine(ForwardCheck());
                break;
            case STATE.Order:
                StopAllCoroutines();
                LineChk = true; // 줄 서있는 상태
                if (Clockchk == false) // Wait 없이 바로 Order로 진입했다면 시계를 생성 = 이미 생성된 시계가 없다면
                {
                    IconArea = GameObject.Find("IconArea");
                    OnIcon("ClockIcon");
                }
                Destroy(Icon.GetComponent<ClockIcon>().myNotouch); // 시계 노터치 비활성화
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
                LineChk = false; // 줄 서있는 상태가 아님
                // 퇴장하는 호스트는 입장하는 호스트를 피해감 (우선순위 조절)
                agent.avoidancePriority = 51;
                agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

                GetComponent<Animator>().SetBool("IsMoving", true);

                if (purpose != 0) // 방문목적이 여관or식당 이라면
                {
                    Destroy(Icon); // 잠 또는 식사 아이콘 삭제
                }

                if(onAngry == false) // 주문이 받아 들여졌을 때(화가 안났을 때)
                {
                    agent.ResetPath(); // 네비 초기화

                    switch (purpose)
                    {
                        case 0:
                            break;
                        case 1: //침대 초기화
                            bedchairvalue._chairSlot[count] = ChairBedChk.ChairSlot.None;
                            GetComponent<Animator>().SetBool("SitToStand", true);
                            break;
                        case 2: //의자 초기화
                            bedchairvalue._bedSlot[count] = ChairBedChk.BedSlot.None;
                            GetComponent<Animator>().SetBool("LayToStand", true);
                            break;
                    }
                }
                else // 화가 난 상태라면
                {
                    OnIcon("AngryIcon");
                }

                if(onSmile == true)
                {
                    OnIcon("SmileIcon");
                }

                agent.SetDestination(Exit.position); // outpoint로 가는 코루틴
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
    public void GoBed() //주문하고 침대로 이동
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
    public void GoTable() //주문하고 테이블로 이동
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
    IEnumerator ForwardCheck()// 앞에 누가있는지 구분하여 상태를 Wait 또는 Order로 바꿔줌
    {
        while (true)
        {
            if (Physics.SphereCast(transform.position, 7.0f, transform.forward, out RaycastHit hitinfo, 7.0f, layerMask)) // 내 앞에 누군가 있을 경우
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero; //가속도 = 0
                GetComponent<Animator>().SetBool("IsMoving", false); // 걷는 애니 중지

                if (hitinfo.collider.gameObject.layer == 6) // layer 6 = Host, layer 3 = Staff // 앞에 Host가 있을 경우
                {
                    if(hitinfo.collider.gameObject.GetComponent<Host>().LineChk == true)
                    {
                        LineChk = true;
                    }
                    ChangeState(STATE.Wait);
                }
                else // 앞에 Staff가 있을 경우
                {
                    myStaff = hitinfo.collider.gameObject; // myStaff를 각 구역에 맞게 설정해줌 ( 스마일 아이콘 발동 시키기 위해 )
                    ChangeState(STATE.Order); // Order로 상태변화
                }
            }
            else // 앞에 있는 누군가가 비켰을 경우
            {
                ChangeState(STATE.Moving);
            }
            yield return null;
        }
    }
    IEnumerator EatToEating() //도착지점에 도착하면 Eat상태에서 Eating상태로
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
   
    IEnumerator BedToSleeping() //도착지점에 도착하면 Eat상태에서 Eating상태로
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
    IEnumerator FinishQuest(float t) // 재방문
    {
        yield return new WaitForSeconds(t);
        SpawnManager.Instance.Teleport(gameObject);
        Clockchk = false;
        onSmile = false;
        ChangeState(STATE.Moving);
    }
    public void GoExit() //먹는상태에서 나가는 상태로
    {
        ChangeState(STATE.Exit);
    }
    public void StartFinishQuest()
    {
        StartCoroutine(FinishQuest(3.0f));
    }

    // 아이콘 관련 함수 또는 코루틴
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