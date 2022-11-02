using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Staff : MonoBehaviour
{
    public Transform myIconZone;
    public GameObject myIconArea;
    public void OnSmileIcon()
    {
        // ������ ����
        GameObject obj = Instantiate(Resources.Load("IconPrefabs/SmileIcon"), myIconArea.transform) as GameObject;
        obj.GetComponent<MoodIcon>().myIconZone = myIconZone; // �ش籸���� �������� ������ �������� �����Ѵ�.
    }
}
