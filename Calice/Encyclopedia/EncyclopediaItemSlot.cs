using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaItemSlot : EncyclopediaSlot
{
    [Header("���� ������ ����")]
    public Image image;
    public EncyclopediaItemType type;

    EncyclopediaItemToolTip tooltip;

    override public void Setting(int num,int slotnum,EncyclopediaToolTip tooltip, string plus = "")   //plus ���� = �߰����� (ex - ������ó�� �Ѱ��� ������ �ʰ� ó������ �����Ͱ� �������� ���ҵǾ� �ִ� ��� ���� ������ ó���ϴ�)
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
    override public void SetDisPlaySlot()       //������ ������� ���� �̹��� �� �ؽ�Ʈ ����
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

    override public void ClickUpEvent()   //Ŭ�� �Լ�
    {
        base.ClickUpEvent();
        tooltip.type = type;
        ToolTip.Display(Num,this); 
    }

    override public void ClickDownEvent()   //Ŭ�� �Լ�
    {
        base.ClickDownEvent();
        
    }

    override public void SetSplitSlot(int type)       //���� ���� ���� ���� ����/�� ���� ����      -1�� ��ü ��
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

    override public void SetSaw()    //���� ������ ���ο� ���� ���� ??? �Ǵ� ������ ����
    {
        switch (type)
        {
            case EncyclopediaItemType.Passive:
                if(EncyclopediaManager.instance.SawDatas_Encylopedia.SawEncyclopediaPassiveItem[Num])    //Ȱ��ȭ �� ���
                {
                    image.color = new Color(1, 1, 1);
                }
                else
                {
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
                    image.color = new Color(0, 0, 0);
                }
                break;
        }
    }
}
