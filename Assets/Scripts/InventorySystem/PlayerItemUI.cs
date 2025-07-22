using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerItemUI : ItemUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        Debug.Log($"Player clicked on {item.itemName}.");
        switch (UIManager.Instance.currentContext)
        {
            case UIContext.Equipment:
                if (item is EquippableItem)
                {
                    EventManager<EquippableItem>.Trigger(EventKeys.EquipPressed, item as EquippableItem);

                }
                else
                {
                    Debug.LogWarning("Item is not an equipment item.");
                }
                break;

            case UIContext.Shop:
                EventManager<PlayerItemUI>.Trigger(EventKeys.SellItemPopUp, this);
                break;
            default:
                Debug.LogWarning("Invalid context for item transfer.");
                break;
        }
    }
    public bool SellItem(int quantity)
    {
        int value = item.value * quantity;
        if (InventoryManager.Instance.TransferItemToTarget(item.itemID, quantity, item.stackable))
        {
            EventManager<bool>.Trigger(EventKeys.InventoryUpdated, true);
            InventoryManager.Instance.AddItem("1", value); // Assuming "1" is the ID for gold
            Debug.Log($"PlayerItemUI: Sold.");
            return true;
        }
        else
        {
            Debug.LogWarning($"PlayerItemUI: Failed to sell {quantity} of {item.itemName}.");
        }
        return false;
    }

}