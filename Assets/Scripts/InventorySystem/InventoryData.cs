using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/Inventory Database")]
public class InventoryData : ScriptableObject
{
    public List<ItemEntry> items = new();
}
[System.Serializable]
public class ItemEntry
{
    public string itemID; // Unique identifier for the item;
    public int quantity; // Quantity of the item in the inventory;
    public int maxStackSize; // Default max stack size

    public ItemEntry(string itemID, int quantity, int maxStackSize = 99)
    {
        this.itemID = itemID;
        this.quantity = quantity;
        this.maxStackSize = maxStackSize;
    }
    /// <summary>
    /// Adds the specified amount to the quantity of this item entry.
    /// If the quantity exceeds the max stack size, it returns the overflow amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>overflow amount</returns>
    public int AddQuantity(int amount)
    {
        quantity += amount;
        if (quantity > maxStackSize)
        {
            int overflow = quantity - maxStackSize;
            quantity = maxStackSize;
            return overflow;
        }
        return 0;
    }
    /// <summary>
    /// Removes the specified amount from the quantity of this item entry.
    /// If the quantity goes below zero, it returns the underflow amount.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns> the remaining ABSOLUTE value that wasn't removed </returns>
    public int RemoveQuantity(int amount)
    {
        int underflow = quantity - amount;
        if (underflow < 0)
        {
            quantity = 0; // Set to zero if underflow occurs
            return Mathf.Abs(underflow);
        }
        quantity = underflow;
        return 0;
    }
}