using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// 
/// ���� ����
/// �г� - ū ī�װ� (ex - ������, ī��)
/// ������ - ���� ī�װ� (ex - �нú� ������, ��Ƽ�� ������)
/// ���� - �⺻ ��� (ex - ��Ʈ������ ����, ����� ����)
/// 
/// ���� ���� ���
/// 1. �����͸� �о� ������ �� ���� ����
///  1-1. '���� ī�װ�' ������ŭ �ش�Ǵ� '�г�'�ȿ� '������' ����
///  1-2. ��ü���� �����͸� �а� �������� '���� ī�װ�'�� ���� �ش�Ǵ� '������' ���� ���� �� ������ ����
///  *������� ���� �� ��� �����͸�ŭ ������ �Ϸ��
/// 2. ������ ����
///  2-1. ��� ���Կ� ������ ���� ������, ��� ������ setdisplay �Լ� ����
/// 

//1. �������� �� ������ ����
public enum EncyclopediaType
{
    Card, Item
}

//2. �������� ���� ���� *������ �̸��� ���ƾ��� ��
public enum EncyclopediaCardType 
{
    Attack, Defend, Heal, Special, end
}
public enum EncyclopediaItemType    //�������� ��� �з��� ���� ó��
{
    Passive, Active, end
}

//3. �������� ���� ���� �� ( ex - ������ or ī��� Tier)
public enum EncyclopediaTier    //�������� ���ڸ� ���� ���� ���� �ε����� ó��
{
    Common, UnCommon, Rare, end
}

[System.Serializable]
public class OrderData      //����ó���� �����͵�
{
    public List<ComcardDBEntity> OrdercomcardDB=new List<ComcardDBEntity>();    //����ī��
    public List<ItemDBEntity> Orderitempassive= new List<ItemDBEntity>();   //�нú� ������
    public List<ItemDBActive> Orderitemactive = new List<ItemDBActive>();   //��Ƽ�� ������
}

[System.Serializable]
public class EncyclopediaSet
{
    [Header("������ ����")]
    public EncyclopediaType Type;

    [Header("------------���� �־�� �� ��------------")]
    [Header("���� ������")]
    public GameObject Page;

    [Header("���� ������ ������")]
    public GameObject PagePrefab;

    [Header("���� ������")]
    public GameObject SlotPrefab;

    [Header("�� �ٴ� �� ���� ����")]
    public int OneSlotNumber;

    [Header("����")]
    public EncyclopediaToolTip ToolTip;
    [Header("---------------------------------------")]


    [Header("������ ����Ʈ (*���� ��)")]
    public List<EncyclopediaPage> PageList = new List<EncyclopediaPage>();

    [Header("���� ����Ʈ (*���� ��)")]
    public List<EncyclopediaSlot> SlotList = new List<EncyclopediaSlot>();
    
}

public class Encyclopedia : MonoBehaviour
{
    [Header("------------���� �־�� �� ��------------")]

    [Header("���� ����Ʈ *Type������ �����س���")]
    public List<EncyclopediaSet> EncyclopediaSet = new List<EncyclopediaSet>();

    [Header("��Ӵٿ�")]
    public Dropdown DropDownType;
    public Dropdown DropDownTier;

    [Header("�����͵�")]
    public TextDB textDB;

    [Header("���帮�� ��ư �ؽ�Ʈ")]
    public TextMeshProUGUI EndLessButtonText;

    [Header("------------------------------------")]

    [Header("���� ó���� �����͵�")]
    public OrderData OrderCommenData = new OrderData();
    //public OrderData OrderEndLessData = new OrderData();

    [Header("������ ������")]
    public EncyclopediaType ChoicePageType;

    [Header("������ ��Ӵٿ� *-1�� ��ü ��")]
    public int ChoiceDropDownType;
    public int ChoiceDropDownTier;

    [Header("������ ���� (üũ ������ ���)")]
    public EncyclopediaSlot ChoiceSlot;    

    public static Encyclopedia instance = null;

    #region ������ �켱����
    //ī��� �����;ȿ� ������ �� �־ ���� �켱���� ���ϱ�
    Dictionary<string, int> CardPriority = new Dictionary<string, int>
        {
            { "Attack", 1 },
            { "Deffend", 2 },
            { "Heal", 3 },
            { "Buff", 4 },
            { "DeBuff", 5 },
            { "FAL", 6 }
        };
    #endregion

