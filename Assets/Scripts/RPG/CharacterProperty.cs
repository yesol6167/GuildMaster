using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterProperty : MonoBehaviour //C#�� ���߻���� �Ұ�
{
    public CharacterStat myStat;
    Animator _anim = null;
    protected Animator myAnim //������Ƽ�� ���ϰ� ����ϴ� ��
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
            return _anim;
        }
    }

    Rigidbody _rigid = null;
    protected Rigidbody myRigid
    {
        get
        {
            if (_rigid == null)
            {
                _rigid = GetComponent<Rigidbody>();
            }
            return _rigid;
        }
    }
}
