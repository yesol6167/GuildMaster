using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform myTop;
    public Transform myBarrel;
    public Vector2 BarrelRange = new Vector2(-30.0f, 5.0f);
    public Transform myMuzzle;
    public GameObject orgBomb;
    public float MoveSpeed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = Instantiate(orgBomb, myMuzzle);
            obj.GetComponent<Bomb>().OnFire();
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * MoveSpeed * Time.deltaTime);
        }
        //�ʴ� 90���� �ӵ��� aŰ�� ������ �������� ȸ�� dŰ�� ������ ���������� ȸ��
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * 90.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * 90.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            myTop.Rotate(-Vector3.up * 45.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            myTop.Rotate(Vector3.up * 45.0f * Time.deltaTime);
        }
        //�ʴ� 30���� �ӵ��� ���ַο�Ű�� ������ ������ ���� �ٿ�ַο�Ű�� ������ ������ �Ʒ��� �����̰� �ϼ���.
        if (Input.GetKey(KeyCode.UpArrow))
        {
            myBarrel.Rotate(-Vector3.right * 30.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            myBarrel.Rotate(Vector3.right * 30.0f * Time.deltaTime);
        }
        Vector3 rot = myBarrel.localRotation.eulerAngles;
        if (rot.x > 180.0f)
        {
            rot.x -= 360.0f;
        }
        /*
        if(rot.x > 5.0f)
        {
            rot.x = 5.0f;
        }
        if(rot.x < - 60.0f)
        {
            rot.x = -60.0f;
        }
        */
        rot.x = Mathf.Clamp(rot.x, BarrelRange.x, BarrelRange.y);
        myBarrel.localRotation = Quaternion.Euler(rot);
    }
}
