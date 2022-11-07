using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[Serializable]
public class GameManager : Singleton<GameManager>
{
  
    public int Gold;
    public int Fame;
    // Start is called before the first frame update

    public void GoTitle() //Ÿ��Ʋ ȭ������ ����
    {
        SceneManager.LoadScene(0);
    }

    public void GoMain() //����ȭ������ ����
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeGold(int Price)
    {
        Gold += Price;
        UIManager.Instance.UiChangeGold(Price);
    }

    public void ChangeFame(int Price)
    {
        Fame += Price;
        UIManager.Instance.UiChangeFame(Price);
    }
}
