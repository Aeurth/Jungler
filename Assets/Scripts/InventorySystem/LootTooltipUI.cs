
using TMPro;
using UnityEngine;


public class LootTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI valueText;
    private bool UpdatePosition = false;
    private void Update()
    {
        if (!UpdatePosition)
        {
            return;
        }
        gameObject.transform.position = Input.mousePosition + new Vector3(10,0,0);
    }
    public void SetItem(Item newItem)
    {
        nameText.text = newItem.itemName;
        descriptionText.text = newItem.description;
        valueText.text = $"{newItem.value} gold";
    }
    public void Show()
    {
        UpdatePosition = true;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        UpdatePosition = false;
        gameObject.SetActive(false);
    }
}
