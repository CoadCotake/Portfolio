using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Transform invenTransform;
    public SlotScript tempSlotObject;
    List<SlotScript> slotScripts = new List<SlotScript>();

    private void OnEnable()
    {
        Instance = this;
        tempSlotObject.gameObject.SetActive(false);

        slotScripts.Clear();

        foreach (var item in GetComponentsInChildren<SlotScript>())
        {
            if (item.gameObject != tempSlotObject.gameObject)
            {
                Destroy(item.gameObject);
            }
        }

        foreach (var item in SaveData.GetHaveItems())
        {
            try
            {
                GameObject ins = Instantiate(tempSlotObject.gameObject, invenTransform);
                ins.GetComponent<SlotScript>().SlotItem = item;
                ins.SetActive(true);
            }
            catch (System.Exception)
            {
            }
        }

        slotScripts.AddRange(GetComponentsInChildren<SlotScript>());
    }

    public void Refresh(ItemV2 itemV2)
    {
        foreach (var slot in slotScripts)
        {
            if (slot.SlotItem == itemV2)
            {
                slot.UpdateSlotBackImage();
            }
        }
    }
}
