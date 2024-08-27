using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EncyclopediaSlot : MonoBehaviour,IPointerUpHandler, IPointerDownHandler
{
    [Header("슬롯 기본 변수")]
    public int Num;
    public int SlotNum;
    public bool ToolTipOn=false;
    public EncyclopediaToolTip ToolTip;

    [Header("슬롯 기본 변수 직접 넣어야 할 것")]
    public Image Check;
    public TextTranslate Head;
    public TextTranslate Body;

    virtual public void Setting(int num, int slotnum, EncyclopediaToolTip tooltip, string plus ="")   //plus 추가사항
    {
        Num = num;
        SlotNum = slotnum;
        ToolTip = tooltip;
        Check.enabled=false;
    }

    virtual public void SetDisPlaySlot()        //슬롯에 데이터에 맞게 이미지와 텍스트 변경
    {
        if (EncyclopediaManager.instance.EndLessOn)
        {
            if (EndlessManager.instance.CheckEndlessItem(Num, true))        //엔드리스에서 사용되지 않는 아이템
            {                
                this.gameObject.SetActive(false);   //도감 비활성화
            }
        }
    }

    virtual public void SetSplitSlot(int type)       //값에 따라 슬롯 켜짐/끔 여부 체크
    {

    }
    virtual public void SetSaw()    //도감 데이터 여부에 따라 슬롯 ??? or 보여짐 세팅
    {
        
    }

    virtual public void SetOpenSlot()
    {
        if (EncyclopediaManager.instance.EndLessOn)
        {
            if (EndlessManager.instance.CheckEndlessItem(Num, true))        //엔드리스에서 사용되지 않는 아이템
            {
                this.gameObject.SetActive(false);   //비활성화
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }

    virtual public void SetCloseSlot()
    {
        this.gameObject.SetActive(false);
    }

    virtual public void ClickDownEvent()    //클릭 다운 함수 - 클릭 시 첫번째로 실행
    {
        ToolTip.Close();
    }

    virtual public void ClickUpEvent()  //클릭 업 함수 - 클릭 시 두번째로 실행
    {
        Check.enabled = true;   //슬롯 체크 표시
        Encyclopedia.instance.ChoiceSlot = this;
    }
    

    public void OnPointerUp(PointerEventData eventData)
    {
        ClickUpEvent();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ClickDownEvent();
    }

}
