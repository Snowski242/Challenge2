using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeonStats : EnemyStats
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i < level; i++)
        {
            attack += 1;
            defense += 1;
            maxHealth += 5;
        }
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
