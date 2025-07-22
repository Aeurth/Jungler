using UnityEngine;

public class FrostBolt : AbilityBase
{
    public string prefabPath = "Projectiles/FrostBolt";
    public FrostBolt()
    {
        Cooldown = 1f; // Example cooldown
        CastTime = 0.5f; // Example cast time
    }
    public override bool Execute(AbilityContext context)
    {
        if (!string.IsNullOrEmpty(prefabPath))
        {

            var prefab = Resources.Load<GameObject>(prefabPath);
            GameObject fireball = GameObject.Instantiate(prefab, context.caster.transform.position, Quaternion.LookRotation(context.direction));
            var proj = fireball.GetComponent<SkillShotProjectile>();
            proj.Initialize(context.position, 10, 10f); // Example values
            return true;
        }

        return false;
    }

    public override string ToString() => "FrostBolt";

    public override TargetingType TargetingType => TargetingType.SkillShot;
    public override AbilityUIData GetUIData()
    {
        return new AbilityUIData(null, Cooldown, this.ToString());
    }
}
