using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    List<SlotScript> slotScripts = new List<SlotScript>();

    private void OnEnable()
    {
        Instance = this;
        Refresh();
    }

    public void Refresh()
    {
        slotScripts.Clear();
        slotScripts.AddRange(GetComponentsInChildren<SlotScript>());

        foreach (var itemV2 in SaveData.UseItems)
        {
            // Find an empty slot of the required category
            var targetSlot = slotScripts.Find(slot => slot.SlotItem == null && slot.slotType == itemV2.ItemData1.SlotCategory);

            if (targetSlot != null)
            {
                targetSlot.SlotItem = itemV2;
                DataManager.Instance.SetItemUse(itemV2);
            }
        }
    }
    public void EquipItem(ItemV2 itemV2)
    {
        // Check if there's a slot that already has an item of the same category equipped
        var equippedSlot = slotScripts.Find(slot => slot.SlotItem != null && slot.slotType == itemV2.ItemData1.SlotCategory);

        if (equippedSlot != null)
        {
            ItemV2 targetItem = equippedSlot.SlotItem;
            // If an item of the same category is already equipped, unequip it
            UnequipItem(equippedSlot.SlotItem);

            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.Refresh(targetItem);
            }
        }

        // Find an empty slot of the required category
        var targetSlot = slotScripts.Find(slot => slot.SlotItem == null && slot.slotType == itemV2.ItemData1.SlotCategory);

        if (targetSlot != null)
        {
            targetSlot.SlotItem = itemV2;
            DataManager.Instance.SetItemUse(itemV2);
        }
    }

    public void UnequipItem(ItemV2 itemV2)
    {
        var targetSlot = slotScripts.Find(slot => slot.SlotItem != null && slot.SlotItem.Hash == itemV2.Hash);
        
        if (targetSlot != null)
        {
            targetSlot.SlotItem = null;
            DataManager.Instance.DeleteItemUse(itemV2);
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.Refresh(itemV2);
        }
    }
}
