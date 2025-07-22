using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentCountText;
    [SerializeField] private TextMeshProUGUI maxCountText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image ItemIconImage;
    [SerializeField] private Slider slider;
    private ShopItemUI shopItemUI;

    private int currentCount;

    void OnEnable()
    {
        buyButton.onClick.AddListener(OnBuyPressed);
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    void OnDisable()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        buyButton.onClick.RemoveListener(OnBuyPressed);
    }


    public void OnBuyPressed()
    {

        if (currentCount <= 0)
        {
            Debug.LogWarning("Cannot purchase 0 items.");
            return;
        }
        if (shopItemUI.PurchaseItem(currentCount))
        {
            Debug.Log($"PurchaseUI: Purchased {currentCount} of {shopItemUI.GetItem().itemName}.");
            // Hide the purchase UI after purchase
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"PurchaseUI: Failed to purchase {currentCount} of {shopItemUI.GetItem().itemName}.");
        }
    }
    public void Show(ShopItemUI item)
    {
        shopItemUI = item;
        Item temp = item.GetItem();
        ItemIconImage.sprite = temp.icon;
        currentCount = 0;
        slider.value = 0;
        slider.maxValue = temp.quantity;
        maxCountText.text = temp.quantity.ToString();
        currentCountText.text = currentCount.ToString();
        Debug.Log($"PurchaseUI: Purchase event");
    }
    public void OnSliderValueChanged(float value)
    {
        currentCount = Mathf.FloorToInt(value);
        currentCountText.text = currentCount.ToString();
        Debug.Log($"PurchaseUI: Slider value changed to {currentCount}");
    }
}
