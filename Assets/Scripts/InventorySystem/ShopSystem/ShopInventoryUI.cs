using UnityEngine;
using System.Collections.Generic;

public class ShopInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject itemUIPrefab;
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private LootTooltipUI tooltipUI;
    [SerializeField] private GameObject purchaseUI;
    [SerializeField] private GameObject sellUI;

    private Inventory shopInventory;
    public void OnEnable()
    {
        UIManager.Instance.currentContext = UIContext.Shop;
        EventManager<ShopItemUI>.Subscribe(EventKeys.PurchaseItemPopUp, OnPurchaseItemPopUp);
        EventManager<PlayerItemUI>.Subscribe(EventKeys.SellItemPopUp, OnSellItemPopUp);
        EventManager<bool>.Subscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }
    void OnDisable()
    {
        UIManager.Instance.currentContext = UIContext.Default;
        EventManager<ShopItemUI>.Unsubscribe(EventKeys.PurchaseItemPopUp, OnPurchaseItemPopUp);
        EventManager<PlayerItemUI>.Unsubscribe(EventKeys.SellItemPopUp, OnSellItemPopUp);
        EventManager<bool>.Unsubscribe(EventKeys.InventoryUpdated, OnInventoryUpdated);
    }

    public void Show(Inventory shopInventory)
    {
        gameObject.SetActive(true);
        this.shopInventory = shopInventory;
        InventoryManager.Instance.SetTargetInventory(shopInventory);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ClearShop();
    }

    private void ClearShop()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }
    private void Refresh()
    {
        ClearShop();

        foreach (ItemEntry Entry in shopInventory.Entries)
        {
            Item item = ItemFactory.Instance.CreateItem(Entry.itemID, Entry.quantity);
            GameObject go = Instantiate(itemUIPrefab, contentParent);
            ShopItemUI itemUI = go.GetComponent<ShopItemUI>();
            itemUI.SetItem(item);
            itemUI.InitializeTooltip(tooltipUI);
        }
    }
    private void OnPurchaseItemPopUp(ShopItemUI item)
    {
        tooltipObject.SetActive(false);
        purchaseUI.SetActive(true);
        PurchaseUI purchase = purchaseUI.GetComponent<PurchaseUI>();
        purchase.Show(item);
    }
    private void OnSellItemPopUp(PlayerItemUI item)
    {
        sellUI.SetActive(true);
        SellUI sell = sellUI.GetComponent<SellUI>();
        sell.Show(item);

    }
    private void OnInventoryUpdated(bool updated)
    {
        if(updated){
            Refresh();
        }
    }
}
