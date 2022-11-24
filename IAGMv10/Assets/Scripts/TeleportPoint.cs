using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class TeleportPoint : MonoBehaviour
{

    public GameObject[] QuestPos1; // ���
    public GameObject[] QuestPos2; // ä��
    public GameObject[] AllPos;
    int num;
    bool questchk; // ��� pos1 or ä�� pos2
    bool allchk = false;
    bool QP1, QP2 = false;

    // Start is called before the first frame update
    void Start()
    {
        AllPos = new GameObject[QuestPos1.Length + QuestPos2.Length];
        for (int i = 0; i < QuestPos1.Length + QuestPos2.Length; i++) // 7
        {
            if (i < QuestPos1.Length)
            {
                AllPos[i] = QuestPos1[i];
            }
            else
            {
                AllPos[i] = QuestPos2[i - QuestPos1.Length];
            }
        }
    }

    public bool AllChk() // true �� ���� �ʾ���
    {
        for (int i = 0; i < AllPos.Length; i++)
        {
            if (AllPos[i].transform.parent.GetComponent<SpawnChk>().chk)
            {
                allchk = true;
            }
        }
        return allchk;
    }

    public bool Quest1chk()
    {
        for (int i = 0; i < QuestPos1.Length; i++)
        {
            if (QuestPos1[i].transform.parent.GetComponent<SpawnChk>().chk)
            {
                QP1 = true;
            }
        }
        return QP1;
    }
    public bool Quest2chk()
    {
        for (int i = 0; i < QuestPos2.Length; i++)
        {
            if (QuestPos2[i].transform.parent.GetComponent<SpawnChk>().chk)
            {
                QP2 = true;
            }
        }
        return QP2;
    }


    private void OnTriggerEnter(Collider obj)
    {
        GameObject Obj = obj.gameObject;

        if (Obj.GetComponent<Host>().Questing == true)
        // IsFinishQuest�� false ��� ����Ʈ �������� ���谡�̹Ƿ� �ڷ���Ʈ ��Ŵ
        {
            questchk = obj.GetComponent<QuestInformation>().myQuest.questname != "ä��";
            //���ڸ����� Ȯ��
            for (int i = 0; i < (questchk ? QuestPos1.Length : QuestPos2.Length);)
            {
                //�� á�� �� ����ó�� �ʿ�
                if ((questchk ? QuestPos1[i] : QuestPos2[i]).transform.parent.GetComponent<SpawnChk>().chk == true)
                {
                    obj.transform.parent = (questchk ? QuestPos1[i] : QuestPos2[i]).transform;
                    num = i;
                    break;
                }
                else
                {
                    i++;
                }
            }
            obj.GetComponent<Host>().FarmAni = num;
            this.Teleport(obj.gameObject, num, questchk);
            if (questchk)
            {
                SpawnManager.Instance.grade = (MonsterStat.GRADE)obj.GetComponent<QuestInformation>().myQuest.questgrade;
                SpawnManager.Instance.MonsterSpawn();
            }
        }
        else // ������ ���� Npc�� ������Ʈ�� �ı� ��Ŵ
        {
            Destroy(Obj);
            SpawnManager.Instance.hostCount--;
        }
    }

    void Teleport(GameObject host, int num, bool chk)
    {
        if (chk)
        {
            host.GetComponent<ADNpc>().AI_Per.SetActive(true);
            if (host.GetComponent<ADNpc>().myStat.npcJob == CharState.NPCJOB.ACHER)
            {
                host.GetComponent<Host>().myBow.SetActive(true);
            }
        }
        //���ڸ����� Ȯ��
        host.GetComponent<NavMeshAgent>().Warp((questchk ? QuestPos1[num] : QuestPos2[num]).transform.position);
        if (host.GetComponent<QuestInformation>().myQuest.questname == "ä��")
        {
            host.GetComponent<Host>().StateFarming();
        }
        /*teleportPos[num].transform.parent.GetComponent<SpawnChk>().chk = false;*/
    }
}