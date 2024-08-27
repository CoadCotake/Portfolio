using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// 
/// 도감 구성
/// 패널 - 큰 카테고리 (ex - 아이템, 카드)
/// 페이지 - 세부 카테고리 (ex - 패시브 아이템, 액티브 아이템)
/// 슬롯 - 기본 요소 (ex - 하트여왕의 선언서, 까마귀의 동전)
/// 
/// 도감 생성 방식
/// 1. 데이터를 읽어 페이지 및 슬롯 생성
///  1-1. '세부 카테고리' 개수만큼 해당되는 '패널'안에 '페이지' 생성
///  1-2. 전체적인 데이터를 읽고 데이터의 '세부 카테고리'에 따라 해당되는 '페이지' 슬롯 생성 및 데이터 삽입
///  *여기까지 진행 될 경우 데이터만큼 세팅이 완료됨
/// 2. 데이터 적용
///  2-1. 모든 슬롯에 데이터 값이 들어가있음, 모든 슬롯의 setdisplay 함수 실행
/// 

//1. 페이지에 들어갈 데이터 선언
public enum EncyclopediaType
{
    Card, Item
}

//2. 데이터의 종류 선언 *엑셀과 이름이 같아야할 것
public enum EncyclopediaCardType 
{
    Attack, Defend, Heal, Special, end
}
public enum EncyclopediaItemType    //정해진게 없어서 분류때 따로 처리
{
    Passive, Active, end
}

//3. 데이터의 세부 정렬 값 ( ex - 아이템 or 카드는 Tier)
public enum EncyclopediaTier    //열거형은 숫자를 쓰지 못해 따로 인덱스로 처리
{
    Common, UnCommon, Rare, end
}

[System.Serializable]
public class OrderData      //정렬처리된 데이터들
{
    public List<ComcardDBEntity> OrdercomcardDB=new List<ComcardDBEntity>();    //조합카드
    public List<ItemDBEntity> Orderitempassive= new List<ItemDBEntity>();   //패시브 아이템
    public List<ItemDBActive> Orderitemactive = new List<ItemDBActive>();   //액티브 아이템
}

[System.Serializable]
public class EncyclopediaSet
{
    [Header("페이지 셋팅")]
    public EncyclopediaType Type;

    [Header("------------직접 넣어야 할 것------------")]
    [Header("도감 페이지")]
    public GameObject Page;

    [Header("도감 페이지 프리펩")]
    public GameObject PagePrefab;

    [Header("슬롯 프리펩")]
    public GameObject SlotPrefab;

    [Header("한 줄당 들어갈 슬롯 개수")]
    public int OneSlotNumber;

    [Header("툴팁")]
    public EncyclopediaToolTip ToolTip;
    [Header("---------------------------------------")]


    [Header("페이지 리스트 (*생성 후)")]
    public List<EncyclopediaPage> PageList = new List<EncyclopediaPage>();

    [Header("슬롯 리스트 (*생성 후)")]
    public List<EncyclopediaSlot> SlotList = new List<EncyclopediaSlot>();
    
}

public class Encyclopedia : MonoBehaviour
{
    [Header("------------직접 넣어야 할 것------------")]

    [Header("도감 리스트 *Type순으로 정렬해놓기")]
    public List<EncyclopediaSet> EncyclopediaSet = new List<EncyclopediaSet>();

    [Header("드롭다운")]
    public Dropdown DropDownType;
    public Dropdown DropDownTier;

    [Header("데이터들")]
    public TextDB textDB;

    [Header("엔드리스 버튼 텍스트")]
    public TextMeshProUGUI EndLessButtonText;

    [Header("------------------------------------")]

    [Header("정렬 처리된 데이터들")]
    public OrderData OrderCommenData = new OrderData();
    //public OrderData OrderEndLessData = new OrderData();

    [Header("선택한 페이지")]
    public EncyclopediaType ChoicePageType;

    [Header("선택한 드롭다운 *-1은 전체 값")]
    public int ChoiceDropDownType;
    public int ChoiceDropDownTier;

