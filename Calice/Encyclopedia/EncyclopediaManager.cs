using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SawEncylopediaData
{
    public List<bool> SawEncyclopediaCard = new List<bool>();
    public List<bool> SawEncyclopediaPassiveItem = new List<bool>();
    public List<bool> SawEncyclopediaActiveItem = new List<bool>();
}

public class EncyclopediaManager : MonoBehaviour
{
    [Header("���� �ر� ����Ʈ")]
    [SerializeField] SawEncylopediaData SawCommenData= new SawEncylopediaData();
    [SerializeField] SawEncylopediaData SawEndLessData = new SawEncylopediaData();

    [Header("���帮����� ����")]
    public bool EndLessOn = false;
    
    /// <summary>
    /// //������, �������� ���帮�� ���� ���ΰ� �ʿ��� ���
    /// </summary>
    public SawEncylopediaData SawDatas_Encylopedia      
    {
        get
        {
            if(!EndLessOn)
            {
                return SawCommenData;
            }
            else
            {
                return SawEndLessData;
            }
        }
    }

    /// <summary>
    /// //��ü��, ���� ���帮�� ������� ���ΰ� �ʿ��� ���
    /// </summary>
    public SawEncylopediaData SawDatas_All
    {
        get
        {
            if (!EndlessManager.instance.isEndless)
            {
                return SawCommenData;
            }
            else
            {
                return SawEndLessData;
            }
        }
    }

    public SawEncylopediaData GetSawDatas(bool Endless=false)
    {        
            if (!Endless)
            {
                return SawCommenData;
            }
            else
            {
                return SawEndLessData;
            }        
    }

    public bool AwakeCheck = false; //�ѹ��� ����ǵ���
    public static EncyclopediaManager instance = null;

    string SaveKey= "SawEncyclopedia";     //������ ���� Ű��
    public string GetSaveKey(bool Endless = false)
    {
        if (!Endless)
        {
            return "SawEncyclopedia";
        }
        else
        {
            return "SawEncyclopediaEnd";
        }
    }
    public void AwakeFunction()
    {
        if (!AwakeCheck)
        {
            //instance ����
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(this.gameObject);

            LoadData(); //�Ϲ�
            LoadData(true); //���帮��

            AwakeCheck = true;
        }
    }

    public void SetData(int num, EncyclopediaType type, string plus = "")
    {
        switch (type)
        {
            case EncyclopediaType.Card:                
                SawDatas_All.SawEncyclopediaCard[num] = true;
                break;
            case EncyclopediaType.Item:
                if (plus == "Passive")
                {
                    SawDatas_All.SawEncyclopediaPassiveItem[num] = true;
                }
                else
                {
                    SawDatas_All.SawEncyclopediaActiveItem[num] = true;
                }
                break;
        }

        DebugX.Log("���� Ȱ��ȭ - ����: " + type.ToString() + " �ε��� ��ȣ: " + num + " �߰�����: " + plus);
    }

