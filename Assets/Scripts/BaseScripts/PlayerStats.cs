using UnityEngine;

[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    public float moveSpeed;
    public float attackRange;
    public int attackDamage;
    public float attackCooldown;
    public int health;
    public int maxHealth;
    public int armor;

    public void AddStats(ItemStats stats)
    {
        moveSpeed += stats.moveSpeed;
        attackRange += stats.attackRange;
        attackDamage += stats.bonusDamage;
        attackCooldown -= stats.attackCooldownReduction; // Assuming cooldown reduction is a positive value
        health += stats.bonusHealth;
        armor += stats.bonusArmor;
    }

    public void RemoveStats(ItemStats stats)
    {
        moveSpeed -= stats.moveSpeed;
        attackRange -= stats.attackRange;
        attackDamage -= stats.bonusDamage;
        attackCooldown += stats.attackCooldownReduction;
        health -= stats.bonusHealth;
        armor -= stats.bonusArmor;
    }
}