using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue1
{
    [TextArea]
    public string dialogue;
    public Sprite cg;
    public string name;
}
public class Tutorial1Dialogue : MonoBehaviour
{
    [SerializeField] public TMP_Text Content;
    [SerializeField] public TMP_Text Name;
    [SerializeField] public Image Profile;
    [SerializeField] public GameObject NextBtn;

    public int ContentNum = 0;
    public bool TalkEnd = false;

    [SerializeField] public Dialogue1[] Contents;

    public void PlayDialogue()
    {
        StartCoroutine(Typing(Content, Contents[ContentNum].dialogue));
        Profile.sprite = Contents[ContentNum].cg;
        Name.text = Contents[ContentNum].name;
    }

    public void ImmediatelyShow()
    {
        StopAllCoroutines();
        Content.text = Contents[ContentNum].dialogue;
        NextBtn.SetActive(true);
        TalkEnd = true;
    }

    IEnumerator Typing(TMP_Text Text, string text) //���� Ÿ���� ȿ��
    {
        NextBtn.SetActive(false);
        TalkEnd = false;
        for (int i = 0; i <= text.Length; ++i)
        {
            Text.text = text.Substring(0, i);
            yield return new WaitForSecondsRealtime(0.15f);
        }
        NextBtn.SetActive(true);
        TalkEnd = true;
    }
}
