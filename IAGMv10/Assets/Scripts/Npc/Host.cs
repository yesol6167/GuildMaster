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

public class Host : MonoBehaviour, IBattle
{
    GameObject Icon;

    public bool LineChk = false; // �����ϴ� Npc�� �����ϴ� Npc�� �浹�� �ð������ �����Ǵ� ���� ����
    public static Host inst = null;

    public GameObject Quest; // ������ ���� ���� >> ����Ʈ �Ϸ� >> �Ϸ�Ǿ����� quest �������� �ı�
    // ȭ�� ��ġ ȭ��
    public Transform myBow;
    public GameObject orgArrow;

    //���� ����Ʈ 
    public GameObject magiceff;
    public Transform trs;

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

    public int FarmAni;
    //�׺���̼�
    NavMeshAgent agent;
    [SerializeField] Vector3 lob; // �κ� ��������
    [SerializeField] Vector3 res; // �Ĵ� ��������
    [SerializeField] Vector3 mot; // ���� ��������
    [SerializeField] Vector3 tel; // �ڷ���Ʈ ��ġ

    [SerializeField] Transform Exit;

    ChairBedChk bedchairvalue;
    public int purpose; // �湮����
    public LayerMask layerMask;
   
    public enum STATE
    {
        Create, Idle, Moving, Wait, Order, Eating, Sleeping, Battle, Exit ,Farming
    }
    public STATE myState = STATE.Create;
    //�ڵ� ���� ����
    public CharacterStat myStat;
    List<IBattle> myAttackers = new List<IBattle>();

    Transform _target = null;
    Transform myTarget
    {
        get => _target;
        set
        {
            _target = value;
            if (_target != null) _target.GetComponent<IBattle>()?.AddAttacker(this);
        }
    }

    //������ ���� ����
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Idle:
                StopAllCoroutines();
                break;
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
            case STATE.Battle:
                AttackTarget(myTarget);
                break;
                case STATE.Farming:
                if(FarmAni == 0)
                {
                    GetComponent<Animator>().SetTrigger("Pick");
                }
                else if(FarmAni == 1)
                {
                    GetComponent<Animator>().SetTrigger("Find");
                }
                else if(FarmAni == 2)
                {
                    GetComponent<Animator>().SetTrigger("Pick");
                }
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
            case STATE.Create:
                break;
            case STATE.Idle:
                break;
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
            case STATE.Battle:
                StopCoroutine(ForwardCheck());
                break;
            case STATE.Farming:
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
        GetComponent<ADNpc>().AI_Per.SetActive(false);
        SpawnManager.Instance.Teleport(gameObject);
        GetComponent<Animator>().SetBool("IsMoving", true);
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
    //�ڵ�����
    protected void AttackTarget(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(AttackingTarget(target, myStat.AttackRange, myStat.AttackDelay));
    }
    public void OnDamage(float dmg)
    {
        myStat.HP -= dmg;
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            StopAllCoroutines();
            foreach (IBattle ib in myAttackers)
            {
                ib.DeadMessage(transform);
            }
            GetComponent<Animator>().SetTrigger("Dead");
            StartCoroutine(FinishQuest(2.0f));
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Damage");
        }
    }

    IEnumerator AttackingTarget(Transform target, float AttackRange, float AttackDelay)
    {
        float playTime = 0.0f;
        float delta = 0.0f;
        while (target != null)
        {
            if (!GetComponent<Animator>().GetBool("IsAttacking")) playTime += Time.deltaTime;
            //�̵�
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude;
            if (dist > myStat.AttackRange)
            {
                GetComponent<Animator>().SetBool("IsMoving", true);
                dir.Normalize();
                delta = myStat.MoveSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                transform.Translate(dir * delta, Space.World);
            }
            else
            {
                GetComponent<Animator>().SetBool("IsMoving", false);
                if (playTime >= myStat.AttackDelay)
                {
                    //����
                    playTime = 0.0f;

                    switch (GetComponent<ADNpc>().adtype)//������ �ִϸ��̼� ����
                    {
                        case 0: // ���� �ü�
                            GetComponent<Animator>().SetTrigger("ArrowAttack");
                            CreateArrow();
                            break;
                        case 1: // ���� ����
                            GetComponent<Animator>().SetTrigger("ThiefAttack");
                            break;
                        case 2: // ���� ������
                            GetComponent<Animator>().SetTrigger("MagicAttack");
                            break;
                        case 3: // ���� �ü�
                            GetComponent<Animator>().SetTrigger("ArrowAttack");
                            CreateArrow();
                            break;
                        case 4: // ���� ����
                            GetComponent<Animator>().SetTrigger("ThiefAttack");
                            break;
                        case 5: // ���� ������
                            GetComponent<Animator>().SetTrigger("MagicAttack");
                            break;
                        case 6: // ���� ����
                            GetComponent<Animator>().SetTrigger("WarriorAttack");
                            break;
                    }
                }
            }
            //ȸ��
            delta = myStat.RotSpeed * Time.deltaTime;
            float Angle = Vector3.Angle(dir, transform.forward);
            float rotDir = 1.0f;
            if (Vector3.Dot(transform.right, dir) < 0.0f)
            {
                rotDir = -rotDir;
            }
            if (delta > Angle)
            {
                delta = Angle;
            }
            transform.Rotate(Vector3.up * delta * rotDir, Space.World);
            yield return null;
        }
        GetComponent<Animator>().SetBool("IsMoving", false);
    }
    public void AttackTarget()
    {
        myTarget.GetComponent<IBattle>()?.OnDamage(myStat.AP);
    }

    public bool IsLive()
    {
        return Mathf.Approximately(myStat.HP, 0.0f);
    }

    public void FindTarget(Transform target)
    {
        myTarget = target;
        StopAllCoroutines();
        ChangeState(STATE.Battle);
    }

    public void LostTarget()
    {
        myTarget = null;
        StopAllCoroutines();
        GetComponent<Animator>().SetBool("IsMoving", false);
        ChangeState(STATE.Idle); // ���� �̻��� �� Idle �߰�
    }

    public void AddAttacker(IBattle ib)
    {
        myAttackers.Add(ib);
    }

    public void DeadMessage(Transform tr)
    {
        if (tr == myTarget)
        {
            LostTarget();
            StartCoroutine(FinishQuest(2.0f));
        }
    }

    public void CreateArrow() // ȭ�� ����
    {
        GameObject obj = Instantiate(orgArrow, myBow);
        obj.GetComponent<Arrow>().myTarget = myTarget;
        obj.GetComponent<Arrow>().OnFire();
    }
    public void MagicImpact()
    {
        Instantiate(magiceff, myTarget.position + myTarget.transform.up * 5.0f, Quaternion.identity);
    }
    public void FarmReturn()
    {
        SpawnManager.Instance.Teleport(gameObject);
        ChangeState(STATE.Moving);
        onSmile = false;
        Clockchk = false;
    }
    public void StateFarming()
    {
        ChangeState(STATE.Farming);
    }
}