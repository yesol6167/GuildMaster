using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StudyTransform : MonoBehaviour
{
    public LayerMask pickMask;

    Coroutine move = null;
    Coroutine rot = null;
    private void Awake()
    {
    }
    private void OnEnable()
    {
    }
    private void Reset()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        Vector3 target = new Vector3(2, 2, 2);//��ġ
        Vector3 pos = this.transform.position;//��ġ
        dir = target - pos;//����
        totalDist = dir.magnitude; //��Į��(����)
        dir.Normalize();
        */
        //dir = new Vector3(0, 1, 0);
        //totalDist = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(Vector3.forward * 360.0f * Time.deltaTime);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, pickMask))
            {
                Vector3 dir = hit.point - transform.position;
                float totalDist = dir.magnitude;
                dir.Normalize();
                //float angle = Vector3.Angle(transform.forward, dir);
                float d = Vector3.Dot(transform.forward, dir);
                float r = Mathf.Acos(d);
                float Angle = r * Mathf.Rad2Deg;
                float rotDir = 1.0f;
                if (Vector3.Dot(transform.right, dir) < 0.0f)
                {
                    rotDir = -1.0f;
                }
                //StopAllCoroutines();
                if (move != null)
                {
                    StopCoroutine(move);
                    move = null;
                }
                move = StartCoroutine(Moving(dir, totalDist));
                if (rot != null)
                {
                    StopCoroutine(rot);
                    rot = null;
                }
                rot = StartCoroutine(Roating(Angle, rotDir));
            }
        }
    }

    IEnumerator Roating(float Angle, float rotDir)
    {
        //�ʴ� 180���� �ӵ��� ȸ�� ��Ű����.
        while (Angle > 0.0f)
        {
            float delta = 180.0f * Time.deltaTime;
            if (Angle < delta)
            {
                delta = Angle;
            }
            Angle -= delta;
            transform.Rotate(Vector3.up * rotDir * delta);
            yield return null;
        }
    }

    IEnumerator Moving(Vector3 dir, float totalDist)
    {
        //�ʴ� 2������ �ӵ��� Ŭ�� �������� �̵��ϰ� ���弼��.        
        while (totalDist > 0.0f)
        {
            float delta = 2.0f * Time.deltaTime;
            if (totalDist < delta)
            {
                delta = totalDist;
            }
            totalDist -= delta;
            //this.transform.position += dir * delta;
            this.transform.Translate(dir * delta, Space.World);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yield break; // �ڷ�ƾ ��� ����
            }
            yield return null;
        }
        move = null;
    }
    /*
    void UpDownPingpong()
    {
        //�ʴ�2������ �ӵ��� ���� 3���� �̵� �����ϸ� �ٽ� �Ʒ��� 3���� �̵� ���ѹݺ�
        float delta = 2.0f * Time.deltaTime;
        if (totalDist < delta)
        {
            delta = totalDist;
        }
        totalDist -= delta;
        transform.position += dir * delta;
        if (Mathf.Approximately(totalDist, 0.0f))
        {
            dir = -dir;
            totalDist = 6.0f;
        }
    }
    */
}