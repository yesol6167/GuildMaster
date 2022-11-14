using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NewsBalloon : MonoBehaviour
{
    float NewsTime;
    public TMP_Text myText;
    
    void Update()
    {
        NewsTime += Time.deltaTime;
        if (NewsTime > 1.5f) // 1.5�� ���� ���̰� �����
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string msg)
    {
        myText.text = msg;
    }
}
