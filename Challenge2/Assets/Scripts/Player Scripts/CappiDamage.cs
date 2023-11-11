using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CappiDamage : MonoBehaviour
{
    public bool invulnerable;
    private Animator animator;
    public CappiController controller;
    public PlayerStats playerStats;
    public static event Action OnHit;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyHitbox" && !invulnerable)
        {
            
            var script = other.transform.GetComponentInParent<EnemyStats>();
            playerStats.TakeDamage(script.attack);
            StartCoroutine(Invincibility());
            
        }

        if (other.gameObject.tag == "EnemyComboHitbox" && !invulnerable)
        {

            var script = other.transform.GetComponentInParent<EnemyStats>();
            playerStats.TakeDamage(script.attack);
            StartCoroutine(Combo());

        }
    }

    IEnumerator Invincibility()
    {
        invulnerable = true;
        OnHit.Invoke();
        
        controller.canMove = false;
        yield return new WaitForSeconds(1.5f);
        invulnerable = false;
        controller.canMove = true;
    }

    IEnumerator Combo()
    {
        invulnerable = true;
        OnHit.Invoke();
        
        controller.canMove = false;
        yield return new WaitForSeconds(0.1f);
        invulnerable = false;
        yield return new WaitForSeconds(1f);
        controller.canMove = true;
    }
}
