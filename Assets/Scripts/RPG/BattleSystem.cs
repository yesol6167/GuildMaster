using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBattle
{
    void OnDamage(float dmg);  //�������̽��� �Լ��� �����ϸ� �ȵȴ�.
    bool IsLive();
}

public class BattleSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
