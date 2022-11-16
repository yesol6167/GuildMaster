using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CamManager : Singleton<CamManager>
{
    public Transform myAxis;
    public Transform myCam;

    public float MoveSpeedK = 1.0f; // Ű���� �̵� ���ǵ�
    public float MoveSpeedM = 1.0f; // ���콺 �̵� ���ǵ�
    public float ZoomSpeed = 1.0f;
    public float RotSpeed = 1.0f;
    public float minX = -164;// -68;
    public float maxX = 174;// 88;
    public float minZ = -124;// -47;
    public float maxZ = 338;//51;


    //��
    Vector3 zoom = Vector3.zero;
    float SetDist = 70f; // �ʱ� �� �Ÿ��� 70
    public Vector2 ZoomRange = new Vector2(10, 10); // �� ����

    //ȭ�� ȸ��
    Vector3 Rot = Vector3.zero;



    // Start is called before the first frame update
    void Start()
    {
        zoom = (myCam.position - myAxis.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        //ȭ���̵�
        if (true)
        {
            // Ű����� �̵�
            if (Input.GetKey(KeyCode.W))
            {
                myAxis.transform.Translate(Vector3.back * MoveSpeedK * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                myAxis.transform.Translate(Vector3.forward * MoveSpeedK * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                myAxis.transform.Translate(Vector3.right * MoveSpeedK * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                myAxis.transform.Translate(Vector3.left * MoveSpeedK * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.Alpha0))
            {
                QuestCam0();
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                QuestCam1();
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                QuestCam2();
            }
            if (Input.GetKey(KeyCode.Alpha3))
            {
                QuestCam3();
            }
            if (Input.GetKey(KeyCode.Alpha4))
            {
                QuestCam4();
            }
            if (Input.GetKey(KeyCode.Alpha5))
            {
                QuestCam5();
            }
            if (Input.GetKey(KeyCode.Alpha6))
            {
                QuestCam6();
            }
            if (Input.GetKey(KeyCode.Alpha7))
            {
                QuestCam7();
            }


            // ���콺�� �̵�
            if (Input.GetMouseButton(1))
            {
                Vector3 dir = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
                myAxis.transform.Translate(dir * MoveSpeedM * Time.unscaledDeltaTime);
            }

            //�̵�����(�ʿ��� ����� �ʰ�)
            float x = Mathf.Clamp(myAxis.transform.position.x, minX, maxX); // ���� ���� ��
            float z = Mathf.Clamp(myAxis.transform.position.z, minZ, maxZ); // ���� ���� ��

            myAxis.transform.position = new Vector3(x, myAxis.transform.position.y, z); // ����,���� ���� ����
        }

        //ȭ��ȸ��
        if (Input.GetKeyDown(KeyCode.Q))
        {
            myAxis.transform.Rotate(Vector3.up * RotSpeed); // �ν����Ϳ��� �����Ϸ��� ���ǵ�� �� //  ��            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            myAxis.transform.Rotate(Vector3.down * RotSpeed);
        }


        //��
        SetDist -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        SetDist = Mathf.Clamp(SetDist, ZoomRange.x, ZoomRange.y);
        myCam.position = myAxis.position + myAxis.rotation * zoom * SetDist;
    }
    public void QuestCam0()
    {
        myAxis.transform.localPosition = new Vector3(35, 0, 0);
        myCam.localPosition = new Vector3(0, 45, 45);
    }
    public void QuestCam1()
    {
        myAxis.transform.localPosition = new Vector3(190, 0, 183);
        myCam.localPosition = new Vector3(0, 45, 45);
    }
    public void QuestCam2()
    {
        myAxis.transform.localPosition = new Vector3(-66, 0, 223);
        myCam.localPosition = new Vector3(0, 30, 30);
    }
    public void QuestCam3()
    {
        myAxis.transform.localPosition = new Vector3(110, 0, 343);
        myCam.localPosition = new Vector3(0, 38, 38);
    }
    public void QuestCam4()
    {
        myAxis.transform.localPosition = new Vector3(-103, 0, 295);
        myCam.localPosition = new Vector3(0, 46, 46);
    }
    public void QuestCam5()
    {
        myAxis.transform.localPosition = new Vector3(200, 0, -33);
        myCam.localPosition = new Vector3(0, 37, 37);
    }
    public void QuestCam6()
    {
        myAxis.transform.localPosition = new Vector3(-88, 0, 139);
        myCam.localPosition = new Vector3(0, 46, 46);
    }

    public void QuestCam7()
    {
        myAxis.transform.localPosition = new Vector3(-16, 0, -91);
        myCam.localPosition = new Vector3(-9, 22, 22);
    }
}
