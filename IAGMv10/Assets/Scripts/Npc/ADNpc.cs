using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ADNpc : MonoBehaviour
{
    public LayerMask enemyMask = default;

    public GameObject AI_Per;

    public int adtype;

    public Sprite[] NpcProfiles;

    public CharState.NPC myStat;
    public struct ADNPC
    {
        //public string Name; // CharState���� �̸��� �ι� ���� ���ٰ��� �ٸ� �ΰ��� �̸��� ������� ���ؾ��� - �ּ�ó�� by ����
        public CharState.NPC myStatInfo; // ������ �� ���� ���� ���� ����Ʈ �й�
    }
    public static ADNPC RandomNPC(int a, Sprite[] np)
    {
        ADNPC adnpc = new ADNPC();
        adnpc.myStatInfo.profile = np[a]; // ������ �̹����� ���� a�� ���� �ٲ�Ƿ� �ش� �������� ����
        switch (a) // ������ n by����
        {
            case 0: // ���� �ü�
                adnpc.myStatInfo.profile = np[0];
                adnpc.myStatInfo.name = "������";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.ACHER;
                adnpc.myStatInfo.agility = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 20 + (adnpc.myStatInfo.strong / 5); // 10�� 2�� ����
                adnpc.myStatInfo.attack = 15 + (adnpc.myStatInfo.dexterity / 2); // 10�� 3�� ����
                adnpc.myStatInfo.defence = 15 + (adnpc.myStatInfo.strong / 10); // 10�� 1�� ����
                adnpc.myStatInfo.charGrade = Grade(adnpc); // �ü� ���ݷ�
                break;
            case 1: // ���� ����
                adnpc.myStatInfo.profile = np[1];
                adnpc.myStatInfo.name = "�糪";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.THIEF;
                adnpc.myStatInfo.agility = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 15 + (adnpc.myStatInfo.strong / 5);
                adnpc.myStatInfo.attack = 20 + (adnpc.myStatInfo.agility / 2);
                adnpc.myStatInfo.defence = 15 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.charGrade = Grade(adnpc);
                break;
            case 2: // ���� ������
                adnpc.myStatInfo.profile = np[2];
                adnpc.myStatInfo.name = "�ڸ���";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.WIZARD;
                adnpc.myStatInfo.agility = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 10 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.attack = 30 + (adnpc.myStatInfo.intellect / 2);
                adnpc.myStatInfo.defence = 10 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.charGrade = Grade(adnpc);
                break;
            case 3: // ���� �ü�
                adnpc.myStatInfo.profile = np[3];
                adnpc.myStatInfo.name = "�긮��";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.ACHER;
                adnpc.myStatInfo.agility = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 20 + (adnpc.myStatInfo.strong / 5);
                adnpc.myStatInfo.attack = 15 + (adnpc.myStatInfo.dexterity / 2);
                adnpc.myStatInfo.defence = 15 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.charGrade = Grade(adnpc);
                break;
            case 4: // ���� ����
                adnpc.myStatInfo.profile = np[4];
                adnpc.myStatInfo.name = "����";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.THIEF;
                adnpc.myStatInfo.agility = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 20 + (adnpc.myStatInfo.strong / 5);
                adnpc.myStatInfo.attack = 20 + (adnpc.myStatInfo.agility / 2);
                adnpc.myStatInfo.defence = 10 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.charGrade = Grade(adnpc);
                break;
            case 5: // ���� ������
                adnpc.myStatInfo.profile = np[5];
                adnpc.myStatInfo.name = "Ŭ��";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.WIZARD;
                adnpc.myStatInfo.agility = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 10 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.attack = 30 + (adnpc.myStatInfo.intellect / 2);
                adnpc.myStatInfo.defence = 10 + (adnpc.myStatInfo.strong / 10);
                adnpc.myStatInfo.charGrade = Grade(adnpc);
                break;
            case 6: // ���� ����
                adnpc.myStatInfo.profile = np[6];
                adnpc.myStatInfo.name = "����";
                adnpc.myStatInfo.npcJob = CharState.NPCJOB.WARRIOR;
                adnpc.myStatInfo.agility = 30 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 2)), 0, 99999999); // 10�� 10������
                adnpc.myStatInfo.intellect = 10 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 10)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.strong = 40 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 1)), 0, 99999999); // 10�� 1�� ����
                adnpc.myStatInfo.dexterity = 20 + Mathf.Clamp(UnityEngine.Random.Range(0, (GameManager.Instance.Fame / 5)), 0, 99999999); // 10�� 10�� ����
                adnpc.myStatInfo.health = 30 + (adnpc.myStatInfo.strong / 5);
                adnpc.myStatInfo.attack = 10 + (adnpc.myStatInfo.strong / 5);
                adnpc.myStatInfo.defence = 10 + (adnpc.myStatInfo.strong / 5);
                adnpc.myStatInfo.charGrade = Grade(adnpc);
                break;
        }
        return adnpc;
    }
    void Start()
    {
        NpcProfiles = Resources.LoadAll<Sprite>("Images/ProfileImages");
        myStat = RandomNPC(adtype, NpcProfiles).myStatInfo; // ��ŸƮ���� ĳ���Ϳ��� ������ �־���
        GetComponent<Host>().myStat.RotSpeed = 360.0f;
        GetComponent<Host>().myStat.MoveSpeed = 15.0f;

        switch (adtype)
        {
            case 0: // ���� �ü�
                GetComponent<Host>().myStat.AttackRange = 40.0f;
                GetComponent<Host>().myStat.AttackDelay = 1.0f;
                break;
            case 1: // ���� ����
                GetComponent<Host>().myStat.AttackRange = 15.0f;
                GetComponent<Host>().myStat.AttackDelay = 2.0f;
                break;
            case 2: // ���� ������
                GetComponent<Host>().myStat.AttackRange = 30.0f;
                GetComponent<Host>().myStat.AttackDelay = 1.0f;
                break;
            case 3: // ���� �ü�
                GetComponent<Host>().myStat.AttackRange = 40.0f;
                GetComponent<Host>().myStat.AttackDelay = 1.0f;
                break;
            case 4: // ���� ����
                GetComponent<Host>().myStat.AttackRange = 15.0f;
                GetComponent<Host>().myStat.AttackDelay = 2.0f;
                break;
            case 5: // ���� ������
                GetComponent<Host>().myStat.AttackRange = 30.0f;
                GetComponent<Host>().myStat.AttackDelay = 1.0f;
                break;
            case 6: // ���� ����
                GetComponent<Host>().myStat.AttackRange = 15.0f;
                GetComponent<Host>().myStat.AttackDelay = 2.0f;
                break;
        }
        GetComponent<Host>().myStat.AP = myStat.attack;
        GetComponent<Host>().myStat.MaxHp = myStat.health;
        GetComponent<Host>().myStat.HP = myStat.health;
        AI_Per.SetActive(false);
    }

    public static CharState.GRADE Grade(ADNPC npc)
    {
        CharState.GRADE grade = new CharState.GRADE();
        // ������ Ȯ��
        // ������ ���� �ɷ�ġ Ȯ��
        // �ɷ�ġ�� ���� ��� �й�

        switch (npc.myStatInfo.npcJob)
        {
            case CharState.NPCJOB.WARRIOR:
                //F~A ���
                // �� : F -> 10, E -> 50, D -> 100, C -> 300, B -> 500, A -> 1000
                if (npc.myStatInfo.strong <= 140) // �⺻���� 40
                {
                    grade = CharState.GRADE.F;
                }
                else if (npc.myStatInfo.strong <= 640 && npc.myStatInfo.strong > 140) // ���� 600
                {
                    grade = CharState.GRADE.E;
                }
                else if (npc.myStatInfo.strong <= 1640 && npc.myStatInfo.strong > 640)
                {
                    grade = CharState.GRADE.D;
                }
                else if (npc.myStatInfo.strong <= 4640 && npc.myStatInfo.strong > 1640)
                {
                    grade = CharState.GRADE.C;
                }
                else if (npc.myStatInfo.strong <= 9640 && npc.myStatInfo.strong > 4640)
                {
                    grade = CharState.GRADE.B;
                }
                else if (npc.myStatInfo.strong > 9640)
                {
                    grade = CharState.GRADE.A;
                }
                break;
            case CharState.NPCJOB.ACHER:
                if (npc.myStatInfo.dexterity <= 140) // �⺻���� 40
                {
                    grade = CharState.GRADE.F;
                }
                else if (npc.myStatInfo.dexterity <= 640 && npc.myStatInfo.dexterity > 140) // ���� 600
                {
                    grade = CharState.GRADE.E;
                }
                else if (npc.myStatInfo.dexterity <= 1640 && npc.myStatInfo.dexterity > 640)
                {
                    grade = CharState.GRADE.D;
                }
                else if (npc.myStatInfo.dexterity <= 4640 && npc.myStatInfo.dexterity > 1640)
                {
                    grade = CharState.GRADE.C;
                }
                else if (npc.myStatInfo.dexterity <= 9640 && npc.myStatInfo.dexterity > 4640)
                {
                    grade = CharState.GRADE.B;
                }
                else if (npc.myStatInfo.dexterity > 9640)
                {
                    grade = CharState.GRADE.A;
                }

                //npc.myStatInfo.dexterity

                break;
            case CharState.NPCJOB.WIZARD:
                if (npc.myStatInfo.intellect <= 140) // �⺻���� 40
                {
                    grade = CharState.GRADE.F;
                }
                else if (npc.myStatInfo.intellect <= 640 && npc.myStatInfo.intellect > 140) // ���� 600
                {
                    grade = CharState.GRADE.E;
                }
                else if (npc.myStatInfo.intellect <= 1640 && npc.myStatInfo.intellect > 640)
                {
                    grade = CharState.GRADE.D;
                }
                else if (npc.myStatInfo.intellect <= 4640 && npc.myStatInfo.intellect > 1640)
                {
                    grade = CharState.GRADE.C;
                }
                else if (npc.myStatInfo.intellect <= 9640 && npc.myStatInfo.intellect > 4640)
                {
                    grade = CharState.GRADE.B;
                }
                else if (npc.myStatInfo.intellect > 9640)
                {
                    grade = CharState.GRADE.A;
                }
                //npc.myStatInfo.intellect

                break;
            case CharState.NPCJOB.THIEF:
                if (npc.myStatInfo.agility <= 140) // �⺻���� 40
                {
                    grade = CharState.GRADE.F;
                }
                else if (npc.myStatInfo.agility <= 640 && npc.myStatInfo.agility > 140) // ���� 600
                {
                    grade = CharState.GRADE.E;
                }
                else if (npc.myStatInfo.agility <= 1640 && npc.myStatInfo.agility > 640)
                {
                    grade = CharState.GRADE.D;
                }
                else if (npc.myStatInfo.agility <= 4640 && npc.myStatInfo.agility > 1640)
                {
                    grade = CharState.GRADE.C;
                }
                else if (npc.myStatInfo.agility <= 9640 && npc.myStatInfo.agility > 4640)
                {
                    grade = CharState.GRADE.B;
                }
                else if (npc.myStatInfo.agility > 9640)
                {
                    grade = CharState.GRADE.A;
                }
                //npc.myStatInfo.agility

                break;
        }
        return grade;
    }
}
