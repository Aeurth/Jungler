using UnityEngine;
using System.Collections.Generic;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory Instance { get; private set; }

    [SerializeField] private ItemDatabase itemDatabase;

    private Dictionary<string, ItemData> itemLookup = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Build fast lookup table
        foreach (var data in itemDatabase.items)
        {
            if (!string.IsNullOrEmpty(data.id))
                itemLookup[data.id] = data;
        }
    }

    public Item CreateItem(string itemID, int quantity)
    {
        if (itemLookup.TryGetValue(itemID, out ItemData data))
        {
            return CreateItemFromData(data, quantity);
        }

        Debug.LogWarning($"ItemFactory: Item ID '{itemID}' not found.");
        return null;
    }

    public Item CreateItemFromData(ItemData data, int quantity)
    {
        if (data.type == ItemType.Loot)
        {
            return new SellableItem
            {
                itemID = data.id,
                itemName = data.itemName,
                icon = data.icon,
                value = data.value,
                description = data.description,
                stackable = data.stackable,
                maxStackSize = data.maxStackSize,
                quantity = quantity
            };
        }
        else if (data.type == ItemType.Gear)
        {
            var itemStats = new ItemStats
            {
                bonusHealth = data.bonusHealth,
                bonusDamage = data.bonusDamage,
                bonusArmor = data.bonusArmor,
                moveSpeed = data.moveSpeed,
                attackRange = data.attackRange,
                attackCooldownReduction = data.attackCooldownReduction
            };

            return new EquippableItem
            {
                itemID = data.id,
                itemName = data.itemName,
                icon = data.icon,
                value = data.value,
                description = data.description,
                slot = data.slot,
                stats = itemStats,
                maxStackSize = data.maxStackSize,
                quantity = quantity
            };
        }

        return null;
    }

    public ItemData GetItemDataByID(string id)
    {
        itemLookup.TryGetValue(id, out var data);
        return data;
    }
    public int GetItemMaxStackSize(string id)
    {
        if (itemLookup.TryGetValue(id, out var data))
        {
            return data.maxStackSize;
        }
        return 1; // Default max stack size
    }
}
