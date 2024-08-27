using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ��� ���̵� ���� or �Լ��� �����ü� �ֵ��� ���� ��ũ��Ʈ
/// ������ ������ ���� ������ �뵵�� ������ �����ؼ� ����ϸ� ��
/// *����* �� ���۽� 2��°�� ������ ���� ��ũ��Ʈ�� 
/// [��ǻ� ���� ����]
/// </summary>

#region ������ Ŭ����
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

#region �۲� Ŭ����
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
    /// 0 = �⺻, 1 = �ƿ����� ����, 2 = �ƿ����� �� 
    /// </summary>
    public TMP_FontAsset[] TMP_List= new TMP_FontAsset[3];

    public int FontFindNumber(string name)
    {
        if(name.Contains("_�ƿ����ν�"))
        {
            return 2;
        }
        else if(name.Contains("_�ƿ����ξ���"))
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

    [Header("�׽�Ʈ �� �� üũ�ϱ�")]
    public bool Test=false;

    [Header("������ ���� �ֱ�")]
    public ItemDB_Commen itemDB_origin;
    public ComcardDB_Commen comcardDB_origin;    

    [Header("�� Ŭ������ ���յ� �����͵�")]
    [SerializeField] ItemDB itemDB;    
    [SerializeField] ComcardDB comcardDB;    

    [Header("ī�� �̹����� �ֱ� (������ �ٸ�)")]
    [SerializeField] Sprite[] cardtier_KR;
    [SerializeField] Sprite[] cardtier_En;
    [SerializeField] Sprite[] cardtier_Jp;

    [Header("���� ��Ʈ �ֱ� (�ѱ�, ����� ��������� ����)")]
    public List<LanguageFont> Fonts = new List<LanguageFont>();

    /// <summary>
    /// 0=�ѱ���, 1= ����, 2=�Ϻ���
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

    public void DataMake()      //�������� ������ ����
    {
        itemDB = new ItemDB(itemDB_origin.Entities, itemDB_origin.Active);        
        comcardDB = new ComcardDB(comcardDB_origin.comcard);        
    }
}
