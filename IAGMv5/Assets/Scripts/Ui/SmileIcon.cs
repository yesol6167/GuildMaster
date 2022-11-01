using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class SmileIcon : MonoBehaviour
{
    public Transform myTarget;
    public GameObject myIcon;

    float SmileTime;

    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);
        // mySmileCreateZone�� 3���� ��ġ���� pos��� �Ѵ�.

        transform.position = pos; // pos�� ���� ��ġ���� �ȴ�.(2�������� ������)

        // ������ 1.5���� ����
        SmileTime += Time.deltaTime;
        if (SmileTime > 1.5f)
        {
            Destroy(myIcon);
        }
    }
}
