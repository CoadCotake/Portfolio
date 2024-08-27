using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaCardToolTip : EncyclopediaToolTip
{    
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Explain;

    public Image CardType;
    public Image Tier;
    public Image Cost;
    public Image Image;

    override public void Display(int num, EncyclopediaSlot slot)    //데이터 기반으로 툴팁 이미지 및 텍스트 설정
    {
        base.Display(num,slot);

        //텍스트
        /*Title.text = Encyclopedia.instance.comcardDB.comcard[num].name;
        Explain.text = Encyclopedia.instance.comcardDB.comcard[num].explain;*/

        Head.textNum = Num;
        Head.textType = TextTranslate.TextType.CardName;

        if (Head.AwakeCheck)
            Head.TranslateText();

        Body.textNum = Num;
        Body.textType = TextTranslate.TextType.CardEx;

        if (Body.AwakeCheck)
            Body.TranslateText();

        //이미지
        Image.sprite = GameManager.instance.Cardilust[num];

        //타입
        switch (Collection.instance.GetCardDB(EncyclopediaManager.instance.EndLessOn).comcard[num].Type)
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
        switch (Collection.instance.GetCardDB(EncyclopediaManager.instance.EndLessOn).comcard[num].SlotCount)
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
        switch (Collection.instance.ComcardDB.comcard[num].Tier)
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

    override public void Close()
    {
        base.Close();
    }

    override public void SetSaw()    //도감 활성화 세팅
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

            if (Head.AwakeCheck)
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
