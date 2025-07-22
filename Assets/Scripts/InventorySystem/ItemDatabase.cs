using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;
}

[System.Serializable]
public class ItemData
{
    public string id; // unique key
    public string itemName;
    public Sprite icon;
    public int value;
    public string description;
    public ItemType type; // "Loot" or "Gear"

    public EquipmentSlot slot;
    public int bonusHealth;
    public int bonusDamage;
    public int bonusArmor;
    public float moveSpeed;
    public float attackRange;
    public float attackCooldownReduction;

    public bool stackable; // stupid, should use maxStackSize instead
    public int maxStackSize;
}