    public void AwakeFunction()
    {
        this.gameObject.SetActive(false);

        //instance ����
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //������ ����
        SplitCardData();
        SplitItemData();

        //������ ����
        SettingEncyclopedia();        
    }

    public void SettingEncyclopedia()
    {
        // ���� ����
        SettingSlotCard(); //ī��
        SettingSlotItem(); //������

        // ���� ������ ����
        SettingData(EncyclopediaType.Card);
        SettingData(EncyclopediaType.Item);
    }


    #region ������ ����
    public void SplitCardData()
    {
        //�Ϲ�
            OrderCommenData.OrdercomcardDB = Collection.instance.GetCardDB().comcard
           .OrderBy(c => CardPriority[c.Type])     // ù ��° ���� ����: ī�� ���� �켱����
           .ThenBy(c => c.Tier)                    // �� ��° ���� ����: Ƽ�� �켱����
           .ThenBy(c => c.serialnum)               // �� ��° ���� ����: �ε��� ��ȣ
           .ToList();
    }

    public void SplitItemData()
    {
        //�Ϲ�
        //�нú�
        OrderCommenData.Orderitempassive = Collection.instance.GetItemDB().Entities
       .OrderBy(i => i.tier)     // ù ��° ���� ����: Ƽ�� ���� �켱����
       .ThenBy(i => i.num) // �� ��° ���� ����: �ε��� �켱����
       .ToList();

        //��Ƽ��
        OrderCommenData.Orderitemactive = Collection.instance.GetItemDB().Active
       .OrderBy(i => i.tier)     // ù ��° ���� ����: Ƽ�� �켱����
       .ThenBy(i => i.num) // �� ��° ���� ����: �ε��� �켱����
       .ToList();
    }

    #endregion

    #region ���� �Լ���

    public void SettingSlotCard(bool IsEnd = false)       //������ ���� �� ���� ���� 
    {
        EncyclopediaSet setcard = EncyclopediaSet[(int)EncyclopediaType.Card];       

        //������ ����
        for (int i=0; i < (int)EncyclopediaCardType.end;i++)
        {
            GameObject ins = Instantiate(setcard.PagePrefab, setcard.Page.transform);
            setcard.PageList.Add(ins.GetComponent<EncyclopediaPage>());
        }

        //���� ����
        for(int i=0; i < OrderCommenData.OrdercomcardDB.Count; i++)
        {
            if (FindTypeCard(OrderCommenData.OrdercomcardDB[i].Type) != -1)      //??? �� �ȵ���
            {
                EncyclopediaSlot ins = Instantiate(setcard.SlotPrefab, setcard.PageList[FindTypeCard(OrderCommenData.OrdercomcardDB[i].Type)].transform).GetComponent<EncyclopediaSlot>();   //���Ի���
                setcard.PageList[FindTypeCard(OrderCommenData.OrdercomcardDB[i].Type)].GetComponent<EncyclopediaPage>().PageSet(ins, OrderCommenData.OrdercomcardDB[i].serialnum - 1, setcard.OneSlotNumber); //������ ����
                ins.Setting(OrderCommenData.OrdercomcardDB[i].serialnum-1,i, setcard.ToolTip);   //���Ծ� ������ ����                
                setcard.SlotList.Add(ins);  //����Ʈ �߰�
            }
        }

        //������ height ����
        for (int i = 0; i < (int)EncyclopediaCardType.end; i++)
        {
            setcard.PageList[i].SetPageHeight();
        }
    }

