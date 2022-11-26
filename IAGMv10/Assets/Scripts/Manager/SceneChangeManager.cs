using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : MonoBehaviour
{
    // �̱���
    #region
    public static SceneChangeManager Inst = null; 
    private void Awake()
    {
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public AsyncOperation ao;
    bool LoadingChk = false; // ���� �ε� �������� �˻��ϴ� �Ұ�
    //public Image myTimeArea;

    private void Start()
    {
        //StartCoroutine(ClockAnim()); // �ð��۵�
    }

    /*IEnumerator ClockAnim()
    {
        myTimeArea.fillAmount = 0.0f;
        float speed = 1.0f;
        while (myTimeArea.fillAmount < 1.0f)
        {
            myTimeArea.fillAmount += speed * Time.deltaTime;
            if (myTimeArea.fillAmount == 1.0f)
            {
                myTimeArea.fillAmount = 0;
            }
            yield return null;
        }
    }*/

    public void ChangeScene(string s)
    {
        if (!LoadingChk)
        {
            StartCoroutine(Loading(s));
        }
    }

    IEnumerator Loading(string s)
    {
        LoadingChk = true;
        yield return SceneManager.LoadSceneAsync("Loading"); // �ε� ���� ��ȯ �Ѵ� = �ε� ���� �����ش�.
        yield return StartCoroutine(LoadNextScene(s));
        LoadingChk = false;
    }

    IEnumerator LoadNextScene(string s)
    {       
        ao = SceneManager.LoadSceneAsync(s);
        ao.allowSceneActivation = false; //���ε��� ������ ������ ���� Ȱ��ȭ ��Ű�� �ʴ´�.

        //���ε��� �Ϸ�Ǹ� ����ȯ
        while (!ao.isDone)
        {
            if (ao.progress >= 0.9f)
            {
                if(Input.anyKeyDown)
                {
                    ao.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
}
