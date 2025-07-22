using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public enum PlayerState { Idle, MovingToTarget, Attacking, Dead }

public class Player : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnPoint; // optional if you want it to come from hand/staff
    [SerializeField] private Inventory inventory;
    [SerializeField] private Inventory equipment;
    [SerializeField] private HealthBar healthBar;

    private PlayerStats playerStats;
    private Animator animator;
    private NavMeshAgent agent;
    private Monster targetEnemy;
    private PlayerState currentState = PlayerState.Idle;
    private float attackTimer = 0f;
    private bool useMeleeAttack = false;

    void OnEnable()
    {
        EventManager<int>.Subscribe(EventKeys.RespawnPlayer, Respawn);
        EventManager<int>.Subscribe(EventKeys.PlayerHit, TakeDamage);
        EventManager<ItemStats>.Subscribe(EventKeys.AddPlayerStats, AddPlayerStats);
        EventManager<ItemStats>.Subscribe(EventKeys.RemovePlayerSats, RemovePlayerStats);
        playerStats = GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component is missing on the Player object.");
        }
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        playerStats = GetComponent<PlayerStats>();

        if (mainCamera == null)
            mainCamera = Camera.main;
        if (agent == null)
            Debug.LogWarning("Agent is not initialized");
        healthBar.Initialize(playerStats.health);
        agent.speed = playerStats.moveSpeed;
    }

    public void SpawnAtStart(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        // Optionally reset state (HP, movement, etc.)
        // Example:
        // currentHealth = maxHealth;
        // agent.ResetPath();

    }

    void Update()
    {
        HandleInput();
        HandleState();

    }

    void HandleMoveInput()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, enemyLayer))
            {
                Debug.Log($"Hit: {hit.collider.name}");
                targetEnemy = hit.collider.GetComponent<Monster>();

                if (targetEnemy != null)
                {
                    Debug.Log($"Targeting: {targetEnemy.name} is dead: {targetEnemy.IsDead()}");
                    MoveTo(targetEnemy.transform.position);
                    currentState = PlayerState.MovingToTarget;
                    return;
                }
                else
                {
                    Debug.LogWarning("Hit an enemy, but no Monster component found.");
                }
            }

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                MoveTo(hit.point);
                targetEnemy = null;
                currentState = PlayerState.MovingToTarget;
            }
        }
    }
    void HandleInput()
    {
        if (currentState == PlayerState.Dead)
            return;

        if (Input.GetKeyDown(KeyCode.I))
        {
            UIManager.Instance.ToggleInventory();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.Instance.TogglePlayerEquipment();
        }
        HandleMoveInput();



    }
    void HandleState()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        // Always reduce attack cooldown, regardless of state
        if (attackTimer > 0f)
            attackTimer = Mathf.Max(0f, attackTimer - Time.deltaTime);

        switch (currentState)
        {
            case PlayerState.MovingToTarget:
                if (targetEnemy != null)
                {
                    float sqrDist = (transform.position - targetEnemy.transform.position).sqrMagnitude;
                    float sqrRange = playerStats.attackRange * playerStats.attackRange;

                    if (sqrDist <= sqrRange)
                    {
                        agent.ResetPath();
                        currentState = PlayerState.Attacking;
                    }

                }
                else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
                {
                    currentState = PlayerState.Idle;
                }
                break;

            case PlayerState.Attacking:
                if (targetEnemy == null || targetEnemy.IsDead())
                {
                    currentState = PlayerState.Idle;
                    return;
                }

                FaceTarget(targetEnemy.transform.position);

                float sqrDistToEnemy = (transform.position - targetEnemy.transform.position).sqrMagnitude;
                if (sqrDistToEnemy > playerStats.attackRange * playerStats.attackRange)
                {
                    MoveTo(targetEnemy.transform.position);
                    currentState = PlayerState.MovingToTarget;
                    return;
                }

                if (attackTimer <= 0f)
                {
                    PlayAttackAnimation();
                    attackTimer = playerStats.attackCooldown;
                }
                break;
        }
    }
    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
    void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        EventManager<Vector3>.Trigger(EventKeys.PlayerMoved, position);
    }
    private void PlayAttackAnimation()
    {
        float baseAnimationDuration = 1.1f; // How long the default attack anim is at 1x speed
        float speedMultiplier = baseAnimationDuration / playerStats.attackCooldown;
        animator.SetFloat("AttackSpeed", speedMultiplier);
        animator.SetTrigger("Attack");
    }
    public void OnAttackHit()
    {
        if (targetEnemy == null || targetEnemy.IsDead())
            return;

        if (useMeleeAttack)
        {
            targetEnemy.TakeDamage(playerStats.attackDamage);
        }
        else
        {
            //GameObject fb = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
           // fb.GetComponent<Fireball>().SetTarget(targetEnemy.transform);
        }

        EventManager<Monster>.Trigger(EventKeys.EnemyHit, targetEnemy);

        if (targetEnemy.IsDead())
        {
            EventManager<Monster>.Trigger("EnemyKilled", targetEnemy);
        }
    }
    public void TakeDamage(int damage)
    {
        if (damage > playerStats.armor)
        {
            int trueDamage = damage - playerStats.armor;
            playerStats.health -= trueDamage;
            if (playerStats.health < 0)
            {
                playerStats.health = 0;
                EventManager<int>.Trigger(EventKeys.PlayerDied, 1);
                currentState = PlayerState.Dead;
                animator.Play("Death");
                agent.enabled = false;
            }
            healthBar.UpdateHealth(playerStats.health);
        }
    }
    public void Respawn(int respawnPoint)
    {
        currentState = PlayerState.Idle;
        playerStats.health = playerStats.maxHealth;
        healthBar.UpdateHealth(playerStats.health);
        agent.enabled = true;
        agent.ResetPath();
        animator.Play("Wait");
    }
    public void AddPlayerStats(ItemStats stats)
    {
        playerStats.AddStats(stats);
        UpdatePlayerComponents();
        EventManager<PlayerStats>.Trigger(EventKeys.PlayerStatsUpdated, playerStats);
    }
    public void RemovePlayerStats(ItemStats stats)
    {
        playerStats.RemoveStats(stats);
        UpdatePlayerComponents();
        Debug.Log($"Player stats removed: {stats}");
        EventManager<PlayerStats>.Trigger(EventKeys.PlayerStatsUpdated, playerStats);
    }
    public Inventory GetInventory()
    {
        return inventory;
    }
    public Inventory GetEquipment()
    {
        return equipment;
    }
    private void UpdatePlayerComponents()
    {
        agent.speed = playerStats.moveSpeed;
        healthBar.UpdateHealth(playerStats.health);
    }
    public PlayerState GetPlayerState()
    {
        return currentState;
    }

}
