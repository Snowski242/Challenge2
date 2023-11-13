using System.Collections;
using System.Collections.Generic;
using ThirdPersonCamera;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public PlayerStats playerStats;
    public LockOnTarget lockOnTarget;
    public Vector3 offset;
    public int level = 1;
    public float currentHealth;
    public float maxHealth = 20;

    
    public int attack = 5;
    public int defense = 3;
    public int magic = 2;

    public GameObject expFloatingPoint;

    [Header("Specific Enemy Settings/Modifiers")]
    public int baseExpAmount = 3;
    public bool isBoss = false;
    public string[] immunities;
    public int rareDropChance;
    public int rareDropNumber;
    public GameObject[] guaranteedDrops;
    public GameObject[] rareDrops;


    void Start()
    {
        if(level <= 0)
        {
            level = 1;
        }

        for (int i = 1; i < level; i++)
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

    public void TakeDamage(float damage, string type)
    {
        //if (immunities != null && immunities. == type)
        //{
        //    damage = 0;
        //}
        damage = Mathf.RoundToInt(damage);
        damage -= defense;
        if(damage < 0)
        {
            damage = 1;
        }
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            RemoveTarget();
            if (!isBoss)
            {
                Death();
            }
            Experience();
            LootDrop();
            
        }
    }

    public void Experience()
    {
        var expMod = 5 + level + baseExpAmount * 1.25f;
        var finalExp = Mathf.RoundToInt(expMod);
        playerStats.ExperienceGain(finalExp);
    }

    public void RemoveTarget()
    {
        if (lockOnTarget.targets.Contains(gameObject.GetComponent<Targetable>()))
        {
            lockOnTarget.targets.Remove(gameObject.GetComponent<Targetable>());
            lockOnTarget.RemoveTarget(gameObject.GetComponent<Targetable>());
            lockOnTarget.HasFollowTarget = false;
        }
    }

    
    public void Death()
    {
        
        if (!isBoss && expFloatingPoint != null)
        {
            var expMod = 5 + baseExpAmount + level * 1.25f;
            var finalExp = Mathf.RoundToInt(expMod);
            var text = Instantiate(expFloatingPoint, offset = new Vector3(transform.position.x, offset.y, transform.position.z), Quaternion.identity);
            text.GetComponent<TextMeshPro>().text = finalExp.ToString() + "P";

        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        
    }

    public void LootDrop()
    {
        if (guaranteedDrops != null)
        {
            rareDropNumber = Random.Range(1, 101);
            float dropForce = 100f;
            Vector3 dropDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(1f, 1f));
            for (int i = 0; i < guaranteedDrops.Length; i++)
            {
                guaranteedDrops[i].GetComponent<Rigidbody>().AddForce(dropDirection * dropForce, ForceMode.Impulse);
                Instantiate(guaranteedDrops[i], transform.position, Quaternion.identity);
            }
        }
        
    }

    

    public void RegainHealth(int heal)
    {
        currentHealth += heal;
    }
}
