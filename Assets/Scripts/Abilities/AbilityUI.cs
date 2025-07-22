using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Image abilityUIImage;
    [SerializeField] private GameObject timerOverlay;
    [SerializeField] private Image fillImage;
    private string abilityName = string.Empty;

    private float cooldownTime = 0f;
    private float cooldownDuration = 0f;

    // Add methods to update UI as needed
    void OnEnable()
    {
        EventManager<string>.Subscribe(EventKeys.AbilityUsed, OnAbilityUsed);
    }
    void OnDisable()
    {
        EventManager<string>.Unsubscribe(EventKeys.AbilityUsed, OnAbilityUsed);
    }

    public void OnAbilityUsed(string abilityName)
    {
        if (abilityName != this.abilityName)
            return;
        cooldownTime = cooldownDuration;
        timerOverlay.SetActive(true);
        if (fillImage != null)
            fillImage.fillAmount = 1f;
    }

    void Update()
    {
        if (cooldownTime > 0f)
        {
            cooldownTime -= Time.deltaTime;
            fillImage.fillAmount = cooldownTime / cooldownDuration;
            return;
        }

        if (cooldownTime <= 0f)
        {
            cooldownTime = 0f;
            timerOverlay.SetActive(false);
            fillImage.fillAmount = 0f;
        }

    }

    public void SetAbilityUI(AbilityUIData data)
    {
        if (abilityUIImage != null)
            abilityUIImage.sprite = data.icon;
        abilityName = data.abilityName;
        cooldownDuration = data.cooldown;
    }
}
