using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldChunk : MonoBehaviour
{
    [SerializeField] private GameObject PlayerSpawnPoint; // Player spawn point
    [SerializeField] private GameObject[] npcPrefabs; // NPC prefabs
    private MonsterPool monsterPool; // Shared MonsterPool
    private Spawner[] monsterSpawnPoints; // Spawners for monsters
    private Spawner[] npcSpawnPoints; // Spawners for NPCs
    void Awake()
    {
        monsterPool = GetComponent<MonsterPool>(); // Get the MonsterPool component from the same GameObject
    }
    private void OnEnable()
    {
        EventManager<Monster>.Subscribe(EventKeys.MonsterDied, OnMonsterDied);
        EventManager<int>.Subscribe(EventKeys.RespawnPlayer, RespawnPlayer);
    }

    private void OnDisable()
    {
        EventManager<Monster>.Unsubscribe(EventKeys.MonsterDied, OnMonsterDied);
        EventManager<int>.Unsubscribe(EventKeys.RespawnPlayer, RespawnPlayer);
    }


    public void GenerateMap()
    {
        Debug.Log("Generating map for chunk: ");
        GetSpawnPoints(); // Find all spawners in the scene
        SpawnNPCs(); // Spawn NPCs at their spawners
        InitializeMonsterSpawners(); // Initialize monster spawners with the MonsterPool
    }

    private void GetSpawnPoints()
    {
        // Find the root objects for monsters and NPCs
        GameObject monstersSpawnerRoot = gameObject.transform.Find("Monsters").gameObject;
        GameObject npcsSpawnerRoot = gameObject.transform.Find("NPCs").gameObject;

        // Get all spawners under the root objects
        monsterSpawnPoints = monstersSpawnerRoot.GetComponentsInChildren<Spawner>();
        npcSpawnPoints = npcsSpawnerRoot.GetComponentsInChildren<Spawner>();
    }

    private void SpawnNPCs()
    {
        foreach (Spawner point in npcSpawnPoints)
        {
            Debug.Log("Spawning NPCs at: " + point.transform.position);
            // Randomly select an NPC prefab and spawn it at the spawner's position
            GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            Instantiate(prefab, point.transform);
        }
    }

    private void InitializeMonsterSpawners()
    {
        foreach (Spawner spawner in monsterSpawnPoints)
        {
            spawner.Initialize(monsterPool); // Pass the shared MonsterPool to each spawner
        }
    }

    private void OnMonsterDied(Monster monster)
    {
        if (monster == null)
        {
            Debug.LogError("MonsterDied event received, but data is not a Monster");
            return;
        }

        // Return the monster to the pool (handled by the spawner system)
        monsterPool.Return(monster);
    }
    private void RespawnPlayer(int spawnpoint)
    {
        GameManager.Instance.GetPlayer().transform.position = PlayerSpawnPoint.transform.position;
    }
}
