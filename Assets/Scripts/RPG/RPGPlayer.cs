using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGPlayer : CharacterMovement, IBattle
{
    public LayerMask pickMask = default;
    public LayerMask enemyMask = default;

    Transform myTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���콺 ��ġ���� ������ ����������� ���� ������ ���̸� �����.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // ���̾� ����ũ�� �ش��ϴ� ������Ʈ�� ���õǾ����� Ȯ���Ѵ�.
            if(Physics.Raycast(ray, out hit, 1000.0f, enemyMask))
            {
                myTarget = hit.transform;
                AttackTarget(myTarget);
            }
            else if (Physics.Raycast(ray, out hit, 1000.0f, pickMask))
            {
                base.MoveToPosition(hit.point); //�θ� �ִ� �Լ�
            }
        }
/*
        if (Input.GetMouseButtonDown(1) && !myAnim.GetBool("IsAttacking"))
        {
            myAnim.SetTrigger("Attack");
        }
*/
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 300.0f); //���� 300��ŭ�� ���� ���ϴ°�
        }
        
    }

    

    private void OnCollisionExit(Collision collision) //�ٴڿ��� ��������
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            myAnim.SetBool("IsAir", true);
        }
      
    }

    private void OnCollisionEnter(Collision collision) //�ٴڿ� �پ�����
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            myAnim.SetBool("IsAir", false);
        }
    }

    public void OnDamage(float dmg)
    {
        myAnim.SetTrigger("Damage");
    }

    public void AttackTarget()
    {
        if (myTarget.GetComponent<IBattle>().IsLive())
        {
            myTarget.GetComponent<IBattle>()?.OnDamage(myStat.AP);
        }  
    }

    public bool IsLive()
    {
        return !Mathf.Approximately(myStat.HP, 0.0f);
    }
}
