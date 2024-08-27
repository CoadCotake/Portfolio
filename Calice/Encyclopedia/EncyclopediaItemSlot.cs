using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaItemSlot : EncyclopediaSlot
{
    [Header("슬롯 아이템 변수")]
    public Image image;
    public EncyclopediaItemType type;

    EncyclopediaItemToolTip tooltip;

    override public void Setting(int num,int slotnum,EncyclopediaToolTip tooltip, string plus = "")   //plus 변수 = 추가사항 (ex - 아이템처럼 한곳에 모여있지 않고 처음부터 데이터가 종류별로 분할되어 있는 경우 같은 변수들 처리하는)
    {
        base.Setting(num, slotnum, tooltip, plus);

        if (plus.Equals("Passive"))
        {
            type = EncyclopediaItemType.Passive;
        }
        else
        {
            type = EncyclopediaItemType.Active;
        }

        this.tooltip = tooltip.GetComponent<EncyclopediaItemToolTip>();
    }
    override public void SetDisPlaySlot()       //데이터 기반으로 슬롯 이미지 및 텍스트 설정
    {
        base.SetDisPlaySlot();

        switch (type)
        {
            case EncyclopediaItemType.Passive:
                image.sprite = GameManager.instance.ItemSpriteList[Num];
                break;
            case EncyclopediaItemType.Active:
                image.sprite = GameManager.instance.ItemSpriteList_act[Num];
                break;
        }

        SetSaw();
    }

    override public void ClickUpEvent()   //클릭 함수
    {
        base.ClickUpEvent();
        tooltip.type = type;
        ToolTip.Display(Num,this); 
    }

    override public void ClickDownEvent()   //클릭 함수
    {
        base.ClickDownEvent();
        
    }

    override public void SetSplitSlot(int type)       //정렬 값에 따라 슬롯 켜짐/끔 여부 결정      -1은 전체 값
    {
        switch (this.type)
        {
            case EncyclopediaItemType.Passive:
                
                if (type == -1 || Collection.instance.GetItemDB().Entities[Num].tier == (type + 1))
                {
                    SetOpenSlot();
                }
                else
                {
                    SetCloseSlot();
                }

                break;
            case EncyclopediaItemType.Active:
                
                if (type == -1 || Collection.instance.GetItemDB().Active[Num].tier == (type + 1))
                {
                    SetOpenSlot();
                }
                else
                {
                    SetCloseSlot();
                }
                break;
        }   
    }

    override public void SetSaw()    //도감 데이터 여부에 따라 슬롯 ??? 또는 보여줌 세팅
    {
        switch (type)
        {
            case EncyclopediaItemType.Passive:
                if(EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaPassiveItem[Num])    //활성화 일 경우
                {
                    image.color = new Color(1, 1, 1);
                }
                else
                {
                    image.color = new Color(0, 0, 0);
                }
                break;
            case EncyclopediaItemType.Active:
                if (EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaActiveItem[Num])    //활성화 일 경우
                {
                    image.color = new Color(1, 1, 1);
                }
                else
                {
                    image.color = new Color(0, 0, 0);
                }
                break;
        }
    }
}