    public void SettingSlotItem(bool IsEnd=false)   //������ ���� �� ���� ���� 
    {
        EncyclopediaSet setitem = EncyclopediaSet[(int)EncyclopediaType.Item];

        //������ ����
        for (int i = 0; i < (int)EncyclopediaItemType.end; i++)
        {
            GameObject ins = Instantiate(setitem.PagePrefab, setitem.Page.transform);
            setitem.PageList.Add(ins.GetComponent<EncyclopediaPage>());
        }

        //�нú� ���� ����
        for (int i = 0; i < OrderCommenData.Orderitempassive.Count; i++)
        {
            if (OrderCommenData.Orderitempassive[i].type != 99)      //�����͸� �����ϴ� �������� �ȵ�����
            {
                EncyclopediaSlot ins = Instantiate(setitem.SlotPrefab, setitem.PageList[(int)EncyclopediaItemType.Passive].transform).GetComponent<EncyclopediaSlot>();
                setitem.PageList[(int)EncyclopediaItemType.Passive].GetComponent<EncyclopediaPage>().PageSet(ins, OrderCommenData.Orderitempassive[i].num, setitem.OneSlotNumber);
                ins.Setting(OrderCommenData.Orderitempassive[i].num, i, setitem.ToolTip, "Passive");
                setitem.SlotList.Add(ins);
            }
        }

        //��Ƽ�� ���� ����
        for (int i = 0; i < OrderCommenData.Orderitemactive.Count; i++)
        {
            EncyclopediaSlot ins = Instantiate(setitem.SlotPrefab, setitem.PageList[(int)EncyclopediaItemType.Active].transform).GetComponent<EncyclopediaSlot>();
            setitem.PageList[(int)EncyclopediaItemType.Active].GetComponent<EncyclopediaPage>().PageSet(ins, OrderCommenData.Orderitemactive[i].num, setitem.OneSlotNumber);
            ins.Setting(OrderCommenData.Orderitemactive[i].num,i, setitem.ToolTip, "Active");
            setitem.SlotList.Add(ins);
        }

        //������ height ����
        for (int i = 0; i < (int)EncyclopediaItemType.end; i++)
        {
            setitem.PageList[i].SetPageHeight();
        }
    }
    #endregion

    #region ������ ����
    public void SettingData(EncyclopediaType Type)  //���� ������ �ֱ�
    {
        for(int i=0; i < EncyclopediaSet[(int)Type].SlotList.Count; i++)
        {
            EncyclopediaSet[(int)Type].SlotList[i].SetDisPlaySlot();
        }
    }

    public void SettingEncylopediaType(EncyclopediaType type)       //Ÿ�Կ� ���� ��ü���� ������ ����
    {
        ChoicePageType = type;  //������ ������ �� ����

        EncyclopediaSet set = EncyclopediaSet[(int)type];

        for (int i=0; i< EncyclopediaSet.Count; i++)     //��ü ������ �� ���� ����
        {
            EncyclopediaSet[i].Page.SetActive(false);
            EncyclopediaSet[i].ToolTip.gameObject.SetActive(false);
        }

        for (int i = 0; i < set.PageList.Count; i++)     //���� �ڽ� ������ ���� ����
        {
            set.PageList[i].SetPageHeight();
        }

        switch (type)       //��Ӵٿ� �ɼ� �߰�    *���ڴ� ���� ���� UI�κ� ����
        {
            case EncyclopediaType.Card:
                AddDropOptions<EncyclopediaCardType>(DropDownType,78);
                AddDropOptions<EncyclopediaTier>(DropDownTier,84);
                break;
            case EncyclopediaType.Item:
                AddDropOptions<EncyclopediaItemType>(DropDownType,82);
                AddDropOptions<EncyclopediaTier>(DropDownTier,84);
                break;
        }
        //AddDropOptions<EncyclopediaTier>(DropDownTier);

        //�⺻ �� - ��ü
        ChoiceDropDownType = -1;
        ChoiceDropDownTier = -1;

        ChoiceDropDwon();   //���ÿ� ���� ������ ���� �� ���� ����

        set.Page.SetActive(true);    //Ŭ���� ������ �ѱ�
    }

    public void AddDropOptions<T>(Dropdown dropdown,int TransNum) where T : Enum     //�ٿ�ڽ� ������ ������ ���� �ؽ�Ʈ �ٲٱ�
    {
        dropdown.ClearOptions();

        List<string> options = new List<string>();

        options.Add("All");     //�������� '��ü' �� ��� ���� �߰��ϱ�

        // Enum�� ��� ���� �ݺ��Ͽ� ���ڿ��� ��ȯ�Ͽ� �ɼ����� �߰�
        foreach (T value in Enum.GetValues(typeof(T)))
        {   
            options.Add(value.ToString());
        }
        options.RemoveAt(options.Count - 1);        //������ ���� end, �� �� �˾Ƴ��� �뵵�� ����
        
        Convert(options,TransNum);   //����ó��
        
        dropdown.AddOptions(options);
    }

