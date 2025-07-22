using UnityEngine;

public class ArrowShot : AbilityBase
{
    public string prefabPath = "Projectiles/Arrow"; // refactor this to use factory pattern later

    // will be refactered to use when used as a scriptable object
    public ArrowShot()
    {
        Cooldown = 1f; // Set the cooldown in seconds (example value)
    }

    public override bool Execute(AbilityContext context)
    {
        if (context.target == null)
            return false;

        if (!string.IsNullOrEmpty(prefabPath))
        {
            var prefab = Resources.Load<GameObject>(prefabPath);
            GameObject arrow = GameObject.Instantiate(prefab, context.caster.transform.position, Quaternion.LookRotation(context.direction));
            var proj = arrow.GetComponent<TabTargetProjectile>();
            proj.Initialize(context.target.transform, 10, 10f); // Example values
            return true;
        }
        return false;
    }

    public override string ToString() => "ArrowShot";

    public override AbilityUIData GetUIData()
    {
        return new AbilityUIData(Icon, Cooldown, this.ToString());
    }

    public override TargetingType TargetingType => TargetingType.TabTarget;
}
