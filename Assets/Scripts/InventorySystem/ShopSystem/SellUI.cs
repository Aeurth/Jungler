using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI currentCountText;
    [SerializeField] private TextMeshProUGUI maxCountText;
    [SerializeField] private Button sellButton;
    [SerializeField] private Image ItemIconImage;
    [SerializeField] private Slider slider;
    private PlayerItemUI ItemUI;

    private int currentCount;
    void OnEnable()
    {
        sellButton.onClick.AddListener(OnSellButtonClicked);
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    void OnDisable()
    {
        sellButton.onClick.RemoveListener(OnSellButtonClicked);
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
    void OnSellButtonClicked()
    {
        if (currentCount <= 0)
        {
            Debug.LogWarning("Cannot sell 0 items.");
            return;
        }
        if (ItemUI.SellItem(currentCount))
        {
            Debug.Log($"SellUI: Sold.");
            // Hide the purchase UI after purchase
            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"SellUI: Failed to sell.");
        }
    }
    void OnSliderValueChanged(float value)
    {
        currentCount = (int)value;
        currentCountText.text = currentCount.ToString();
    }
    public void Show(PlayerItemUI item)
    {
        ItemUI = item;
        Item temp = item.GetItem();
        ItemIconImage.sprite = temp.icon;
        currentCount = 0;
        slider.value = 0;
        slider.maxValue = temp.quantity;
        maxCountText.text = temp.quantity.ToString();
        currentCountText.text = currentCount.ToString();
        Debug.Log($"SellUI: Sell pop-up");
    }


}
