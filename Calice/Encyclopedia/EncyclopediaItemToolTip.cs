using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaItemToolTip : EncyclopediaToolTip
{
    public Image image;
    public Text Title;
    public Text Explain;

    public EncyclopediaItemType type;


    override public void Display(int num, EncyclopediaSlot slot)        //데이터 기반으로 툴팁 이미지 및 텍스트 설정
    {
        base.Display(num,slot);

        switch (type)
        {
            case EncyclopediaItemType.Passive:
                image.sprite = GameManager.instance.ItemSpriteList[num];
                /*Title.text = Encyclopedia.instance.itemDB.Entities[num].name + TierTextSet(Encyclopedia.instance.itemDB.Entities[num].tier);
                Explain.text = Encyclopedia.instance.itemDB.Entities[num].explain;*/
                Head.textNum = Num;
                Head.textType = TextTranslate.TextType.PasName;

                if (Head.AwakeCheck)
                    Head.TranslateText();

                Body.textNum = Num;
                Body.textType = TextTranslate.TextType.PasEx;

                if (Body.AwakeCheck)
                    Body.TranslateText();
                break;
            case EncyclopediaItemType.Active:
                image.sprite = GameManager.instance.ItemSpriteList_act[num];
                /*Title.text = Encyclopedia.instance.itemDB.Active[num].name + TierTextSet(Encyclopedia.instance.itemDB.Active[num].tier);
                Explain.text = Encyclopedia.instance.itemDB.Active[num].explain;*/
                Head.textNum = Num;
                Head.textType = TextTranslate.TextType.ActName;

                if (Head.AwakeCheck)
                    Head.TranslateText();

                Body.textNum = Num;
                Body.textType = TextTranslate.TextType.ActEx;

                if (Body.AwakeCheck)
                    Body.TranslateText();
                break;
        }

        SetSaw();
    }

    override public void Close()
    {
        base.Close();
    }

    public string TierTextSet(int tier)
    {
        string tierText = "";
        switch (tier)
        {
            case 1:
                tierText = " (일반)";
                break;
            case 2:
                tierText = " <color=green>(고급)</color>";
                break;
            case 3:
                tierText = " <color=blue>(희귀)</color>";
                break;
        }

        return tierText;
    }

    override public void SetSaw()    //도감 활성화 세팅
    {
        switch (type)
        {
            case EncyclopediaItemType.Passive:
                if (EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaPassiveItem[Num])    //활성화 일 경우
                {                    
                    image.color = new Color(1, 1, 1);
                }
                else
                {
                    //텍스트
                    /*Title.text = "???";
                    Explain.text = "아직 발견하지 않은 패시브 아이템입니다";*/
                    Head.textNum = 76;
                    Head.textType = TextTranslate.TextType.UI;

                    if (Head.AwakeCheck)
                        Head.TranslateText();

                    Body.textNum = 74;
                    Body.textType = TextTranslate.TextType.UI;

                    if (Body.AwakeCheck)
                        Body.TranslateText();

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
                    //텍스트
                    /*Title.text = "???";
                    Explain.text = "아직 발견하지 않은 액티브 아이템입니다";*/
                    Head.textNum = 76;
                    Head.textType = TextTranslate.TextType.UI;

                    if (Head.AwakeCheck)
                        Head.TranslateText();

                    Body.textNum = 75;
                    Body.textType = TextTranslate.TextType.UI;

                    if (Body.AwakeCheck)
                        Body.TranslateText();
                    image.color = new Color(0, 0, 0);
                }
                break;
        }

    }
}
