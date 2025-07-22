using UnityEngine;

public class AbilityUIData
{
    public Sprite icon;
    public float cooldown;
    public string abilityName;
    // Add more fields as needed
    public AbilityUIData(Sprite icon, float cooldown, string abilityName)
    {
        this.icon = icon;
        this.cooldown = cooldown;
        this.abilityName = abilityName;
    }
}
