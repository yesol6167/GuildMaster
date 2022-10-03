using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UIElements;

public class StudyCoroutine : MonoBehaviour
{
    IEnumerator moving = null;
    // Start is called before the first frame update
    void Start()
    {
        moving = MovingUp(3.0f);
        //StartCoroutine(MovingUp(3.0f)); 
        //����) ���� ������Ʈ ���� ����ϸ� �ȵȴ�
    }

    // Update is called once per frame
    void Update() //������Ʈ ���� �ִ°��� ���� �ڷ�ƾ���� ���� �� �ִ�.
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Rotating(360.0f));
        }
        moving.MoveNext();
    }

    IEnumerator Rotating(float angle)
    {
        
        //�����̽��ٸ� ���������� �ʴ� �ѹ����� ������
        while (angle > 0.0f)
        {
            float delta = 360.0f * Time.deltaTime;
            if (delta > angle)
            {
              delta = angle;
            }
            angle -= delta;
            transform.Rotate(Vector3.up * delta);
            yield return null;
        }

    }

    //�ʴ� 2������ �ӵ��� ���� 3����, �Ʒ��� 3���� �Դٰ����ϰ� ����ÿ�
    IEnumerator MovingUp(float d) //�ڷ�ƾ�� ������ yield return�� ���ԵǾ� �־�� �Ѵ�.
    {
        Vector3 dir = Vector3.up;
        float dist = d;
        while (true)
        {
            float delta = 2.0f * Time.deltaTime;
            if(delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(dir*delta);
            if (Mathf.Approximately(dist, 0.0f))
            {
                dir = -dir;
                dist = d;

                yield return StartCoroutine(Rotating(180.0f)); 
            }
            //���� �ڵ尡 �������Ӹ��� �ݺ�

            yield return null;

            //�ٽ�. ���� ���� 0.5�� �� �������� �ٲ�� �ٽ� �����Ѵ�. null�� ������ ����
            }
        }

   
    
}
