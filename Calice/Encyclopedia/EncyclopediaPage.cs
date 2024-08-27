using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//정렬로 분류된 페이지, 페이지마다 정렬값을 가짐 ex) 카드면 공격,방어.. 아이템이면 패시브 엑티브, 페이지는 정렬로 분류된 자식(슬롯)들을 가지고 있음

public class EncyclopediaPage : MonoBehaviour       
{
    public RectTransform RectTransform;
    public GridLayoutGroup GridLayoutGroup;
    public int SumSlotHeight; //슬롯 하나 공간 크기

    [Header("확장 옵션")]
    public List<EncyclopediaSlot> Slots = new List<EncyclopediaSlot>();     //타입에 분류 된 슬롯들
    public List<int> SlotNums = new List<int>();    //슬롯안에 들어가있는 인덱스들
    public int LinetoSlotNum;  //한 라인에 슬롯 몇개가 들어가는 지
    public int ChildCount;  //활성화된 자식 수 

    public void GetChildCount()     //현재 켜져있는 페이지 체크 (*자동으로 넓이 설정할 때 사용)
    {
        ChildCount = 0;

        for (int i =0; i < this.transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                ChildCount++;
            }
        }
    }

    public void SetPageHeight()     //페이지 넓이 설정
    {
        RectTransform = this.GetComponent<RectTransform>();
        GridLayoutGroup = this.GetComponent<GridLayoutGroup>();

        int n;
        GetChildCount();    //활성화된 자식 검색

        if ((ChildCount % LinetoSlotNum) != 0)
        {
            n = (ChildCount / LinetoSlotNum) + 1;
        }
        else
        {
            n = ChildCount / LinetoSlotNum;
        }

        SumSlotHeight = (int)(GridLayoutGroup.cellSize.y + GridLayoutGroup.spacing.y);

        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, (n * SumSlotHeight));
    }

    /// <summary>
    ///  페이지에 슬롯 정보 넣기 / 데이터 인덱스, 슬롯 인덱스
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="num"></param>
    /// <param name="slotnum"></param>
    public void PageSet(EncyclopediaSlot slot, int num, int slotnum)   
    {
        Slots.Add(slot);
        SlotNums.Add(num);
        LinetoSlotNum = slotnum;
    }

    public void SetOpenPage(int tier)   //분류에 쓰이는 켜짐 함수
    {
        SplitSlot(tier);    //분류때 쓰이니 안에 포함된 슬롯들도 분류
        this.gameObject.SetActive(true);
    }

    public void SetClosePage()  //분류에 쓰이는 꺼짐 함수
    {
        this.gameObject.SetActive(false);
    }

    public void SplitSlot(int tier)     //티어 분류 함수  (드롭 다운 선택할 때 실행됨)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].SetSplitSlot(tier);        //슬롯마다 분류 값 확인시켜 켜짐/끔 설정
        }

    }
}
