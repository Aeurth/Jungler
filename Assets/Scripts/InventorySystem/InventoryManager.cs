using UnityEngine;


public class InventoryManager: MonoBehaviour
{
    // Singleton pattern to ensure only one instance of InventoryManager exists
    public static InventoryManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Inventory playerInventory;
    private Inventory targetInventory;
    private void Start()
    {
        playerInventory = GameManager.Instance.GetPlayerInventory();
    }
    public void SetTargetInventory(Inventory inventory)
    {
        targetInventory = inventory;
    }
    public void ConsumeItem(string itemID, int amount)
    {
        playerInventory.RemoveItem(itemID, amount);
    }
    public void AddItem(string itemID, int amount)
    {
        playerInventory.AddItem(itemID, amount);
    }
    public bool TransferItemToPlayer(string itemID, int amount, bool stackable)
    {
        return TransferItem(itemID, amount, targetInventory, playerInventory, stackable);
    }
    public bool TransferItemToTarget(string itemID, int amount, bool stackable)
    {
        return TransferItem(itemID, amount, playerInventory, targetInventory, stackable);
    }
    private bool TransferItem(string itemID, int amount, Inventory from, Inventory to, bool stackable)
    {
        bool success = false;
        if (from == null)
        {
            Debug.LogError($"InventoryManager:from inventory is null.");
            return success;
        }
        if (to == null)
        {
            Debug.LogError("InventoryManager: Destination inventory is null.");
            return success;
        }
        if (!from.HasItem(itemID, amount))
        {
            Debug.LogError($"InventoryManager: {from.inventoryName} inventory does not have enough of item {itemID}.");
            return success;
        }

        if (stackable)
        {
            if (to.AddItem(itemID, amount) > 0 & !from.RemoveItem(itemID, amount))
            {
                Debug.Log("InventoryManager: not all items could be transferred.");
                success = false;
            }
            else
            {
                success = true;
                Debug.Log($"InventoryManager: {amount} of itemID:{itemID} transferred from {from.inventoryName} to {to.inventoryName}.");
            }
        }
        else
        {
            if (to.AddEntry(itemID)) //adds seperate item entry for not stackable items
            {
                from.RemoveEntry(itemID); // removes the entry from the source inventory
                Debug.Log($"InventoryManager: {itemID} transferred from {from.inventoryName} to {to.inventoryName}.");
                success = true;
            }
        }

        return success;
    }
}
