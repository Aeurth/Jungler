using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public string inventoryName;
    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private int maxSlots;
   
    public List<ItemEntry> Entries => inventoryData.items;
    public InventoryData Copy()
    {
        InventoryData runtimeData = Instantiate(inventoryData);
        List<ItemEntry> runtimeItems = new List<ItemEntry>();
        foreach (var entry in Entries)
        {
            runtimeItems.Add(new ItemEntry(entry.itemID, entry.quantity));
        }
        runtimeData.items = runtimeItems;
        return runtimeData;
    }
    public void SetInventoryData(InventoryData data)
    {
        inventoryData = data;
    }

    public int AddItem(string itemID, int amount = 1)
    {
        return AddItemRecursive(itemID, amount);
    }
    private int AddItemRecursive(string itemID, int remainingAmount)
    {
        if (remainingAmount <= 0 || Entries.Count == maxSlots)
            return remainingAmount;

        // Try to add to existing entries
        foreach (ItemEntry entry in Entries)
        {
            if (entry.itemID == itemID)
            {
                remainingAmount = entry.AddQuantity(remainingAmount);
                if (remainingAmount <= 0)
                    return 0;
            }
        }

        // Add a new entry if space allows
        if (Entries.Count < maxSlots)
        {
            ItemEntry newEntry = new ItemEntry(itemID, 0, ItemFactory.Instance.GetItemMaxStackSize(itemID));
            remainingAmount = newEntry.AddQuantity(remainingAmount);
            Entries.Add(newEntry);
            if (remainingAmount <= 0)
                return 0;
        }

        // Recurse until conditions are met
        return AddItemRecursive(itemID, remainingAmount);
    }
    public bool RemoveItem(string itemID, int amount = 1)
    {
        int amountToRemove = amount;
        List<ItemEntry> itemsToRemove = new List<ItemEntry>();

        foreach (ItemEntry entry in Entries)
        {
            if (amountToRemove <= 0)
                break; // All requested items have been removed

            if (entry.itemID == itemID)
            {
                amountToRemove = entry.RemoveQuantity(amountToRemove);
                Debug.Log($"Inventory: amountToRemove: {amountToRemove}");
                if (entry.quantity <= 0)
                {
                    itemsToRemove.Add(entry); // Mark the entry for removal
                }
            }
        }

        // Remove marked entries after iteration

        foreach (ItemEntry entry in itemsToRemove)
        {
            Entries.Remove(entry);
        }

        if (amountToRemove > 0)
        {
            Debug.Log($"Inventory: Unable to remove {amount} of {itemID}. Only {amount - amountToRemove} removed.");
            return false;
        }

        return true;
    }
    public void SetMaxSize(int count)
    {
        maxSlots = count;
    }
    public int GetMaxSize()
    {
        return maxSlots;
    }
    public bool HasItem(string itemID, int amount = 1)
    {
        int currentAmount = 0;
        foreach (var entry in Entries)
        {
            if (entry.itemID == itemID){
                currentAmount += entry.quantity;
            }
            if (currentAmount >= amount)
            {
                return true; // Found enough of the item
            }
            
        }
        return false;
    }
    public bool AddEntry(string itemID, int amount = 1)
    {
        if (Entries.Count == maxSlots)
        {
            Debug.Log("Inventory: No space for new entry.");
            return false;
        }
        Entries.Add(new ItemEntry(itemID, amount));
        return true;
        
    }
    public bool RemoveEntry(ItemEntry entry)
    {
        if(Entries.Remove(entry))
            return true;
        
        return false;
    }
    public bool RemoveEntry(string itemID)
    {
        foreach (var entry in Entries)
        {
            if (entry.itemID == itemID)
            {
                Entries.Remove(entry);
                return true;
            }
        }
        return false;
    }
}
