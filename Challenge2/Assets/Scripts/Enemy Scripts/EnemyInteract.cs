using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
[RequireComponent(typeof(EnemyStats))]
public class EnemyInteract : MonoBehaviour
{
    
    EnemyStats enemyStats;
    public EnemySpecialStates enemySpecialStates;
    public PlayerStats playerStats;
    public bool isInteracting = false;
    public ParticleSystem yoyoHitEffect;
    [SerializeField] private ParticleSystem fireHitEffect;
    public bool superArmor;
    Animator animator;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AttackPlayer" && !isInteracting)
        {
            var stats = FindObjectOfType<PlayerStats>();
            enemyStats.TakeDamage(stats.attack * stats.baseDmg, "standard");
            if(!superArmor)
            {
                StartCoroutine(Damage());
            }
        }

        if (other.gameObject.tag == "AttackPFire" && !isInteracting)
        {
            var stats = FindObjectOfType<PlayerStats>();
            enemyStats.TakeDamage(stats.magic * stats.baseDmg, "fire");
            if(!superArmor)
            {
                StartCoroutine(DamageFire());
            }
            
        }
    }

    

    private IEnumerator Damage()
    {
        animator.SetBool("Hurt", true);
        animator.ResetTrigger("Attack");
        isInteracting = true;
        yoyoHitEffect.Play();
        
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("Hurt", false);
        isInteracting = false;
    }

    private IEnumerator DamageFire()
    {
        animator.SetBool("Hurt", true);
        animator.ResetTrigger("Attack");
        isInteracting = true;
        
        fireHitEffect.Play();
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("Hurt", false);
        isInteracting = false;
    }
}
