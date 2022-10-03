using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public delegate void MyAction();

public class CharacterMovement : CharacterProperty, IBattle
{

    Coroutine moveCo = null;
    Coroutine rotCo = null;
    Coroutine attackCo = null;

   protected void AttackTarget(Transform target)
    {
        StopAllCoroutines();
        attackCo = StartCoroutine(AttackingTarget(target,myStat.AttackRange,myStat.AttackDelay));
    }
    protected void MoveToPosition(Vector3 pos, UnityAction done = null, bool Rot=true)
    {
        if (attackCo != null)
        {
            StopCoroutine(attackCo);
            attackCo = null;
        }
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        moveCo = StartCoroutine(MovingToPosition(pos,done));
        if (Rot)
        {
            if (rotCo != null)
            {
                StopCoroutine(rotCo);
                rotCo = null;
            }
            rotCo = StartCoroutine(RotatingToPosition(pos));
        }
    }
    IEnumerator RotatingToPosition(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized; //���̰� 1�� ���ͷ� ����
        float Angle = Vector3.Angle(transform.forward, dir); // ���� ���ϱ�
        float rotDir = 1.0f;
        if (Vector3.Dot(transform.right, dir) < 0.0f) //right ���Ϳ� dir ����
        {
            rotDir = -rotDir;
        }

        while (Angle > 0.0f)
        {
            float delta = myStat.RotSpeed * Time.deltaTime;
            if (delta > Angle)
            {
                delta = Angle;
            }
            Angle -= delta;
            transform.Rotate(Vector3.up * rotDir * delta, Space.World);
            yield return null;
        }
    }

    IEnumerator MovingToPosition(Vector3 pos, UnityAction done) //�������� �����ϸ� �ش� ���������� �̵�
    {
        Vector3 dir = pos - transform.position; //����
        float dist = dir.magnitude; //�Ÿ�
        dir.Normalize(); //����ȭ

        //�޸��� ����
        myAnim.SetBool("IsMoving", true);
        //�ڱⰡ �����ִ� ������Ʈ���� ���ϴ� ������Ʈ �������� getcomponent
        while (dist > 0.0f) //�̵��� �Ÿ��� ������ ��� �ݺ�
        {
            if (!myAnim.GetBool("IsAttacking"))
            {
                float delta = myStat.MoveSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                dist -= delta;
                transform.Translate(dir * delta, Space.World);
            }
            yield return null;
        }
        //�޸��� �� - ����
        myAnim.SetBool("IsMoving", false);
        done?.Invoke(); //done�� null�� �ƴҶ����� ����Ǿ� �ִ� �Լ��� �����ϴ� Invoke
    }

    IEnumerator AttackingTarget(Transform target, float AttackRange, float AttackDelay) //���� �ƴ϶� ���������� ����
    {
        float playTime = 0.0f;
        float delta = 0.0f;
        while (target != null)
        {
           
            if(!myAnim.GetBool("IsAttacking")) playTime += Time.deltaTime;

            //�̵�
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude;
            dir.Normalize();

            if (dist > AttackRange)
            {
                myAnim.SetBool("IsMoving", true);
                delta = myStat.MoveSpeed * Time.deltaTime;
                if(delta > dist)
                 {
                     delta = dist;
                 }
                transform.Translate(dir * delta, Space.World);
            }
            //�Ź� ���� ���ϴ� ���̶� ���� �ʿ�� ����
            else
            {
                myAnim.SetBool("IsMoving", false);
                if(playTime >= AttackDelay)
                {
                    //����
                    playTime = 0.0f;
                    myAnim.SetTrigger("Attack");
                }
            }

            //ȸ��
            delta = myStat.RotSpeed * Time.deltaTime;
            float Angle = Vector3.Angle(dir, transform.forward);
            float rotDir = 1.0f;
            if(Vector3.Dot(transform.right, dir) < 0.0f)
            {
                rotDir = -rotDir;
            }
            if(delta > Angle)
            {
                delta = Angle;
            }
            transform.Rotate(Vector3.up * rotDir * delta, Space.World);

            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
    }

    public void OnDamage(float dmg)
    {

    }
}
