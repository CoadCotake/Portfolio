using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaToolTip : MonoBehaviour
{
    public int Num;
    EncyclopediaSlot Slot;

    [Header("직접 넣기")]
    public TextTranslate Head;
    public TextTranslate Body;


    virtual public void Display(int num, EncyclopediaSlot slot)     //데이터 설정 및 툴팁 이미지 및 텍스트 설정
    {
        if(slot != null)
        {

        }

        Num = num;
        Slot = slot;
        this.gameObject.SetActive(true);
    }

    virtual public void Close()
    {
        if(Slot != null)    //전 슬롯 체크 표시 해제
        {
            Slot.Check.enabled = false;
        }
        //this.gameObject.SetActive(false);
    }

    virtual public void SetSaw()    //도감 활성화 세팅
    {

    }
}