    #region ���� �Լ�    
    public void RefreshTransformDropOptions()
    {
        //'��ü' �� ����
        DropDownType.options[0].text = ReturnTranslateTextUI(77);
        DropDownTier.options[0].text = ReturnTranslateTextUI(77);

        switch (ChoicePageType)       //    *���ڴ� ���� ���� UI�κ� ����
        {
            case EncyclopediaType.Card:
                for(int i=1;i < DropDownType.options.Count;i++)
                {
                    DropDownType.options[i].text = ReturnTranslateTextUI(77 + i);   //78�����ε� i�� 1�̶�
                }
                for (int i = 1; i < DropDownTier.options.Count; i++)
                {
                    DropDownTier.options[i].text = ReturnTranslateTextUI(83 + i);   //84�����ε� i�� 1�̶�
                }
                break;
            case EncyclopediaType.Item:
                for (int i = 1; i < DropDownType.options.Count; i++)
                {
                    DropDownType.options[i].text = ReturnTranslateTextUI(81 + i);   //82�����ε� i�� 1�̶�
                }
                for (int i = 1; i < DropDownTier.options.Count; i++)
                {
                    DropDownTier.options[i].text = ReturnTranslateTextUI(83 + i);   //84�����ε� i�� 1�̶�
                }
                break;
        }

        //��ӿɼ� ����
        DropDownType.captionText.text = DropDownType.options[DropDownType.value].text;
        DropDownTier.captionText.text = DropDownTier.options[DropDownTier.value].text;
    }


    // ����Ʈ�� ���� ���ڿ��� �ѱ۷� ��ȯ�ϴ� �Լ�
    public void Convert(List<string> Strings, int TransNum)
    {
        Strings[0] = ReturnTranslateTextUI(77);     //'��ü'�� ����

        for (int i = 1; i < Strings.Count; i++)
        {
            Strings[i] = ReturnTranslateTextUI(TransNum+i-1);
        }
    }

    private string ReturnTranslateTextUI(int textnum)
    {
        if (textnum == -1)
            return null;

        switch (Collection.instance.textLanguage)
        {
            case 0:
                return textDB.UI[textnum].Kr;
            case 1:
                return textDB.UI[textnum].En;
            case 2:
                return textDB.UI[textnum].Jp;
            case 3:
                return textDB.UI[textnum].Cn;
            default:
                return textDB.UI[textnum].En;
        }
    }
    #endregion

    public void ChoiceDropDwon()        //��Ӵٿ� ������ �� �����
    {
        // ��ü -1 , 0~n ���� ��
        ChoiceDropDownType = DropDownType.value-1;
        ChoiceDropDownTier = DropDownTier.value-1;

        EncyclopediaSet set = EncyclopediaSet[(int)ChoicePageType];

        //�з�
        if(ChoiceDropDownType == -1)    //��ü
        {
            for(int i=0; i < set.PageList.Count; i++)
            {
                set.PageList[i].SetOpenPage(ChoiceDropDownTier);
            }
        }
        else
        {
            for (int i = 0; i < set.PageList.Count; i++)    //������ �з��� �ѱ�
            {
                if (i != ChoiceDropDownType)
                {
                    set.PageList[i].SetClosePage();
                }
                else
                {
                    set.PageList[i].SetOpenPage(ChoiceDropDownTier); //�̰͸� �ѱ�
                }
            }
        }

        //������ ���� ����
        for (int i = 0; i < set.PageList.Count; i++)     //���� �ڽ� ������ ���� ����
        {
            set.PageList[i].SetPageHeight();
        }

        set.Page.GetComponent<EncyclopediaPageSize>().SetContentHeight();   //Ŭ���� ��ü ������ ���� ����
    }
    #endregion

    
    public int FindTypeCard(string Type)    //�ش�Ǵ� Ÿ�԰� ã�� (ī��)
    {

        switch (Type)
        {
            case "Attack":
                return (int)EncyclopediaCardType.Attack;
            case "Deffend":
                return (int)EncyclopediaCardType.Defend;
            case "Heal":
                return (int)EncyclopediaCardType.Heal;
            case "Buff":
                return (int)EncyclopediaCardType.Special;
            case "DeBuff":
                return (int)EncyclopediaCardType.Special;
            default:
                DebugX.LogError("���� Ÿ�� ���� �ش�ȵ�");
                return -1;
        }
    }

    public void OpenPage()      //������ ���� (��ư �Ҵ�)
    {
        this.gameObject.SetActive(true);
        RefreshTransformDropOptions();      //���� ���ΰ�ħ
    }

    public void ClosePage()      //������ �ݱ� (��ư �Ҵ�)
    {
        this.gameObject.SetActive(false);
    }

    public void RemoveCheckSlot()   //������ ����� ���� üũ �����ϱ�
    {
        if (ChoiceSlot != null)
        {
            ChoiceSlot.Check.enabled = false;
        }
    }

    public void EndLessModeOnOff()     //���帮�� ���� ��/��
    {

    }        
}
