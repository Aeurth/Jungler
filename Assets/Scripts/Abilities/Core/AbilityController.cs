using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public IAbility[] abilities; // Assign in Inspector or at runtime
    public AbilityUI[] abilityUIs; // Assign in Inspector or at runtime
    public Sprite[] sprites;
    public Player player; // Reference to Player component
    public AbilityExecutor executor; // Reference to AbilityExecutor

    public TabTargetTargeting tabTargeting = new TabTargetTargeting();
    public SkillShotTargeting skillShotTargeting = new SkillShotTargeting();
    public SkillShotAoETargeting skillShotAoETargeting = new SkillShotAoETargeting();

    void Start()
    {
        IAbility fireball = new Fireball();
        IAbility frostbolt = new FrostBolt();
        IAbility arrowshot = new ArrowShot();

        abilities = new IAbility[] { fireball, frostbolt, arrowshot };
        executor = new AbilityExecutor();
        SetAbilityUIs();
    }

    void Update()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                IAbility ability = abilities[i];
                AbilityContext context = new AbilityContext
                {
                    caster = player.gameObject
                };
                // Targeting logic
                switch (ability.TargetingType)
                {
                    case TargetingType.TabTarget:
                        tabTargeting.AcquireTarget(context);
                        break;
                    case TargetingType.SkillShot:
                        skillShotTargeting.AcquireTarget(context);
                        break;
                    case TargetingType.SkillShot_AoE:
                        skillShotAoETargeting.AcquireTarget(context);
                        break;
                }

                if (executor.TryCast(ability, context))
                {
                    EventManager<string>.Trigger(EventKeys.AbilityUsed, ability.ToString());
                }
            
            }
        }
    }
    private void SetAbilityUIs()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (i < abilityUIs.Length)
            {
                AbilityUIData uiData = abilities[i].GetUIData();
                uiData.icon = System.Array.Find(sprites, s => s.name == uiData.abilityName);
                abilityUIs[i].SetAbilityUI(uiData);
            }
        }
    }
}
