using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemUI : ItemUI
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        EventManager<ShopItemUI>.Trigger(EventKeys.PurchaseItemPopUp, this);
        Debug.Log($"ShopItemUI: {item.itemName} clicked for purchase.");
    }

    public bool PurchaseItem(int amount)
    {
        if (item == null )
        {
            Debug.LogError("ShopItemUI: Missing item");
            return false;
        }

        // Calculate the total cost of the purchase
        int totalCost = item.value * amount;

        // Check if the player has enough gold to make the purchase
        if (!GameManager.Instance.GetPlayerInventory().HasItem("1", totalCost))
        {
            Debug.LogWarning($"ShopItemUI: Not enough gold to purchase {amount} of {item.itemName}. Required: {totalCost}");
            return false;
        }

        // Attempt to transfer the item from the shop to the player's inventory
        bool success = InventoryManager.Instance.TransferItemToPlayer(item.itemID, amount, item.stackable);

        if (success)
        {
            // Deduct the gold from the player's inventory
            InventoryManager.Instance.ConsumeItem("1", totalCost);
            EventManager<bool>.Trigger(EventKeys.InventoryUpdated, true);
            Debug.Log($"ShopItemUI: Successfully purchased {amount} of {item.itemName} for {totalCost} gold.");
            
            // Update the UI to reflect the new quantity
            quantity.text = item.quantity.ToString();

            // If the item quantity is 0, optionally hide or disable the UI element
            if (item.quantity <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning($"ShopItemUI: Failed to transfer between inventories {amount} of {item.itemName}.");
        }
        return success;
    }

}