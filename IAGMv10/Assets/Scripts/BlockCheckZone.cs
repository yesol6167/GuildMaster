using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheckZone : MonoBehaviour
{
    public GameObject mySpawnManager; // �����Ŵ��� ���ε�

    private void OnTriggerStay(Collider obj) // �ش� ������ Npc�� �������� ���ο� Npc ���� �ߴ�
    {
        mySpawnManager.GetComponent<SpawnManager>().BlockChk = true;
    }

    private void OnTriggerExit(Collider obj)
    {
        mySpawnManager.GetComponent<SpawnManager>().BlockChk = false;
    }
}
