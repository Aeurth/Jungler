using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentItemUI : ItemUI
{
    public EquipmentSlot equipmentSlot; // The slot this item occupies
    void Awake()
    {
        EventManager<EquippableItem>.Subscribe(EventKeys.EquipPressed, OnEquipPressed); // Subscribe to the item equipped event
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                UnequipItem(this.item as EquippableItem); // Unequip the item if it is already equipped
            }
        }
    }
    void OnEquipPressed(EquippableItem item)
    {
        if (item.slot == equipmentSlot)
        {
            if (this.item == null) // Check if the slot is empty
            {
                EquipItem(item);
                return; 
            }
                
            if(UnequipItem(this.item as EquippableItem))// Unequip the current item in the slot
            {
                EquipItem(item); // Equip the new item in the slot
                return;
            } 
        }
    }
    private void EquipItem(EquippableItem item)
    {
       if(InventoryManager.Instance.TransferItemToTarget(item.itemID, item.quantity, item.stackable))// Equip the item in the inventory
       {
            SetItem(item); // Set the item in the slot UI
            //add tool tip here
            EventManager<ItemStats>.Trigger(EventKeys.AddPlayerStats, item.stats); // Update player stats when equipping an item
            Debug.Log($"EquipmentItemUI: Equipped {item.itemName} in slot {equipmentSlot}.");
            EventManager<bool>.Trigger(EventKeys.InventoryUpdated, true); // Trigger inventory updated event
            return;
        } 

        Debug.LogWarning($"PlayerEquipmentUI: Failed to equip {item.itemName} in slot {equipmentSlot}.");
        
    }
    private bool UnequipItem(EquippableItem item)
    {
        if(InventoryManager.Instance.TransferItemToPlayer(item.itemID, 1, false)) // Unequip the item in the inventory
        {
            EventManager<ItemStats>.Trigger(EventKeys.RemovePlayerSats, item.stats);
            Clear(); // Clear the slot UI
            EventManager<bool>.Trigger(EventKeys.InventoryUpdated, true); // Trigger inventory updated event
            Debug.Log($"EquipmentItemUI: Unequipped {item.itemName} from slot {equipmentSlot.ToString()}.");    
            return true; // Return true if the item was unequipped successfully
        }
        return false; // Return false if the item was not unequipped successfully
    }
}