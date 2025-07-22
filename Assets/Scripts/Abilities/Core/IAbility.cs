using UnityEngine;
using System;

public interface IAbility
{
    bool Execute(AbilityContext context);
    float Cooldown { get; }
    float CastTime { get; }
    TargetingType TargetingType { get; } // Added property for targeting
    AbilityUIData GetUIData(); // Added method for retrieving UI data
}
