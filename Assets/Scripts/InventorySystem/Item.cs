using UnityEngine;

public enum ItemType
{
    Loot,
    Gear
}
public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Weapon,
    Offhand
}

[System.Serializable]
public abstract class Item
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public int value; // gold price
    public string description;
    public bool stackable;
    public int maxStackSize;
    public int quantity; // current amount of items in the stack

    public virtual string GetItemType()
    {
        return "Item";
    }
}

[System.Serializable]
public class SellableItem : Item
{
    public override string GetItemType()
    {
        return "Loot";
    }
}

[System.Serializable]
public class ItemStats
{
    public int bonusHealth;
    public int bonusDamage;
    public int bonusArmor;
    public float moveSpeed;
    public float attackRange;
    public float attackCooldownReduction;
}

[System.Serializable]
public class EquippableItem : Item
{
    public EquipmentSlot slot;
    public ItemStats stats; // Use ItemStats to encapsulate item properties

    public override string GetItemType()
    {
        return "Gear";
    }

    public void SetItemStats(ItemStats newStats)
    {
        stats = newStats;
    }

    public ItemStats GetItemStats()
    {
        return stats;
    }
}

