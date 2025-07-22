using UnityEngine;

public class AreaEffect : MonoBehaviour
{
    public float tickRate = 1f; // How often the area effect applies
    public int damage = 5;
    public float radius = 2f;
    public LayerMask enemyLayer;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= tickRate)
        {
            timer = 0f;
            ApplyDamage();
        }
    }

    private void ApplyDamage()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (var hit in hits)
        {
            Monster monster = hit.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
        }
    }
}
