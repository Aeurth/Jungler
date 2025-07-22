using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PlayerInventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory; // Player inventory
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private LootTooltipUI tooltipUI;
    [SerializeField] private TextMeshProUGUI GoldCount;
     private Inventory targetInventory; // Target inventory for item transfer
     private PlayerItemUI[] itemSlots; // Array of item slots in the UI
    
    private void OnEnable()
    {
        Refresh();
        EventManager<bool>.Subscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }

    private void OnDisable()
    {
        EventManager<bool>.Unsubscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }

    private void OnInventoryUpdated(bool updated)
    {
        Refresh();
    }
    public void PopulateSlots(int count)
    {
        // Clear existing children in contentParent
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Instantiate itemSlotPrefab for the specified count
        for (int i = 0; i < count; i++)
        {
            GameObject slotObject = Instantiate(itemSlotPrefab, contentParent); // Instantiate the prefab
            itemSlots[i] = slotObject.GetComponent<PlayerItemUI>(); // Get the PlayerItemUI component
        }

    Debug.Log($"PlayerInventoryUI: Populated {count} slots in contentParent.");
    }

    public void Refresh()
    {
        if(contentParent.childCount == 0)
        {
            itemSlots = new PlayerItemUI[inventory.GetMaxSize()];
           PopulateSlots(itemSlots.Length);
        }
        List<ItemEntry> items = inventory.Entries;
        for (int i = 0; i < items.Count; i++)
        {
            if(itemSlots[i].GetItem() == null)
            {
                itemSlots[i].SetItem(ItemFactory.Instance.CreateItem(items[i].itemID, items[i].quantity));
            }
            else
            {
                if(itemSlots[i].GetItemID() != items[i].itemID)
                {
                    itemSlots[i].SetItem(newItem: ItemFactory.Instance.CreateItem(items[i].itemID, items[i].quantity));
                }
                itemSlots[i].UpdateQuantity(items[i].quantity);
                
            }
                
             
            itemSlots[i].InitializeTooltip(tooltipUI);
        }
        for (int i = items.Count; i < itemSlots.Length; i++)
        {
            itemSlots[i].Clear();
        }
    }
}
