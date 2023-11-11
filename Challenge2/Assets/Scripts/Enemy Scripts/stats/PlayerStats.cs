using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public float currentHealth;
    public float maxHealth = 20;
    public float currentMP;
    public float maxMP = 50;

    public float baseDmg = 1;
    public int critRate = 3;

    public int attack = 5;
    public int defense = 3;
    public int magic = 4;

    public float exp;
    public int expLimit = 100;
    void Start()
    {
        for (int i = 1; i < level; i++)
        {
            attack += 1;
            defense += 1;
            maxHealth += 5;
        }
        currentHealth = maxHealth;
        currentMP = maxMP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        damage -= defense;
        if (damage < 0)
        {
            damage = 1;
        }
        currentHealth -= damage ;
    }

    public void RegainHealth(int heal)
    {
        currentHealth += heal;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void RegainMP(int mp)
    {
        currentMP += mp;
        if(currentMP > maxMP)
        {
            currentMP = maxMP;
        }
    }

    public void LevelUp()
    {
        level++;
        expLimit += 300;

        attack += 1;
        defense += 1;
        maxHealth += 5;
    }

    public void ExperienceGain(float experience)
    {
        exp += experience;
        if(exp >= expLimit)
        {
            LevelUp();
        }
    }
}
