using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int MaxCapacity = 5; // Maximum number of monsters this spawner can hold
    public float SpawnDelay = 3f; // Delay between spawns
    public int CurrentMonsterCount { get; private set; } = 0;
    public int SpawnRange = 5; // Range for spawning monsters

    private MonsterPool monsterPool;
    private bool isSpawning = false; // Tracks if the SpawnMonsters coroutine is active

    private void OnEnable()
    {
        EventManager<Monster>.Subscribe(EventKeys.MonsterDied, OnMonsterDeath);
    }
    void OnDisable()
    {
        EventManager<Monster>.Unsubscribe(EventKeys.MonsterDied, OnMonsterDeath);
    }
    public void Initialize(MonsterPool pool)
    {
        monsterPool = pool;
        StartSpawning();
    }

    private void StartSpawning()
    {
        if (monsterPool == null)
        {
            Debug.LogError("Spawner: MonsterPool is not initialized. Make sure to call Initialize() before starting the spawner.");
            return;
        }
        if (!isSpawning)
        {
            StartCoroutine(SpawnMonsters());
        }
    }

    private IEnumerator SpawnMonsters()
    {
        isSpawning = true;

        while (CurrentMonsterCount < MaxCapacity)
        {
            // Spawn a monster
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-SpawnRange, SpawnRange), 0, Random.Range(-SpawnRange, SpawnRange));
            Monster monster = monsterPool.Get(spawnPosition);
            monster.spawnPoint = transform.position; // Assign the spawn point to the monster
            CurrentMonsterCount++;

            yield return new WaitForSeconds(SpawnDelay); // Wait for the spawn delay
        }

        isSpawning = false; // Mark coroutine as inactive when done
    }

    public void OnMonsterDeath(Monster monster)
    {
        if (monster.spawnPoint == transform.position)
        {
            CurrentMonsterCount--;

            // Restart the SpawnMonsters coroutine if it's not already running
            if (!isSpawning)
            {
                StartSpawning();
            }
        }
    }
}
