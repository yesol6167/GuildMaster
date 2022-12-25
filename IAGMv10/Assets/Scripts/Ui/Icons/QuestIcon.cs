using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestIcon : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public Transform myIconZone;
    public GameObject myButton;
    public Transform QuestWindow;
    Vector2 dragOffset = Vector2.zero;
    Quest.QuestInfo npcQuest;
    public GameObject hostobj;
    bool VLchk; // ������� or ���谡 üũ��

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetOut());
    }

    // Update is called once per frame
    void Update()
    {

        if (myIconZone != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(myIconZone.position);
            //-���߿� �̺κ� ������ ���� ������ ��� ã��-
            //myTarget.parent.parent.parent.parent.parent.parent.parent.parent.gameObject = host;
            hostobj = myIconZone.parent.parent.parent.parent.parent.parent.parent.parent.gameObject;
            VLchk = hostobj.GetComponent<Host>().VLchk;// ȣ��Ʈ ���� �����Ŵ������� �ٷ� �޾ƿ���
            npcQuest = VLchk ? hostobj.GetComponent<VLNpc>().myQuest : hostobj.GetComponent<QuestInformation>().myQuest; // ���׽����� ������� ��, ���谡 ������
        }
    }

    IEnumerator GetOut()
    {
        yield return new WaitForSeconds(15.0f);
        hostobj.GetComponent<Host>().onAngry = true;
        Destroy(gameObject);
    }

    public void ShowRequestWindow() //����Ʈ ��ư ������ ����Ʈ ��û�� ����
    {
        //GameManager.Instance.GetComponent<AudioSource>().Play();
        GameObject RQwindow;
        if (VLchk)
        {
            RQwindow = Instantiate(Resources.Load("Prefabs/RequestWindow"), GameObject.Find("WindowArea").transform) as GameObject;
        }
        else // ���谡���
        {
            if (hostobj.GetComponent<Host>().Questing == true)
            {
                RQwindow = Instantiate(Resources.Load("Prefabs/QuestReportWindow"), GameObject.Find("WindowArea").transform) as GameObject;
            }
            else
            {
                RQwindow = Instantiate(Resources.Load("Prefabs/QuestWindow"), GameObject.Find("WindowArea").transform) as GameObject;
            }
        }

        RQwindow.GetComponent<QuestInformation>().People = hostobj.GetComponent<Host>().People;
        RQwindow.GetComponent<QuestInformation>().IsQuestchk = hostobj.GetComponent<Host>().Questing;
        RQwindow.GetComponent<QuestInformation>().myQuest = npcQuest;
        //NPCchk= tru or false / Ʈ���ϰ�� â���������� �ڽ����� ��û�� ���� or �޽��� ���  IsFinishQuest�� bool�� üũ �� ���ǿ� �´� ���๮�� ����

        if (VLchk)
        {
            //RQwindow ������Ʈ�� QuestInformation ��ũ��Ʈ�� myQuest ������ npcQuest�� �ִ´�.
            RQwindow.GetComponent<QuestInformation>().myNpc = hostobj;
        }
        RQwindow.GetComponent<QuestInformation>().ShowQuest(npcQuest, hostobj);
        //UiCanvas �ؿ� ����
        RQwindow.transform.SetAsFirstSibling();
        RQwindow.SetActive(true);
        myButton.SetActive(false);
    }

    public void onDestroyIcon()
    {
        Destroy(gameObject);
    }

    //����Ʈâ �巡�� �� ���
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)QuestWindow.position - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        QuestWindow.position = eventData.position + dragOffset;
    }
}
