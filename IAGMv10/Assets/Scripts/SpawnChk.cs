using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChk : MonoBehaviour
{
    public bool chk = true;
    public GameObject AD;
    public GameObject Monster;
    public int QuestNum;

    public void Update()
    {
        //1~4����, 5~7ä��
        if (AD.transform.childCount == 0)
        {
            if (QuestNum > 0 && QuestNum < 5)
            {
                if (Monster.transform.childCount == 0)
                {
                    chk = true;
                }
                else
                {
                    chk = false;
                }
            }
            else
            {
                chk = true;
            }
        }
        else
        {
            chk = false;
        }
    }
}
