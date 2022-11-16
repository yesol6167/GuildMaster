using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodIcon : MonoBehaviour // ������ + �ޱ׸� ������
{
    public Transform myIconZone;
    float MoodTime = 1.5f; // �������� �ð�
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(myIconZone.position);
        transform.position = pos;

        time += Time.deltaTime;
        if (time > MoodTime)
        {
            Destroy(gameObject);
        }
    }
}
