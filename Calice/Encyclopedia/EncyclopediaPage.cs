using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���ķ� �з��� ������, ���������� ���İ��� ���� ex) ī��� ����,���.. �������̸� �нú� ��Ƽ��, �������� ���ķ� �з��� �ڽ�(����)���� ������ ����

public class EncyclopediaPage : MonoBehaviour       
{
    public RectTransform RectTransform;
    public GridLayoutGroup GridLayoutGroup;
    public int SumSlotHeight; //���� �ϳ� ���� ũ��

    [Header("Ȯ�� �ɼ�")]
    public List<EncyclopediaSlot> Slots = new List<EncyclopediaSlot>();     //Ÿ�Կ� �з� �� ���Ե�
    public List<int> SlotNums = new List<int>();    //���Ծȿ� ���ִ� �ε�����
    public int LinetoSlotNum;  //�� ���ο� ���� ��� ���� ��
    public int ChildCount;  //Ȱ��ȭ�� �ڽ� �� 

    public void GetChildCount()     //���� �����ִ� ������ üũ (*�ڵ����� ���� ������ �� ���)
    {
        ChildCount = 0;

        for (int i =0; i < this.transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                ChildCount++;
            }
        }
    }

    public void SetPageHeight()     //������ ���� ����
    {
        RectTransform = this.GetComponent<RectTransform>();
        GridLayoutGroup = this.GetComponent<GridLayoutGroup>();

        int n;
        GetChildCount();    //Ȱ��ȭ�� �ڽ� �˻�

        if ((ChildCount % LinetoSlotNum) != 0)
        {
            n = (ChildCount / LinetoSlotNum) + 1;
        }
        else
        {
            n = ChildCount / LinetoSlotNum;
        }

        SumSlotHeight = (int)(GridLayoutGroup.cellSize.y + GridLayoutGroup.spacing.y);

        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, (n * SumSlotHeight));
    }

    /// <summary>
    ///  �������� ���� ���� �ֱ� / ������ �ε���, ���� �ε���
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="num"></param>
    /// <param name="slotnum"></param>
    public void PageSet(EncyclopediaSlot slot, int num, int slotnum)   
    {
        Slots.Add(slot);
        SlotNums.Add(num);
        LinetoSlotNum = slotnum;
    }

    public void SetOpenPage(int tier)   //�з��� ���̴� ���� �Լ�
    {
        SplitSlot(tier);    //�з��� ���̴� �ȿ� ���Ե� ���Ե鵵 �з�
        this.gameObject.SetActive(true);
    }

    public void SetClosePage()  //�з��� ���̴� ���� �Լ�
    {
        this.gameObject.SetActive(false);
    }

    public void SplitSlot(int tier)     //Ƽ�� �з� �Լ�  (��� �ٿ� ������ �� �����)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            Slots[i].SetSplitSlot(tier);        //���Ը��� �з� �� Ȯ�ν��� ����/�� ����
        }

    }
}
