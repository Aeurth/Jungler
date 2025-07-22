using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] protected private Slider slider;
    // the monster or player this health bar is tracking

    public void Initialize(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void UpdateHealth(float current)
    {
        float targetRatio = current / slider.maxValue;

        LeanTween.value(slider.gameObject, slider.value, current, 0.25f)
            .setOnUpdate((float val) => slider.value = val)
            .setEaseOutQuad();
    }

    
}