    [Header("선택한 슬롯 (체크 해제때 사용)")]
    public EncyclopediaSlot ChoiceSlot;    

    public static Encyclopedia instance = null;

    #region 데이터 우선순위
    //카드는 데이터안에 종류가 다 있어서 따로 우선순위 정하기
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

        //instance 생성
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //데이터 정렬
        SplitCardData();
        SplitItemData();

        //페이지 설정
        SettingEncyclopedia();        
    }

    public void SettingEncyclopedia()
    {
        // 슬롯 생성
        SettingSlotCard(); //카드
        SettingSlotItem(); //아이템

        // 슬롯 데이터 적용
        SettingData(EncyclopediaType.Card);
        SettingData(EncyclopediaType.Item);
    }


    #region 데이터 정렬
    public void SplitCardData()
    {
        //일반
            OrderCommenData.OrdercomcardDB = Collection.instance.GetCardDB().comcard
           .OrderBy(c => CardPriority[c.Type])     // 첫 번째 정렬 기준: 카드 종류 우선순위
           .ThenBy(c => c.Tier)                    // 두 번째 정렬 기준: 티어 우선순위
           .ThenBy(c => c.serialnum)               // 세 번째 정렬 기준: 인덱스 번호
           .ToList();
    }

    public void SplitItemData()
    {
        //일반
        //패시브
        OrderCommenData.Orderitempassive = Collection.instance.GetItemDB().Entities
       .OrderBy(i => i.tier)     // 첫 번째 정렬 기준: 티어 종류 우선순위
       .ThenBy(i => i.num) // 두 번째 정렬 기준: 인덱스 우선순위
       .ToList();

        //액티브
        OrderCommenData.Orderitemactive = Collection.instance.GetItemDB().Active
       .OrderBy(i => i.tier)     // 첫 번째 정렬 기준: 티어 우선순위
       .ThenBy(i => i.num) // 두 번째 정렬 기준: 인덱스 우선순위
       .ToList();
    }

    #endregion

    #region 생성 함수들

    public void SettingSlotCard(bool IsEnd = false)       //페이지 생성 및 슬롯 생성 
    {
        EncyclopediaSet setcard = EncyclopediaSet[(int)EncyclopediaType.Card];       

        //페이지 생성
        for (int i=0; i < (int)EncyclopediaCardType.end;i++)
        {
            GameObject ins = Instantiate(setcard.PagePrefab, setcard.Page.transform);
            setcard.PageList.Add(ins.GetComponent<EncyclopediaPage>());
        }

        //슬롯 생성
        for(int i=0; i < OrderCommenData.OrdercomcardDB.Count; i++)
        {
            if (FindTypeCard(OrderCommenData.OrdercomcardDB[i].Type) != -1)      //??? 값 안들어가게
            {
                EncyclopediaSlot ins = Instantiate(setcard.SlotPrefab, setcard.PageList[FindTypeCard(OrderCommenData.OrdercomcardDB[i].Type)].transform).GetComponent<EncyclopediaSlot>();   //슬롯생성
                setcard.PageList[FindTypeCard(OrderCommenData.OrdercomcardDB[i].Type)].GetComponent<EncyclopediaPage>().PageSet(ins, OrderCommenData.OrdercomcardDB[i].serialnum - 1, setcard.OneSlotNumber); //페이지 설정
                ins.Setting(OrderCommenData.OrdercomcardDB[i].serialnum-1,i, setcard.ToolTip);   //슬롯안 데이터 셋팅                
                setcard.SlotList.Add(ins);  //리스트 추가
            }
        }

        //페이지 height 설정
        for (int i = 0; i < (int)EncyclopediaCardType.end; i++)
        {
            setcard.PageList[i].SetPageHeight();
        }
    }

    public void SettingSlotItem(bool IsEnd=false)   //페이지 생성 및 슬롯 생성 
    {
        EncyclopediaSet setitem = EncyclopediaSet[(int)EncyclopediaType.Item];

        //페이지 생성
        for (int i = 0; i < (int)EncyclopediaItemType.end; i++)
        {
            GameObject ins = Instantiate(setitem.PagePrefab, setitem.Page.transform);
            setitem.PageList.Add(ins.GetComponent<EncyclopediaPage>());
        }

        //패시브 슬롯 생성
        for (int i = 0; i < OrderCommenData.Orderitempassive.Count; i++)
        {
            if (OrderCommenData.Orderitempassive[i].type != 99)      //데이터만 존재하는 아이템이 안들어가도록
            {
                EncyclopediaSlot ins = Instantiate(setitem.SlotPrefab, setitem.PageList[(int)EncyclopediaItemType.Passive].transform).GetComponent<EncyclopediaSlot>();
                setitem.PageList[(int)EncyclopediaItemType.Passive].GetComponent<EncyclopediaPage>().PageSet(ins, OrderCommenData.Orderitempassive[i].num, setitem.OneSlotNumber);
                ins.Setting(OrderCommenData.Orderitempassive[i].num, i, setitem.ToolTip, "Passive");
                setitem.SlotList.Add(ins);
            }
        }

        //엑티브 슬롯 생성
        for (int i = 0; i < OrderCommenData.Orderitemactive.Count; i++)
        {
            EncyclopediaSlot ins = Instantiate(setitem.SlotPrefab, setitem.PageList[(int)EncyclopediaItemType.Active].transform).GetComponent<EncyclopediaSlot>();
            setitem.PageList[(int)EncyclopediaItemType.Active].GetComponent<EncyclopediaPage>().PageSet(ins, OrderCommenData.Orderitemactive[i].num, setitem.OneSlotNumber);
            ins.Setting(OrderCommenData.Orderitemactive[i].num,i, setitem.ToolTip, "Active");
            setitem.SlotList.Add(ins);
        }

        //페이지 height 설정
        for (int i = 0; i < (int)EncyclopediaItemType.end; i++)
        {
            setitem.PageList[i].SetPageHeight();
        }
    }
    #endregion

    #region 페이지 세팅
    public void SettingData(EncyclopediaType Type)  //슬롯 데이터 넣기
    {
        for(int i=0; i < EncyclopediaSet[(int)Type].SlotList.Count; i++)
        {
            EncyclopediaSet[(int)Type].SlotList[i].SetDisPlaySlot();
        }
    }

    public void SettingEncylopediaType(EncyclopediaType type)       //타입에 따른 전체적인 페이지 셋팅
    {
        ChoicePageType = type;  //선택한 페이지 값 저장

        EncyclopediaSet set = EncyclopediaSet[(int)type];

        for (int i=0; i< EncyclopediaSet.Count; i++)     //전체 페이지 및 툴팁 끄기
        {
            EncyclopediaSet[i].Page.SetActive(false);
            EncyclopediaSet[i].ToolTip.gameObject.SetActive(false);
        }

        for (int i = 0; i < set.PageList.Count; i++)     //슬롯 자식 페이지 넓이 셋팅
        {
            set.PageList[i].SetPageHeight();
        }

        switch (type)       //드롭다운 옵션 추가    *숫자는 번역 엑셀 UI부분 참고
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

        //기본 값 - 전체
        ChoiceDropDownType = -1;
        ChoiceDropDownTier = -1;

        ChoiceDropDwon();   //선택에 따른 페이지 변경 및 넓이 변경

        set.Page.SetActive(true);    //클릭한 페이지 켜기
    }

    public void AddDropOptions<T>(Dropdown dropdown,int TransNum) where T : Enum     //다운박스 페이지 종류에 따라 텍스트 바꾸기
    {
        dropdown.ClearOptions();

        List<string> options = new List<string>();

        options.Add("All");     //열거형에 '전체' 값 없어서 따로 추가하기

        // Enum의 모든 값을 반복하여 문자열로 변환하여 옵션으로 추가
        foreach (T value in Enum.GetValues(typeof(T)))
        {   
            options.Add(value.ToString());
        }
        options.RemoveAt(options.Count - 1);        //마지막 값은 end, 끝 값 알아내는 용도라 제거
        
        Convert(options,TransNum);   //번역처리
        
        dropdown.AddOptions(options);
    }

    #region 번역 함수    
    public void RefreshTransformDropOptions()
    {
        //'전체' 값 번역
        DropDownType.options[0].text = ReturnTranslateTextUI(77);
        DropDownTier.options[0].text = ReturnTranslateTextUI(77);

        switch (ChoicePageType)       //    *숫자는 번역 엑셀 UI부분 참고
        {
            case EncyclopediaType.Card:
                for(int i=1;i < DropDownType.options.Count;i++)
                {
                    DropDownType.options[i].text = ReturnTranslateTextUI(77 + i);   //78시작인데 i가 1이라서
                }
                for (int i = 1; i < DropDownTier.options.Count; i++)
                {
                    DropDownTier.options[i].text = ReturnTranslateTextUI(83 + i);   //84시작인데 i가 1이라서
                }
                break;
            case EncyclopediaType.Item:
                for (int i = 1; i < DropDownType.options.Count; i++)
                {
                    DropDownType.options[i].text = ReturnTranslateTextUI(81 + i);   //82시작인데 i가 1이라서
                }
                for (int i = 1; i < DropDownTier.options.Count; i++)
                {
                    DropDownTier.options[i].text = ReturnTranslateTextUI(83 + i);   //84시작인데 i가 1이라서
                }
                break;
        }

        //드롭옵션 적용
        DropDownType.captionText.text = DropDownType.options[DropDownType.value].text;
        DropDownTier.captionText.text = DropDownTier.options[DropDownTier.value].text;
    }


    // 리스트의 영어 문자열을 한글로 변환하는 함수
    public void Convert(List<string> Strings, int TransNum)
    {
        Strings[0] = ReturnTranslateTextUI(77);     //'전체'값 번역

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

    public void ChoiceDropDwon()        //드롭다운 선택할 때 실행됨
    {
        // 전체 -1 , 0~n 벨류 값
        ChoiceDropDownType = DropDownType.value-1;
        ChoiceDropDownTier = DropDownTier.value-1;

        EncyclopediaSet set = EncyclopediaSet[(int)ChoicePageType];

        //분류
        if(ChoiceDropDownType == -1)    //전체
        {
            for(int i=0; i < set.PageList.Count; i++)
            {
                set.PageList[i].SetOpenPage(ChoiceDropDownTier);
            }
        }
        else
        {
            for (int i = 0; i < set.PageList.Count; i++)    //설정된 분류만 켜기
            {
                if (i != ChoiceDropDownType)
                {
                    set.PageList[i].SetClosePage();
                }
                else
                {
                    set.PageList[i].SetOpenPage(ChoiceDropDownTier); //이것만 켜기
                }
            }
        }

        //페이지 넓이 셋팅
        for (int i = 0; i < set.PageList.Count; i++)     //슬롯 자식 페이지 넓이 셋팅
        {
            set.PageList[i].SetPageHeight();
        }

        set.Page.GetComponent<EncyclopediaPageSize>().SetContentHeight();   //클릭한 전체 페이지 넓이 셋팅
    }
    #endregion

    
    public int FindTypeCard(string Type)    //해당되는 타입값 찾기 (카드)
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
                DebugX.LogError("슬롯 타입 값이 해당안됨");
                return -1;
        }
    }

    public void OpenPage()      //페이지 열기 (버튼 할당)
    {
        this.gameObject.SetActive(true);
        RefreshTransformDropOptions();      //번역 새로고침
    }

    public void ClosePage()      //페이지 닫기 (버튼 할당)
    {
        this.gameObject.SetActive(false);
    }

    public void RemoveCheckSlot()   //페이지 변경시 슬롯 체크 해제하기
    {
        if (ChoiceSlot != null)
        {
            ChoiceSlot.Check.enabled = false;
        }
    }

    public void EndLessModeOnOff()     //엔드리스 도감 켬/끔
    {

    }        
}
