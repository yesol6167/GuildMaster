using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCheckZone : MonoBehaviour
{
    public bool BlockChk = false;

    private void OnTriggerStay(Collider obj) // �ش� ������ Npc�� �������� ���ο� Npc ���� �ߴ�
    {
        BlockChk = true;
    }

    private void OnTriggerExit(Collider obj)
    {
        BlockChk = false;
    }
}
