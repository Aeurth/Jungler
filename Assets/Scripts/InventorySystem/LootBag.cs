using UnityEngine;
using System.Collections.Generic;

public class LootBag : MonoBehaviour
{
    private Inventory inventory;
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        EventManager<bool>.Subscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }
    private void OnDestroy()
    {
    EventManager<bool>.Unsubscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }
    public void SetLoot(Inventory inv)
    {
        InventoryData runtimeData = inv.Copy();
        inventory.SetInventoryData(runtimeData);
        inventory.SetMaxSize(inventory.Entries.Count);
    }
    public Inventory GetInventory()
    {
        return inventory;
    }
    private void OnMouseDown()
    {
        EventManager<Inventory>.Trigger("LootBagClicked", inventory);
    }
    private void OnInventoryUpdated(bool updated)
    {
        if(inventory.Entries.Count <= 0)
            Destroy(gameObject);
    }
        
}
