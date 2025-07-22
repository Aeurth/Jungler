using UnityEngine;

public class AbilityExecutor
{
    private CooldownManager cooldownManager = new CooldownManager();

    public bool TryCast(IAbility ability, AbilityContext context)
    {
        float currentTime = Time.time;
        string abilityId = ability.ToString(); // Or use a unique ID property if available
        if (cooldownManager.IsOnCooldown(abilityId, currentTime))
        {
            float readyIn = 0f;
            cooldownManager.TryGetCooldownEnd(abilityId, out float endTime);
            readyIn = endTime - currentTime;
            Debug.Log($"Ability {ability} is on cooldown. Ready in {readyIn:F1}s");
            return false;
        }

        if (ability.Execute(context))
        {
            cooldownManager.SetCooldown(abilityId, currentTime + ability.Cooldown);
            return true;
        }

        return false;
    }
}
