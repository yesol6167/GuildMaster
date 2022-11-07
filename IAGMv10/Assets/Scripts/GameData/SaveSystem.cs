using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveSystem : MonoBehaviour
{
    
    public static string SavePath => Application.persistentDataPath + "/saves/"; //�������� ���
 
     public static void Save<T>(T adnpc,string saveFileName)
    {
                    
            if (!Directory.Exists(SavePath)) //SavePath�� directory�� �������� �ʴ´ٸ�
            {
                Directory.CreateDirectory(SavePath); //directory ����
            }
            string saveJson = JsonUtility.ToJson(adnpc,true); //charState�� Json���� ǥ��
            string saveFilePath = SavePath + saveFileName + ".json";

            File.WriteAllText(saveFilePath, saveJson); //saveFilePath ������ �����ϰ� Json���� ǥ���� charState ������ �ۼ�
           
            Debug.Log("���� ����: " + saveFilePath);
           
    }

    public static T Load<T>(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath)) //��ο� ���������� �������� �ʴ´ٸ�
        {
            Debug.Log("����� ������ �����ϴ�.");
        }

        string saveFile = File.ReadAllText(saveFilePath);
        T adnpc = JsonUtility.FromJson<T>(saveFile); //saveFile�� CharState ������ �ҷ��´�

        return adnpc;
      
    }




}
