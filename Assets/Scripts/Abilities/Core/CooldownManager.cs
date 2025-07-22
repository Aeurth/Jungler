using System.Collections.Generic;

public class CooldownManager
{
    // Maps ability to cooldown end time per player
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();

    public bool IsOnCooldown(string abilityId, float currentTime)
    {
        if (cooldowns.TryGetValue(abilityId, out float endTime))
        {
            return currentTime < endTime;
        }
        return false;
    }

    public void SetCooldown(string abilityId, float endTime)
    {
        cooldowns[abilityId] = endTime;
    }

    public bool TryGetCooldownEnd(string abilityId, out float endTime)
    {
        return cooldowns.TryGetValue(abilityId, out endTime);
    }
}
