using UnityEngine;

public class SkillShotProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 8;
    public float maxDistance = 30f;
    private Vector3 direction;
    private Vector3 targertPosition;
    private Vector3 startPosition;

    public void Initialize(Vector3 target, int damage, float speed)
    {
        this.startPosition = transform.position;
        this.targertPosition = new Vector3(target.x, transform.position.y, target.z);
        this.direction = (targertPosition - startPosition).normalized;
        this.damage = damage;
        this.speed = speed;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        float distanceTraveled = (transform.position - startPosition).sqrMagnitude;
        transform.LookAt(targertPosition);
        if (distanceTraveled > maxDistance * maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Monster>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
