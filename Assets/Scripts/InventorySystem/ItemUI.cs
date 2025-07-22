using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ItemUI :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI quantity;

    protected LootTooltipUI tooltip;
    protected Item item;

    public virtual void InitializeTooltip(LootTooltipUI sharedTooltip)
    {
        tooltip = sharedTooltip;
    }

    public virtual void SetItem(Item newItem)
    {
        item = newItem;
        iconImage.sprite = item.icon;
        this.quantity.text = item.quantity.ToString();
    }

    public Item GetItem(){
        return item;
    }
    public string GetItemID(){
        return item.itemID;
    }
    public void UpdateQuantity(int newQuantity)
    {
        if (item != null)
        {
            item.quantity = newQuantity;
            quantity.text = item.quantity.ToString();
            return;
        }
        Debug.LogWarning("Item is null, cannot update quantity.");

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip == null || item == null) return;

        tooltip.SetItem(item);
        tooltip.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Hide();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Base ItemUI clicked");
    }
    public void Clear()
    {
        iconImage.sprite = null;
        quantity.text = string.Empty;
        tooltip = null;
        item = null;
    }
}
