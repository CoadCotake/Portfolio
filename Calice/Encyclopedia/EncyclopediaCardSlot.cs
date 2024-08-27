using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TextTranslate;

public class EncyclopediaCardSlot : EncyclopediaSlot
{
    [Header("슬롯 카드 변수")]
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Explain;

    public Image CardType;
    public Image Tier;
    public Image Cost;
    public Image Image;

    private void Start()
    {
        if (Head.AwakeCheck)
            Head.TranslateText();

        if (Body.AwakeCheck)
            Body.TranslateText();
    }

    override public void SetDisPlaySlot()       //데이터 기반으로 슬롯 이미지 및 텍스트 설정
    {
        base.SetDisPlaySlot();        

        //텍스트        
        Head.textNum = Num;
        Head.textType = TextTranslate.TextType.CardName;

        if (Head.AwakeCheck)
            Head.TranslateText();

        Body.textNum = Num;
        Body.textType = TextTranslate.TextType.CardEx;

        if (Body.AwakeCheck)
            Body.TranslateText();


        //이미지
        Image.sprite = GameManager.instance.Cardilust[Num];

        //타입
        switch (Collection.instance.GetCardDB(EncyclopediaManager.instance.EndLessOn).comcard[Num].Type)
        {
            case "Attack":
                CardType.sprite = Collection.instance.CardTier[0];
                break;
            case "Deffend":
                CardType.sprite = Collection.instance.CardTier[1];
                break;
            case "Heal":
                CardType.sprite = Collection.instance.CardTier[2];
                break;
            case "Buff":
                CardType.sprite = Collection.instance.CardTier[3];
                break;
            case "DeBuff":
                CardType.sprite = Collection.instance.CardTier[4];
                break;
        }

        //코스트
        switch (Collection.instance.GetCardDB(EncyclopediaManager.instance.EndLessOn).comcard[Num].SlotCount)
        {
            case 1:
                Cost.sprite = GameManager.instance.Cost[0];
                break;
            case 2:
                Cost.sprite = GameManager.instance.Cost[1];
                break;
            case 3:
                Cost.sprite = GameManager.instance.Cost[2];
                break;
        }

        //티어
        switch (Collection.instance.GetCardDB(EncyclopediaManager.instance.EndLessOn).comcard[Num].Tier)
        {
            case 1:
                Tier.sprite = GameManager.instance.Tier[0];
                break;
            case 2:
                Tier.sprite = GameManager.instance.Tier[1];
                break;
            case 3:
                Tier.sprite = GameManager.instance.Tier[2];
                break;
        }

        SetSaw();
    }

    override public void ClickUpEvent()   //클릭 함수
    {
        base.ClickUpEvent();
        ToolTip.Display(Num,this);
    }

    override public void ClickDownEvent()   //클릭 함수
    {
        base.ClickDownEvent();
        
    }

    override public void SetSplitSlot(int type)       //정렬 값에 따라 슬롯 켜짐/끔 여부 결정      -1은 전체 값
    {
        if(type == -1 || Collection.instance.GetCardDB(EncyclopediaManager.instance.EndLessOn).comcard[Num].Tier == (type+1))
        {
            SetOpenSlot();
        }
        else
        {
            SetCloseSlot();
        }
    }

    override public void SetSaw()    //도감 데이터 여부에 따라 슬롯 ??? 또는 보여줌 세팅
    {
        if (EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaCard[Num])    //활성화 일 경우
        {
            //따로 처리하는 것 없음
        }
        else
        {
            //텍스트
            Head.textNum = 76;
            Head.textType = TextTranslate.TextType.UI;
            
            if(Head.AwakeCheck)
                Head.TranslateText();
            
            Body.textNum = 73;
            Body.textType = TextTranslate.TextType.UI;
            
            if (Body.AwakeCheck)
                Body.TranslateText();

            //이미지
            Image.sprite = GameManager.instance.Cardilust[GameManager.instance.Cardilust.Length - 1];
        }
    }
}
