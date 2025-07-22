using Unity.VisualScripting;
using UnityEngine;

public abstract class AbilityBase : IAbility
{
    public virtual Sprite Icon { get; protected set; }
    public virtual float Cooldown { get; protected set; }
    public virtual float CastTime { get; protected set; }
    public abstract bool Execute(AbilityContext context);
    public abstract TargetingType TargetingType { get; }
    public abstract AbilityUIData GetUIData();
}
