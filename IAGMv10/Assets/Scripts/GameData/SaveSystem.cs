using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SaveSystem : MonoBehaviour
{

    private static string SavePath => Application.persistentDataPath + "/saves/"; //�������� ���
 
     public static void Save(ADNpc.ADNPC adnpc, string saveFileName)
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

    public static ADNpc.ADNPC Load(string saveFileName)
    {
        string saveFilePath = SavePath + saveFileName + ".json";

        if (!File.Exists(saveFilePath)) //��ο� ���������� �������� �ʴ´ٸ�
        {
            Debug.Log("����� ������ �����ϴ�.");
        }

        string saveFile = File.ReadAllText(saveFilePath);
        ADNpc.ADNPC adnpc = JsonUtility.FromJson<ADNpc.ADNPC>(saveFile); //saveFile�� CharState ������ �ҷ��´�
        return adnpc;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
