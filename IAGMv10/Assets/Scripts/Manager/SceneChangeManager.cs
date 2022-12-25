using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : Singleton<SceneChangeManager>
{

    public AsyncOperation ao;
    public bool LoadingChk = false; // ���� �ε� �������� �˻��ϴ� �Ұ�

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