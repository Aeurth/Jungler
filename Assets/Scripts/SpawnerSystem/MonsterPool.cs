using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    [SerializeField] private Monster[] monsterPrefabs;
    [SerializeField] private int size;

    private Queue<Monster> pool = new Queue<Monster>();

    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            int randomPrefabIndex = Random.Range(0, monsterPrefabs.Length);
            Monster m = Instantiate(monsterPrefabs[randomPrefabIndex], transform);
            m.gameObject.SetActive(false);
            pool.Enqueue(m);
        }
    }
    public int GetSize(){
        return size;
    }
    public Monster Get(Vector3 position)
    {
        if (pool.Count > 0)
        {
            Monster monster = pool.Dequeue();
            monster.transform.position = position;
            monster.Revive();
            monster.gameObject.SetActive(true);
            return monster;
        }
        else
        {
            Debug.LogWarning("MonsterPool: Pool is empty! Consider increasing the initial size.");
            return null; // Or instantiate a new monster if needed
        }
    }

    public void Return(Monster monster)
    {
        monster.gameObject.SetActive(false);
        pool.Enqueue(monster);
    }
}
