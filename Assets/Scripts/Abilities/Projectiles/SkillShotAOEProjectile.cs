using UnityEngine;
using UnityEngine.VFX;

public class SkillShotAOEProjectile : MonoBehaviour
{
    public float maxDistance;
    public float speed;
    public int damage;
    Vector3 direction;
    Vector3 startPosition;
    public GameObject aoePrefab;
    public float aoeDuration;
    private Vector3 target;
    private VisualEffect visualEffect;
    private Vector3 lastPosition;

    public void Initialize(Vector3 target, int damage, GameObject aoePrefab, float aoeDuration)
    {
        // Ignore y axis for movement
        this.target = new Vector3(target.x, transform.position.y, target.z);
        this.damage = damage;
        this.aoePrefab = aoePrefab;
        this.aoeDuration = aoeDuration;
        startPosition = transform.position;
        direction = (this.target - startPosition).normalized;
    }

    void Start()
    {
        // Get the VisualEffect component from a child object
        visualEffect = GetComponentInChildren<VisualEffect>();
        lastPosition = transform.position;
    }

    private void UpdateVisualEffectVelocity(Vector3 velocity)
    {
        if (visualEffect != null)
        {
            visualEffect.SetVector3("Velocity", velocity);
        }
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
        // Calculate current velocity
        Vector3 currentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        // Convert world velocity to local velocity relative to the VFX
        Vector3 localVelocity = currentVelocity;
        if (visualEffect != null)
        {
            localVelocity = visualEffect.transform.InverseTransformDirection(currentVelocity);
        }
        UpdateVisualEffectVelocity(localVelocity);
        lastPosition = transform.position;
        // Check if reached the target
        float sqrDistToTarget = (transform.position - target).sqrMagnitude;
        if (sqrDistToTarget < 0.04f) // or another small threshold
        {
            SpawnAOE();
            Destroy(gameObject);
            return;
        }

        // Check if traveled max distance
        float sqrDistTraveled = (transform.position - startPosition).sqrMagnitude;
        if (sqrDistTraveled > maxDistance * maxDistance)
        {
            SpawnAOE();
            Destroy(gameObject);
            return;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponent<Monster>().TakeDamage(damage);
            SpawnAOE();
            Destroy(gameObject);
        }
    }

    private void SpawnAOE()
    {
        if (aoePrefab != null)
        {
            // Rotate -90 degrees on the X axis
            Vector3 aoePosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
            GameObject aoe = Instantiate(aoePrefab, aoePosition, Quaternion.Euler(-90f, 0f, 0f));
            Destroy(aoe, aoeDuration);
        }
    }
}
