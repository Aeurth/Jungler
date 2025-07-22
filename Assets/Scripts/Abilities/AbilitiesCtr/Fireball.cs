using UnityEngine;

public class Fireball : AbilityBase
{
    public string prefabPath = "Projectiles/Fireball";
    public string aoePrefabPath = "Projectiles/FireballAoE";
    public float aoeDuration = 3f;
    public Fireball()
    {
        Cooldown = 2f; // Example cooldown
    }
    public override bool Execute(AbilityContext context)
    {
        if (!string.IsNullOrEmpty(prefabPath))
        {
            var prefab = Resources.Load<GameObject>(prefabPath);
            GameObject fireball = GameObject.Instantiate(prefab, context.caster.transform.position, Quaternion.LookRotation(context.direction));
            var proj = fireball.GetComponent<SkillShotAOEProjectile>();
            GameObject aoePrefab = Resources.Load<GameObject>(aoePrefabPath);
            proj.Initialize(context.position, 10, aoePrefab, aoeDuration); // Example values
            return true;
        }
        return false;
    }

    public override string ToString() => "Fireball";

    public override TargetingType TargetingType => TargetingType.SkillShot_AoE;
    public override AbilityUIData GetUIData()
    {
        return new AbilityUIData(Icon, Cooldown, this.ToString());
    }
}
