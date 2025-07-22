using UnityEngine;

public class TabTargetProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private Transform target;
    private bool isActive = false;

    public void Initialize(Transform target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        isActive = true;
    }

    void Update()
    {
        if (!isActive || target == null) { Destroy(gameObject); return; }
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        transform.LookAt(target);
        if ((transform.position - target.position).sqrMagnitude < 0.04f)
        {
            target.GetComponent<Monster>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
