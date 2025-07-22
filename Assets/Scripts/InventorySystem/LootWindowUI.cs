using System.Collections.Generic;
using UnityEngine;

public class LootWindowUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject lootItemPrefab;
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private LootTooltipUI tooltipUI;

    private Inventory lootInventory;
    private void OnEnable()
    {
        UIManager.Instance.currentContext = UIContext.Loot;
 
        EventManager<bool>.Subscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }
    private void OnDisable()
    {
        UIManager.Instance.currentContext = UIContext.Default;
        EventManager<bool>.Unsubscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }

    public void Show( Inventory lootSourceInventory)
    {
        gameObject.SetActive(true);
        lootInventory = lootSourceInventory;
        InventoryManager.Instance.SetTargetInventory(lootInventory);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
            
        if(lootInventory == null)
        {
            Debug.LogWarning("Loot inventory is null.");
            return;
        }
        if(lootInventory.Entries.Count == 0){
            Hide();
            Debug.LogWarning("Loot inventory is empty.");
            return;
        }
        foreach (ItemEntry Entry in lootInventory.Entries)
        {
            Item item = ItemFactory.Instance.CreateItem(Entry.itemID, Entry.quantity);
            if (item == null)
            {
                Debug.LogError($"Failed to create item with ID: {Entry.itemID}");
                continue;
            }
            GameObject go = Instantiate(lootItemPrefab, contentParent);
            LootItemUI itemUI = go.GetComponent<LootItemUI>();

            itemUI.SetItem(item);

            itemUI.InitializeTooltip(tooltipUI);
        }
    }
    private void OnInventoryUpdated(bool updated)
    {
        Refresh();
    }
}