    public void LoadData(bool Endless=false)
    {
        SawEncylopediaData SawData;

        if (!Endless)
        {
            SawData = GetSawDatas();
            SaveKey = GetSaveKey();
        }
        else
        {
            SawData = GetSawDatas(true);
            SaveKey = GetSaveKey(true);
        }
       

        //ī��
        if (ES3.KeyExists(SaveKey + "Card", "./SaveData/EncyclopediaData.es3"))
        {
            SawData.SawEncyclopediaCard = ES3.Load<List<bool>>(SaveKey + "Card", "./SaveData/EncyclopediaData.es3");

            if (SawData.SawEncyclopediaCard.Count < Collection.instance.ComcardDB.comcard.Count)   //���� �߰��� ī�尡 ������ Ȯ��
            {
                for (int i = SawData.SawEncyclopediaCard.Count; i < Collection.instance.ComcardDB.comcard.Count; i++)
                {
                    SawData.SawEncyclopediaCard.Add(false);
                }
                ES3.Save<List<bool>>(SaveKey + "Card", SawData.SawEncyclopediaCard, "./SaveData/EncyclopediaData.es3");
            }

        }
        else
        {
            for (int i = 0; i < Collection.instance.ComcardDB.comcard.Count; i++)
            {
                SawData.SawEncyclopediaCard.Add(false);
            }

            ES3.Save<List<bool>>(SaveKey + "Card", SawData.SawEncyclopediaCard, "./SaveData/EncyclopediaData.es3");
        }


        // �нú� ������
        if (ES3.KeyExists(SaveKey + "PassiveItem", "./SaveData/EncyclopediaData.es3"))
        {
            SawData.SawEncyclopediaPassiveItem = ES3.Load<List<bool>>(SaveKey + "PassiveItem", "./SaveData/EncyclopediaData.es3");

            if (SawData.SawEncyclopediaPassiveItem.Count < Collection.instance.ItemDB.Entities.Count)  //���� �߰��� �нú갡 ������ Ȯ��
            {
                for (int i = SawData.SawEncyclopediaPassiveItem.Count; i < Collection.instance.ItemDB.Entities.Count; i++)
                {
                    SawData.SawEncyclopediaPassiveItem.Add(false);
                }
                ES3.Save<List<bool>>(SaveKey + "PassiveItem", SawData.SawEncyclopediaPassiveItem, "./SaveData/EncyclopediaData.es3");
            }
        }
        else
        {
            for (int i = 0; i < Collection.instance.ItemDB.Entities.Count; i++)
            {
                SawData.SawEncyclopediaPassiveItem.Add(false);
            }

            ES3.Save<List<bool>>(SaveKey + "PassiveItem", SawData.SawEncyclopediaPassiveItem, "./SaveData/EncyclopediaData.es3");
        }


        // ��Ƽ�� ������
        if (ES3.KeyExists(SaveKey + "ActiveItem", "./SaveData/EncyclopediaData.es3"))
        {
            SawData.SawEncyclopediaActiveItem = ES3.Load<List<bool>>(SaveKey + "ActiveItem", "./SaveData/EncyclopediaData.es3");

            if (SawData.SawEncyclopediaActiveItem.Count < Collection.instance.ItemDB.Active.Count)     //���� �߰��� ��Ƽ�� Ȯ��
            {
                for (int i = SawData.SawEncyclopediaActiveItem.Count; i < Collection.instance.ItemDB.Active.Count; i++)
                {
                    SawData.SawEncyclopediaActiveItem.Add(false);
                }

                ES3.Save<List<bool>>(SaveKey + "ActiveItem", SawData.SawEncyclopediaActiveItem, "./SaveData/EncyclopediaData.es3");
            }
        }
        else
        {
            for (int i = 0; i < Collection.instance.ItemDB.Active.Count; i++)
            {
                SawData.SawEncyclopediaActiveItem.Add(false);
            }

            ES3.Save<List<bool>>(SaveKey + "ActiveItem", SawData.SawEncyclopediaActiveItem, "./SaveData/EncyclopediaData.es3");
        }
        
    }

    public void DataSave()
    {
        SawEncylopediaData SawData;

        if (!EndlessManager.instance.isEndless)
        {
            SawData = GetSawDatas();
            SaveKey = GetSaveKey();
        }
        else
        {
            SawData = GetSawDatas(true);
            SaveKey = GetSaveKey(true);
        }

        ES3.Save<List<bool>>(SaveKey + "Card", SawData.SawEncyclopediaCard, "./SaveData/EncyclopediaData.es3");
        ES3.Save<List<bool>>(SaveKey + "PassiveItem", SawData.SawEncyclopediaPassiveItem, "./SaveData/EncyclopediaData.es3");
        ES3.Save<List<bool>>(SaveKey + "ActiveItem", SawData.SawEncyclopediaActiveItem, "./SaveData/EncyclopediaData.es3");

        DebugX.Log("���� ������ ����");
    }

    public void SetFristCardData(List<int> card)
    {
        SawEncylopediaData SawData;

        if (!EndlessManager.instance.isEndless)
        {
            SawData = GetSawDatas();
            SaveKey = GetSaveKey();
        }
        else
        {
            SawData = GetSawDatas(true);
            SaveKey = GetSaveKey(true);
        }


        for (int i = 0; i < card.Count; i++)
        {
            SawData.SawEncyclopediaCard[(card[i] - 1)] = true;        //-1 �ϴ� ������ �ε��� ��ȣ�� �ƴ� �ø��� ��ȣ�� �� ����
        }
    }

    public void SetFristItemData(int item)
    {
        SawEncylopediaData SawData;

        if (!EndlessManager.instance.isEndless)
        {
            SawData = GetSawDatas();
            SaveKey = GetSaveKey();
        }
        else
        {
            SawData = GetSawDatas(true);
            SaveKey = GetSaveKey(true);
        }


        SawData.SawEncyclopediaPassiveItem[item] = true;
    }

    void OnApplicationQuit()    //���� ���� �� ������ ����
    {
        DataSave();
    }
}
