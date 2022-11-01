using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffManager : MonoBehaviour
{
    public GameObject[] Staffs;

    private void OnSmileIcon()
    {
        // ������ ����
        GameObject iconarea = GameObject.Find("IconArea");
        GameObject obj = Instantiate(Resources.Load("IconPrefabs/SmileIcon"), iconarea.transform) as GameObject;

        int i = obj.GetComponent<SmileIcon>().myTarget.GetComponent<Host>().purpose; // ȣ��Ʈ�� �湮���� �˻�
        obj.GetComponent<SmileIcon>().myTarget = Staffs[i].GetComponent<Staff>().myIconZone; // �ش籸���� �������� ������ �������� �����Ѵ�.
    }
}
