using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData
{
    public string uniqueName;
    public int count;

    
    public Sprite GetMySprite()
    {
        Sprite spr = Resources.Load<Sprite>("ItemSprites/" + uniqueName);
        // 나중에 넣을 코드
        return spr;
    }
}

public class Inventory : MonoBehaviour
{
    static public Inventory main;
    bool ExistItme;
    GameObject Slot;

    [Header("아이템 리스트")]
    [SerializeField] List<ItemData> Inven_ItemList = new List<ItemData>();
    [SerializeField] List<ItemData> Chest_ItemList = new List<ItemData>();

    [Header("추격아이템 첫 생성 위치")]
    [SerializeField] GameObject StartChase;
    
    private void Awake()
    {
        main = this;   //싱글톤 지정
        Slot = Resources.Load<GameObject>("Prefab/Slot");
        StartChase = GameObject.Find("ChaseStartPoint");
    }

    public void CreateInven(Transform invenContent, InvenType type)
    {
        foreach (InvenSlot invenSlot in invenContent.GetComponentsInChildren<InvenSlot>())
        {
            if (invenSlot.transform != Slot.transform)
            {
                Destroy(invenSlot.gameObject);
            }
        }

        switch (type)
        {
            case InvenType.Inventory:
                foreach (ItemData itemData in Inven_ItemList)
                {
                    GameObject obj = Instantiate(Slot, invenContent);
                    obj.GetComponent<InvenSlot>().itemData = itemData;
                    obj.GetComponent<InvenSlot>().Refresh();        //아이템 적용시키기
                    obj.SetActive(true);
                }
                break;
            case InvenType.Chest:
                foreach (ItemData itemData in Chest_ItemList)
                {
                    GameObject obj = Instantiate(Slot, invenContent);
                    obj.GetComponent<InvenSlot>().itemData = itemData;
                    obj.GetComponent<InvenSlot>().Refresh();        //아이템 적용시키기
                    obj.SetActive(true);
                }
                break;
            default:
                break;
        }


        Slot.SetActive(false);
    }

    public void TakeItem(ItemData drop)    //아이템 획득시 작동
    {
        Debug.Log("아이템획득");
        ExistItme = false;

        for (int i = 0; i < Chest_ItemList.Count; i++)
        {
            if (Chest_ItemList[i].uniqueName == drop.uniqueName)
            {
                Chest_ItemList[i].count += drop.count;
                ExistItme = true;
                break;
            }
        }

        if (!ExistItme)
        {
            Chest_ItemList.Add(drop);
        }
    }

    public void CarryChest()
    {

        for (int i = 0; i < Chest_ItemList.Count; i++)
        {
            Debug.Log("실행됨");
            if (Inven_ItemList.Count != 0)
            {
                for (int ii = 0; ii < Inven_ItemList.Count; ii++)
                {
                    if (Inven_ItemList[ii].uniqueName == Chest_ItemList[i].uniqueName)
                    {
                        Inven_ItemList[ii].count+=Chest_ItemList[i].count;
                    }
                    else
                    {
                        Inven_ItemList.Add(Chest_ItemList[i]);
                    }
                }
            }
            else
            {
                Inven_ItemList.Add(Chest_ItemList[i]);
            }
        }
        Chest_ItemList.Clear();
    }

    public void GainChest()
    {
        for(int i=0;i<Chest_ItemList.Count;i++)
        {
            ItemChaseManger.m_ICM.Spawn(Chest_ItemList[i].count, Resources.Load<Sprite>("ItemSprites/" + Chest_ItemList[i].uniqueName),
                StartChase.transform,ItemChaseManger.m_ICM.SetTarget("Inventory"));  //아이템 생성
        }
        CarryChest();
    }

}
    

