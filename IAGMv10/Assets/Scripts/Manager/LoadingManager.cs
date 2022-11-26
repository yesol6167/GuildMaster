using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public GameObject LoadingText;
    public GameObject AnyKeyText;

    // Update is called once per frame
    void Update()
    {
        if(SceneChangeManager.Inst.ao.progress >= 0.9f) // �� �ε��� �Ϸ� �Ǿ��� ��
        {
            LoadingText.GetComponent<Animator>().SetTrigger("OnIdle");
            LoadingText.GetComponent<TMP_Text>().text = "�ε��Ϸ�!";
            AnyKeyText.SetActive(true);
        }
    }
}
