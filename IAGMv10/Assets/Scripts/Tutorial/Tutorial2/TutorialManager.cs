using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialDialogues;
    public GameObject Mask;
    public bool isTutorialEnd = false;
    public GameObject IconArea;
    public GameObject Notouch;
    public Button MenuIcon;
    public Transform PQ_Content;
    public Animator CamManager;
    public bool QuestChk = true;
    public GameObject FadeOut;

    void Start()
    {
        StartCoroutine(starttutorial());
    }

    IEnumerator starttutorial()
    {
        Time.timeScale = 0.0f;
        yield return new WaitWhile(() => tutorialDialogues[0].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[0].GetComponent<Tutorial2Dialogue>().dialogue.Length); // dialogue1 ��ȭ ����� ������ ����
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(1.0f);
        SpawnNpc("VL");//������� ����
        Notouch.SetActive(true);
        yield return new WaitForSeconds(5.8f);
        tutorialDialogues[1].SetActive(true);
        Time.timeScale = 0.0f;
        yield return new WaitWhile(() => tutorialDialogues[1].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[1].GetComponent<Tutorial2Dialogue>().dialogue.Length); //dialogue2 ��ȭ ����� ������ ����
        IconArea.GetComponentInChildren<ClockIcon>().myButton.onClick.AddListener(C_ClockIcon);
        Time.timeScale = 1.0f;
        yield return new WaitWhile(() => QuestManager.Instance.RQlist.Count < 1);
        Time.timeScale = 0.0f;
        tutorialDialogues[3].SetActive(true);
        MenuIcon.onClick.AddListener(C_MenuIcon);
        yield return new WaitWhile(() => tutorialDialogues[4].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[4].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(8.0f);
        //���谡 ����
        SpawnNpc("Q_AD");
        yield return new WaitForSeconds(6.5f);
        Time.timeScale = 0.0f;
        tutorialDialogues[5].SetActive(true);
        yield return new WaitWhile(() => tutorialDialogues[5].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[5].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(9.0f); // ���谡 ���� -> �����Ϸ� ������ ��
        Mask.GetComponent<Animator>().SetTrigger("M_QuestList");
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(1.5f); // ����ũ �̵�
        tutorialDialogues[6].SetActive(true);
        yield return new WaitWhile(() => tutorialDialogues[6].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[6].GetComponent<Tutorial2Dialogue>().dialogue.Length); //dialogue7 ��ȭ ����� ������ ����
        PQ_Content.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(Mask_AD);
        yield return new WaitWhile(() => tutorialDialogues[7].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[7].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        Time.timeScale = 0.0f;
        Mask.GetComponent<Animator>().SetTrigger("M_QuestList");
        PQ_Content.GetChild(0).gameObject.GetComponent<Button>().onClick.RemoveListener(Mask_AD);
        PQ_Content.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(BackGuild);
        yield return new WaitWhile(() => QuestManager.Instance.FQlist.Count < 1); // ����� ����Ʈ�� �߰��Ǹ�
        Mask.GetComponent<Animator>().SetTrigger("M_QuestList");
        yield return new WaitWhile(() => SpawnManager.Instance.spawnPoints[0].transform.childCount != 0);
        Time.timeScale = 0.0f;
        tutorialDialogues[9].SetActive(true);
        yield return new WaitWhile(() => tutorialDialogues[9].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[9].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        Mask.SetActive(false);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(1.0f);
        SpawnNpc("P_AD"); //�� ���谡 ����
        yield return new WaitForSeconds(7.0f);
        Time.timeScale = 0.0f;
        CamManager.SetTrigger("PubCam");
        Mask.GetComponent<Animator>().SetTrigger("M_AD");
        Mask.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        tutorialDialogues[10].SetActive(true);
        yield return new WaitWhile(() => tutorialDialogues[10].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[10].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(9.0f);
        tutorialDialogues[11].SetActive(true);
        yield return new WaitWhile(() => tutorialDialogues[11].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[11].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        CamManager.SetTrigger("MotelCam");
        SpawnNpc("M_AD"); //���� ���谡 ����
        yield return new WaitForSeconds(17.0f); 
        tutorialDialogues[12].SetActive(true);
        yield return new WaitWhile(() => tutorialDialogues[12].GetComponent<Tutorial2Dialogue>().count <= tutorialDialogues[12].GetComponent<Tutorial2Dialogue>().dialogue.Length);
        Mask.SetActive(false);
        CamManager.SetTrigger("LobbyCam");
        yield return new WaitForSecondsRealtime(1.5f);
        FadeOut.SetActive(true);
        StartCoroutine(FadeOutAnim());
    }

    public void C_ClockIcon()
    {
        StartCoroutine(CoNextTalk(2.5f, 2));
    }

    public void C_MenuIcon()
    {
        Mask.GetComponent<Animator>().SetTrigger("M_QuestList");
        StartCoroutine(CoNextTalk(2.0f, 4));
    }

    public void Mask_AD()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(CoNextTalk(5.0f, 7));
        Mask.GetComponent<Animator>().SetTrigger("M_AD");
    }

    public void BackGuild()
    {
        Mask.GetComponent<Animator>().SetTrigger("M_AD");
        Time.timeScale = 1.0f;
        StartCoroutine(CoNextTalk(1.5f, 8));
    }

    public void SpawnNpc(string npc)
    {
        switch(npc)
        {
            case "VL":
                SpawnManager.Instance.Host = Instantiate(SpawnManager.Instance.VL[0], SpawnManager.Instance.spawnPoints[0]) as GameObject;
                SpawnManager.Instance.Host.GetComponent<VLNpc>().job = VLNpc.NPCJOB.COMMONS;
                SpawnManager.Instance.Host.GetComponent<Host>().VLchk = true;
                SpawnManager.Instance.Host.GetComponent<Host>().purpose = 0;
                break;
            case "Q_AD":
                SpawnManager.Instance.Host = Instantiate(SpawnManager.Instance.AD[0], SpawnManager.Instance.spawnPoints[0]) as GameObject;
                int j = UnityEngine.Random.Range(0, QuestManager.Instance.RQlist.Count); //RQ����Ʈ�� �ִ� ����Ʈ �߿��� �������� ���谡���� �ο�
                SpawnManager.Instance.Host.GetComponent<QuestInformation>().myQuest = QuestManager.Instance.RQlist[j];
                QuestManager.Instance.RQlist.RemoveAt(j);//����Ʈ �����ϱ� ����
                Destroy(QuestManager.Instance.RQuest.transform.GetChild(j).gameObject);
                SpawnManager.Instance.Host.GetComponent<ADNpc>().adtype = 0;
                SpawnManager.Instance.Host.GetComponent<Host>().VLchk = false;
                SpawnManager.Instance.Host.GetComponent<Host>().People = 1;
                break;
            case "P_AD":
                SpawnManager.Instance.Host = Instantiate(SpawnManager.Instance.AD[0], SpawnManager.Instance.spawnPoints[1]) as GameObject;
                SpawnManager.Instance.Host.GetComponent<ADNpc>().adtype = 0;
                SpawnManager.Instance.Host.GetComponent<Host>().VLchk = false;
                SpawnManager.Instance.Host.GetComponent<Host>().People = 1;
                SpawnManager.Instance.Host.GetComponent<Host>().purpose = 1;
                break;
            case "M_AD":
                SpawnManager.Instance.Host = Instantiate(SpawnManager.Instance.AD[0], SpawnManager.Instance.spawnPoints[2]) as GameObject;
                SpawnManager.Instance.Host.GetComponent<ADNpc>().adtype = 0;
                SpawnManager.Instance.Host.GetComponent<Host>().VLchk = false;
                SpawnManager.Instance.Host.GetComponent<Host>().People = 1;
                SpawnManager.Instance.Host.GetComponent<Host>().purpose = 2;
                break;
        }
    }


    IEnumerator CoNextTalk(float time,int D_num)
    {
        Notouch.SetActive(true);
        yield return new WaitForSecondsRealtime(time);
        tutorialDialogues[D_num].SetActive(true);
    }

    IEnumerator FadeOutAnim()
    {
        while (FadeOut.GetComponent<Image>().fillAmount < 1.0f)
        {
            FadeOut.GetComponent<Image>().fillAmount += 0.5f * Time.deltaTime;
            if (FadeOut.GetComponent<Image>().fillAmount == 1.0f)
            {
                SceneChangeManager.Inst.ChangeScene("Main");
            }
            yield return null;
        }
    }
}
