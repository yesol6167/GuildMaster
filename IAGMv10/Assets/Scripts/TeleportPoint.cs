using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class TeleportPoint : MonoBehaviour
{

    public GameObject teleportPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider obj)
    {
        GameObject Obj = obj.gameObject;

        if(Obj.GetComponent<Host>().IsFinishQuest == false)
        // IsFinishQuest�� false ��� ����Ʈ �������� ���谡�̹Ƿ� �ڷ���Ʈ ��Ŵ
        {
            obj.transform.Rotate(Vector3.up * 180);
            this.Teleport(obj.gameObject);
            obj.transform.parent = teleportPos.transform;

            Obj.GetComponent<Host>().StartFinishQuest();
        }
        else // ������ ���� Npc�� ������Ʈ�� �ı� ��Ŵ
        {
            Destroy(Obj);
            SpawnManager.Instance.hostCount--;
        }
    }

    void Teleport(GameObject host)
    {
        host.GetComponent<NavMeshAgent>().Warp(teleportPos.transform.position);
    }
}