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


    override public void Display(int num, EncyclopediaSlot slot)        //������ ������� ���� �̹��� �� �ؽ�Ʈ ����
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
                tierText = " (�Ϲ�)";
                break;
            case 2:
                tierText = " <color=green>(���)</color>";
                break;
            case 3:
                tierText = " <color=blue>(���)</color>";
                break;
        }

        return tierText;
    }

    override public void SetSaw()    //���� Ȱ��ȭ ����
    {
        switch (type)
        {
            case EncyclopediaItemType.Passive:
                if (EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaPassiveItem[Num])    //Ȱ��ȭ �� ���
                {                    
                    image.color = new Color(1, 1, 1);
                }
                else
                {
                    //�ؽ�Ʈ
                    /*Title.text = "???";
                    Explain.text = "���� �߰����� ���� �нú� �������Դϴ�";*/
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
                if (EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaActiveItem[Num])    //Ȱ��ȭ �� ���
                {                    
                    image.color = new Color(1, 1, 1);
                }
                else
                {
                    //�ؽ�Ʈ
                    /*Title.text = "???";
                    Explain.text = "���� �߰����� ���� ��Ƽ�� �������Դϴ�";*/
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
