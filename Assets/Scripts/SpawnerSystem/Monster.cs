using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterState
{
    Wander,
    Alert,
    Aggro,
    Attacking
}

public class Monster : MonoBehaviour
{
    [SerializeField] private HealthBarOffset healthBarPrefab;
    [SerializeField] private int maxHealth;
    [SerializeField] public float attackCooldown;

    public string ID;
    private int health;
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public Transform player;
    public Animator animator;
    public Vector3 spawnPoint;
    public bool IsDead() => health <= 0;

    private NavMeshAgent agent;
    private float wanderDelayTimer;
    private MonsterState currentState = MonsterState.Wander;
    private HealthBarOffset myHealthBar;
    private Inventory inventory;
    

    void Start()
    {
        inventory = GetComponent<Inventory>();
        agent = GetComponent<NavMeshAgent>();
        player = GameManager.Instance.GetPlayer().transform;
        health = maxHealth;
        myHealthBar = Instantiate(healthBarPrefab, transform);
        myHealthBar.Initialize(transform, maxHealth);
    }
    //state machine
    public void Update()
    {
        switch (currentState)
        {
            case MonsterState.Wander:
                HandleWander();
                break;

            case MonsterState.Alert:
                HandleAlert();
                break;

            case MonsterState.Aggro:
                HandleAggro();
                break;
            case MonsterState.Attacking:
                break;

            default:
                break;
        }
    }

    private void HandleAlert()
    {
        FacePlayer();
        if (!IsPlayerInRange(detectionRange))
        {
            currentState = MonsterState.Wander;
        }
    }
    private void HandleWander()
    {
        if(IsPlayerInRange(detectionRange)){
            currentState = MonsterState.Alert;
            agent.ResetPath();
            animator.SetFloat("Speed", 0f);
            return;
        }
        if (agent.pathPending) return;

        if (!agent.hasPath || agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            wanderDelayTimer -= Time.deltaTime;

            if (wanderDelayTimer <= 0f)
            {
                Vector3 newPos = GetRandomNavMeshPoint(spawnPoint, wanderRadius);
                agent.SetDestination(newPos);
                wanderDelayTimer = wanderInterval;
                
            }
        }
        float normalizedSpeed = Mathf.Clamp(agent.velocity.magnitude / agent.speed, 0f, 1f);
        animator.SetFloat("Speed", normalizedSpeed);
    }
    private bool IsPlayerInRange(float range)
    {
        if (player == null || !GameManager.Instance.IsPlayerAlive()) 
            return false;

        float sqrDist = (transform.position - player.position).sqrMagnitude;
        return sqrDist <= range * range;
    }

    private void FacePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        }
    }

    private void HandleAggro()
    {
        
        if(IsPlayerInRange(attackRange))
        {
            Debug.Log($"Monster: {ID} is attacking player!");
            agent.ResetPath();
            currentState = MonsterState.Attacking;
            animator.SetTrigger("Attack");
           
        }
        else
        {
            agent.SetDestination(player.position);
            agent.stoppingDistance = attackRange * 0.9f;
            
        }
        float normalizedSpeed = Mathf.Clamp(agent.velocity.magnitude / agent.speed, 0f, 1f);
        animator.SetFloat("Speed", normalizedSpeed);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        myHealthBar.UpdateHealth(health);

        if (health <= 0)
        {
            Die();
            return;
        }

        // On first hit → enter AGGRO mode
        if (currentState != MonsterState.Aggro 
        && currentState != MonsterState.Attacking)
        {
            EventManager<int>.Subscribe(EventKeys.PlayerDied, DropAggro);
            currentState = MonsterState.Aggro;

        }
    }
    private void DropAggro(int data)
    {
        EventManager<int>.Unsubscribe(EventKeys.PlayerDied, DropAggro);
        currentState = MonsterState.Wander;
    }
    public void Revive()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameManager.Instance.GetPlayer().transform;
        
        health = maxHealth;

        // Ensure HealthBar is created and reset
        if (myHealthBar == null)
        {
            myHealthBar = Instantiate(healthBarPrefab, transform);
            myHealthBar.Initialize(transform, health);
        }

        myHealthBar.UpdateHealth(health);
        myHealthBar.gameObject.SetActive(true);
        wanderDelayTimer = wanderInterval;
        currentState = MonsterState.Wander;
    }
    private void Die()
    {
        agent.ResetPath();
        myHealthBar.gameObject.SetActive(false);
        EventManager<Monster>.Trigger(EventKeys.MonsterDied, this);
    }
    private Vector3 GetRandomNavMeshPoint(Vector3 center, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * radius;
            randomPos.y = center.y;

            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                return hit.position;
        }

        return center;
    }
    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public void OnMonsterAttackHit()
    {
        if (IsPlayerInRange(attackRange))
            EventManager<int>.Trigger(EventKeys.PlayerHit, attackDamage);

    }
    public void OnMonsterAttackEnd()
    {
        if(currentState == MonsterState.Wander)
            return;

         Debug.Log($"attack animation ended");
        if (!IsPlayerInRange(attackRange))
        {
            currentState = MonsterState.Aggro;
            return;
        }
        animator.SetTrigger("Attack");

    }
    public Inventory GetLoot()
    {
        if (inventory == null)
        {
            Debug.Log($"Monster: {ID} had no Inventory, returned null");
            return null;
        }
        return inventory;
    }

}
