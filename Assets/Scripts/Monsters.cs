using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monsters : MonoBehaviour
{
    public string monsterName;
    public int monsterLevel;
    public int monsterDamage;
    public int monsterMaxHP;
    public int monsterCurrentHP;
    public Animator PlayerAnimator;
    public Animator EnemyAnimator;
    public bool TakeDamage(int dmg)
    {
        monsterCurrentHP -= dmg;

        if (monsterCurrentHP <= 0)
            return true;
        else
            return false;
    }
    public void Heal(int amount)
    {
        monsterCurrentHP += amount;
        if (monsterCurrentHP > monsterMaxHP)
            monsterCurrentHP = monsterMaxHP;
    }
}
