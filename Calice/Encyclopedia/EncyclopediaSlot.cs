using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EncyclopediaSlot : MonoBehaviour,IPointerUpHandler, IPointerDownHandler
{
    [Header("���� �⺻ ����")]
    public int Num;
    public int SlotNum;
    public bool ToolTipOn=false;
    public EncyclopediaToolTip ToolTip;

    [Header("���� �⺻ ���� ���� �־�� �� ��")]
    public Image Check;
    public TextTranslate Head;
    public TextTranslate Body;

    virtual public void Setting(int num, int slotnum, EncyclopediaToolTip tooltip, string plus ="")   //plus �߰�����
    {
        Num = num;
        SlotNum = slotnum;
        ToolTip = tooltip;
        Check.enabled=false;
    }

    virtual public void SetDisPlaySlot()        //���Կ� �����Ϳ� �°� �̹����� �ؽ�Ʈ ����
    {
        if (EncyclopediaManager.instance.EndLessOn)
        {
            if (EndlessManager.instance.CheckEndlessItem(Num, true))        //���帮������ ������ �ʴ� ������
            {                
                this.gameObject.SetActive(false);   //���� ��Ȱ��ȭ
            }
        }
    }

    virtual public void SetSplitSlot(int type)       //���� ���� ���� ����/�� ���� üũ
    {

    }
    virtual public void SetSaw()    //���� ������ ���ο� ���� ���� ??? or ������ ����
    {
        
    }

    virtual public void SetOpenSlot()
    {
        if (EncyclopediaManager.instance.EndLessOn)
        {
            if (EndlessManager.instance.CheckEndlessItem(Num, true))        //���帮������ ������ �ʴ� ������
            {
                this.gameObject.SetActive(false);   //��Ȱ��ȭ
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

    virtual public void ClickDownEvent()    //Ŭ�� �ٿ� �Լ� - Ŭ�� �� ù��°�� ����
    {
        ToolTip.Close();
    }

    virtual public void ClickUpEvent()  //Ŭ�� �� �Լ� - Ŭ�� �� �ι�°�� ����
    {
        Check.enabled = true;   //���� üũ ǥ��
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
