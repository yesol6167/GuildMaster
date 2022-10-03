using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bomb : MonoBehaviour
{
    public LayerMask crashMask;
    bool bFire = false;
    public float MoveSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (bFire)
        {
            float delta = MoveSpeed * Time.deltaTime;
            Ray ray = new Ray(transform.position, transform.up); //���� ��ġ, ���� ��ġ world ���� 
            if(Physics.Raycast(ray, out RaycastHit hit, delta, crashMask))
            {
                Invoke("CreateTarget", 2.0f); //createtarget �Լ��� �ҷ��� �� ����
                hit.transform.GetComponent<Cube>()?.OnDelete();
                // ?�� null ���� �ƴ��� Ȯ�����ִ�
            }


            transform.Translate(Vector3.up * delta);
        }
    }

    public void OnFire()
    {
        bFire = true;
        //transform.parent = null;
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        bFire = false;
        Destroy(this.gameObject);
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        if ((crashMask & 1 << collision.gameObject.layer) != 0)
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((crashMask & 1 << other.gameObject.layer) != 0)
        {
           // Invoke("CreateTarget", 2.0f); //createtarget �Լ��� �ҷ��� �� ����
           // other.GetComponent<Cube>().OnDelete();
          
        }
    }

    void CreateTarget()
    {

        Destroy(gameObject);
        GameObject obj = Instantiate(Resources.Load("Prefabs\\Tar")) as GameObject;
        //Resources �ȿ� �ִ� �͸� �����ؼ� ������ �� ����
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
    }
}
