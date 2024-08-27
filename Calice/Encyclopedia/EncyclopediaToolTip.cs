using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncyclopediaToolTip : MonoBehaviour
{
    public int Num;
    EncyclopediaSlot Slot;

    [Header("���� �ֱ�")]
    public TextTranslate Head;
    public TextTranslate Body;


    virtual public void Display(int num, EncyclopediaSlot slot)     //������ ���� �� ���� �̹��� �� �ؽ�Ʈ ����
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
        if(Slot != null)    //�� ���� üũ ǥ�� ����
        {
            Slot.Check.enabled = false;
        }
        //this.gameObject.SetActive(false);
    }

    virtual public void SetSaw()    //���� Ȱ��ȭ ����
    {

    }
}
