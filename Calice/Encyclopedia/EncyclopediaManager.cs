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
    [Header("도감 해금 리스트")]
    [SerializeField] SawEncylopediaData SawCommenData= new SawEncylopediaData();
    [SerializeField] SawEncylopediaData SawEndLessData = new SawEncylopediaData();

    [Header("엔드리스모드 여부")]
    public bool EndLessOn = false;
    
    /// <summary>
    /// //도감용, 도감에서 엔드리스 켜짐 여부가 필요한 경우
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
    /// //전체용, 현재 엔드리스 모드인지 여부가 필요한 경우
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

    public bool AwakeCheck = false; //한번만 선언되도록
    public static EncyclopediaManager instance = null;

    string SaveKey= "SawEncyclopedia";     //저장할 변수 키값
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
            //instance 생성
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(this.gameObject);

            LoadData(); //일반
            LoadData(true); //엔드리스

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

        DebugX.Log("도감 활성화 - 종류: " + type.ToString() + " 인덱스 번호: " + num + " 추가사항: " + plus);
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
       

        //카드
        if (ES3.KeyExists(SaveKey + "Card", "./SaveData/EncyclopediaData.es3"))
        {
            SawData.SawEncyclopediaCard = ES3.Load<List<bool>>(SaveKey + "Card", "./SaveData/EncyclopediaData.es3");

            if (SawData.SawEncyclopediaCard.Count < Collection.instance.ComcardDB.comcard.Count)   //새로 추가된 카드가 없는지 확인
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


        // 패시브 아이템
        if (ES3.KeyExists(SaveKey + "PassiveItem", "./SaveData/EncyclopediaData.es3"))
        {
            SawData.SawEncyclopediaPassiveItem = ES3.Load<List<bool>>(SaveKey + "PassiveItem", "./SaveData/EncyclopediaData.es3");

            if (SawData.SawEncyclopediaPassiveItem.Count < Collection.instance.ItemDB.Entities.Count)  //새로 추가된 패시브가 없는지 확인
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


        // 엑티브 아이템
        if (ES3.KeyExists(SaveKey + "ActiveItem", "./SaveData/EncyclopediaData.es3"))
        {
            SawData.SawEncyclopediaActiveItem = ES3.Load<List<bool>>(SaveKey + "ActiveItem", "./SaveData/EncyclopediaData.es3");

            if (SawData.SawEncyclopediaActiveItem.Count < Collection.instance.ItemDB.Active.Count)     //새로 추가된 액티브 확인
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

        DebugX.Log("도감 데이터 저장");
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
            SawData.SawEncyclopediaCard[(card[i] - 1)] = true;        //-1 하는 이유는 인덱스 번호가 아닌 시리얼 번호가 들어가 있음
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

    void OnApplicationQuit()    //게임 꺼질 때 데이터 저장
    {
        DataSave();
    }
}
