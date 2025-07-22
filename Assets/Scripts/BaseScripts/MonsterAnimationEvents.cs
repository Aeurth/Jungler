using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimationEvents : MonoBehaviour
{
    [SerializeField] private Monster monster;
    public void OnMonsterAttackHit()
    {
        monster.OnMonsterAttackHit();
    }
    public void OnMonsterAttackEnd()
    {
        monster.OnMonsterAttackEnd();
    }
}
