using UnityEngine;
using UnityEngine.EventSystems;

public class LootItemUI : ItemUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (InventoryManager.Instance.TransferItemToPlayer(item.itemID, item.quantity, item.stackable))
        {
            Debug.Log($"{item.itemName} transferred from loot.");
            EventManager<bool>.Trigger(EventKeys.InventoryUpdated, true);
            OnPointerExit(eventData);
        }
        else
        {
            Debug.Log($"LootItemUI: {item.itemName} could not be transferred.");
        }
    }
}