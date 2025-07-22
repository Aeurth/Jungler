using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class PlayerEquipmentUI : MonoBehaviour
{   

    
    [Header("Player Stats")]
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI armorText;
    [SerializeField] private TextMeshProUGUI movementSpeedText;
    [SerializeField] private PlayerStats playerStats; // Reference to the player's stats

    [Header("Item Slots")]
    [SerializeField] private GameObject ContentParent;
    [SerializeField] private Inventory equipmentInventory; // Player equipment inventory
    private EquipmentItemUI[] itemSlots; // Array of item slots in the UI
    void Awake()
    {
        itemSlots = ContentParent.GetComponentsInChildren<EquipmentItemUI>(); // Get all item slots in the content parent
    }
    private void OnEnable()
    {
        Refresh(); // Refresh the UI when enabled
        UIManager.Instance.currentContext = UIContext.Equipment; // Set the current context to Equipment
        InventoryManager.Instance.SetTargetInventory(equipmentInventory); // Set the target inventory to the equipment inventory
        OnPlayerStatsUpdated(playerStats); // Update the player stats UI when the equipment UI is enabled
        EventManager<PlayerStats>.Subscribe(EventKeys.PlayerStatsUpdated, OnPlayerStatsUpdated); // Subscribe to the player stats updated event    
    }

    private void OnDisable()
    {
        UIManager.Instance.currentContext = UIContext.Default; // Reset the context to Default
        EventManager<PlayerStats>.Unsubscribe(EventKeys.PlayerStatsUpdated, OnPlayerStatsUpdated); // Unsubscribe from the player stats updated event
    }
    // this is stupid, I have the referenc to player stats but still use events
    // I should just call the method directly
    // but I want to use events to check if the player stats are updated
    // should just use unity basic events
    private void OnPlayerStatsUpdated(PlayerStats stats)
    {
        attackText.text = $"Attack: {stats.attackDamage}";
        attackSpeedText.text = $"Attack cooldown: {stats.attackCooldown}";
        attackRangeText.text = $"Attack Range: {stats.attackRange}";
        healthText.text = $"Health: {stats.health}";
        armorText.text = $"Armor: {stats.armor}";
        movementSpeedText.text = $"Speed: {stats.moveSpeed}";
    }
    public void Refresh()
    {
        List<ItemEntry> items = equipmentInventory.Entries;

        for (int i = 0; i < items.Count; i++)
        {
            EquipmentSlot currentSlot = ItemFactory.Instance.GetItemDataByID(items[i].itemID).slot;

            for (int j = 0; j < itemSlots.Length; j++)
            {
                if (itemSlots[j].equipmentSlot == currentSlot)
                {
                    itemSlots[j].SetItem(
                    newItem: ItemFactory.Instance.CreateItem(items[i].itemID, items[i].quantity)
                    );
                    break;
                }
            }
            //itemSlots[i].InitializeTooltip(tooltipUI);
        }
        for (int i = items.Count; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].GetItem() != null)
            {
                itemSlots[i].Clear(); // Clear the item slot if there are no items
            }
        }
    }
}