using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TextTranslate;

public class EncyclopediaCardSlot : EncyclopediaSlot
{
    [Header("���� ī�� ����")]
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

    override public void SetDisPlaySlot()       //������ ������� ���� �̹��� �� �ؽ�Ʈ ����
    {
        base.SetDisPlaySlot();        

        //�ؽ�Ʈ        
        Head.textNum = Num;
        Head.textType = TextTranslate.TextType.CardName;

        if (Head.AwakeCheck)
            Head.TranslateText();

        Body.textNum = Num;
        Body.textType = TextTranslate.TextType.CardEx;

        if (Body.AwakeCheck)
            Body.TranslateText();


        //�̹���
        Image.sprite = GameManager.instance.Cardilust[Num];

        //Ÿ��
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

        //�ڽ�Ʈ
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

        //Ƽ��
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

    override public void ClickUpEvent()   //Ŭ�� �Լ�
    {
        base.ClickUpEvent();
        ToolTip.Display(Num,this);
    }

    override public void ClickDownEvent()   //Ŭ�� �Լ�
    {
        base.ClickDownEvent();
        
    }

    override public void SetSplitSlot(int type)       //���� ���� ���� ���� ����/�� ���� ����      -1�� ��ü ��
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

    override public void SetSaw()    //���� ������ ���ο� ���� ���� ??? �Ǵ� ������ ����
    {
        if (EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaCard[Num])    //Ȱ��ȭ �� ���
        {
            //���� ó���ϴ� �� ����
        }
        else
        {
            //�ؽ�Ʈ
            Head.textNum = 76;
            Head.textType = TextTranslate.TextType.UI;
            
            if(Head.AwakeCheck)
                Head.TranslateText();
            
            Body.textNum = 73;
            Body.textType = TextTranslate.TextType.UI;
            
            if (Body.AwakeCheck)
                Body.TranslateText();

            //�̹���
            Image.sprite = GameManager.instance.Cardilust[GameManager.instance.Cardilust.Length - 1];
        }
    }
}
