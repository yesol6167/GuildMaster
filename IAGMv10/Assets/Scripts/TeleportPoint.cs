using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class TeleportPoint : MonoBehaviour
{

    public GameObject[] QuestPos1; // ���
    public GameObject[] QuestPos2; // ä��
    int num;
    bool questchk; // ��� pos1 or ä�� pos2

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider obj)
    {
        GameObject Obj = obj.gameObject;


        if (Obj.GetComponent<Host>().Questing == true)
        // IsFinishQuest�� false ��� ����Ʈ �������� ���谡�̹Ƿ� �ڷ���Ʈ ��Ŵ
        {
            questchk = obj.GetComponent<QuestInformation>().myQuest.questname != "ä��";
                //���ڸ����� Ȯ��
                for (int i = 0; i < (questchk? QuestPos1.Length : QuestPos2.Length);)
                {
                    //�� á�� �� ����ó�� �ʿ�
                    if ((questchk?QuestPos1[i]: QuestPos2[i]).transform.parent.GetComponent<SpawnChk>().chk == true)
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
            this.Teleport(obj.gameObject, num,questchk);
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

    void Teleport(GameObject host, int num,bool chk)
    {
        if (chk)
        {
            host.GetComponent<ADNpc>().AI_Per.SetActive(true);
        }
        //���ڸ����� Ȯ��
        host.GetComponent<NavMeshAgent>().Warp((questchk ? QuestPos1[num] : QuestPos2[num]).transform.position);
        if(host.GetComponent<QuestInformation>().myQuest.questname == "ä��")
        {
            host.GetComponent<Host>().StateFarming();
        }
        /*teleportPos[num].transform.parent.GetComponent<SpawnChk>().chk = false;*/
    }
}