using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 어느 씬이든 변수 or 함수를 가져올수 있도록 만든 스크립트
/// 주제는 정해져 있지 않으며 용도만 맞으면 선언해서 사용하면 됨
/// *주의* 씬 시작시 2번째로 셋팅이 빠른 스크립트임 
/// [사실상 제일 빠름]
/// </summary>

#region 데이터 클래스
[System.Serializable]
public class ItemDB
{
    public List<ItemDBEntity> Entities;
    public List<ItemDBActive> Active;

    public ItemDB(List<ItemDBEntity> entities, List<ItemDBActive> active)
    {
        Entities = entities;
        Active = active;
    }
}

[System.Serializable]
public class ComcardDB
{
    public List<ComcardDBEntity> comcard;

    public ComcardDB(List<ComcardDBEntity> comcard_)
    {
        comcard = comcard_;
    }
}
#endregion

#region 글꼴 클래스
public enum Language
{
    Korean, English, Japanese
}

[System.Serializable]
public class LanguageFont
{
    public Language language;
    public Font NomalFont;
    /// <summary>
    /// 0 = 기본, 1 = 아웃라인 약함, 2 = 아웃라인 쌤 
    /// </summary>
    public TMP_FontAsset[] TMP_List= new TMP_FontAsset[3];

    public int FontFindNumber(string name)
    {
        if(name.Contains("_아웃라인쌤"))
        {
            return 2;
        }
        else if(name.Contains("_아웃라인약함"))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
#endregion

public class Collection : MonoBehaviour
{    
    static public Collection instance;

    [Header("테스트 할 때 체크하기")]
    public bool Test=false;

    [Header("데이터 직접 넣기")]
    public ItemDB_Commen itemDB_origin;
    public ComcardDB_Commen comcardDB_origin;    

    [Header("한 클래스로 통합된 데이터들")]
    [SerializeField] ItemDB itemDB;    
    [SerializeField] ComcardDB comcardDB;    

    [Header("카드 이미지들 넣기 (언어에따라 다름)")]
    [SerializeField] Sprite[] cardtier_KR;
    [SerializeField] Sprite[] cardtier_En;
    [SerializeField] Sprite[] cardtier_Jp;

    [Header("나라별 폰트 넣기 (한국, 영어는 빈공간으로 유지)")]
    public List<LanguageFont> Fonts = new List<LanguageFont>();

    /// <summary>
    /// 0=한국어, 1= 영어, 2=일본어
    /// </summary>
    public int textLanguage;

    public Sprite[] CardTier
    {
        get
        {
            switch (textLanguage)
            {
                case 0:
                    return cardtier_KR;
                case 1:
                    return cardtier_En;
                case 2:
                    return cardtier_Jp;
                default:
                    return cardtier_KR;
            }
        }
    }
    
    public ItemDB ItemDB
    {
        get
        {
            return itemDB;            
        }
    }
    public ComcardDB ComcardDB
    {
        get
        {
            return comcardDB;            
        }
    }

    public ItemDB GetItemDB(bool Endless=false)
    {
        return itemDB;        
    }

    public ComcardDB GetCardDB(bool Endless=false)
    {
        return comcardDB;        
    }

    public void AwakeFunction()
    {
        if (SceneManager.GetActiveScene().name == "Main" || Test)
        {
            DataMake();

            DontDestroyOnLoad(gameObject);

            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }
    }

    public void DataMake()      //여러버전 데이터 통합
    {
        itemDB = new ItemDB(itemDB_origin.Entities, itemDB_origin.Active);        
        comcardDB = new ComcardDB(comcardDB_origin.comcard);        
    }
}
