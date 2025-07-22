using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    [SerializeField] private GameObject lootBagPrefab;

    private void OnEnable()
    {
        EventManager<Monster>.Subscribe("MonsterDied", OnMonsterDied);
    }

    private void OnDisable()
    {
        EventManager<Monster>.Unsubscribe("MonsterDied", OnMonsterDied);
    }

    private void OnMonsterDied(Monster data)
    {
        Debug.Log("LootManager: mosnter died");
        if (data is Monster monster)
        {
            Inventory loot = monster.GetLoot();

            if (loot.Entries.Count > 0)
            {
                GameObject lootBag = Instantiate(lootBagPrefab, monster.transform.position, Quaternion.identity);
                LootBag bag = lootBag.GetComponent<LootBag>();
                bag.SetLoot(loot);
            }
            else
            {
                Debug.Log("empty inventory");
            }
            
        }
    }
   

}
