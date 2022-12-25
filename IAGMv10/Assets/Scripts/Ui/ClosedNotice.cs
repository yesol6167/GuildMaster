using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClosedNotice : MonoBehaviour
{
    public TMP_Text myText;

    float CloseTime;

    private void Start()
    {
        CloseTime = TimeManager.Instance.CloseReadyTime;
        StartCoroutine(ClockSound());
    }

    void Update()
    {
        CloseTime -= Time.deltaTime;
        myText.text = $"�����������\n{(int)CloseTime/60}�� {(int)CloseTime%60}�� ��";
    }

    public void ResetTime()
    {
        CloseTime = TimeManager.Instance.CloseReadyTime;
    }

    IEnumerator ClockSound()
    {
        while (TimeManager.Instance.clockSound)
        {
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(2.0f);
        }
        yield return null;
    }

}
