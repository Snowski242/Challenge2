using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ThirdPersonCamera;
using UnityEngine;

public class CappiMagicAttack : MonoBehaviour
{
    public Animator anim;
    private CappiController controller;
    public PlayerStats playerStats;
    public MagicCommands magicCommands;
    public LockOnTarget lockOnTarget;
    [SerializeField] private AudioClip fireSFX;
    [SerializeField] private ParticleSystem fireExplosion;
    public float delay;
    [SerializeField] public float delayDefault = 1.5f;
    [SerializeField] private float delayDecrease;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    private bool canUseMagic = false;
    void Start()
    {
        
        controller = GetComponent<CappiController>();
        delay = delayDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if(delay > 0)
        {
            delay -= delayDecrease;
        }
        else
        {
            canUseMagic = true;
        }

        if(magicCommands.canInput)
        {
            if (Input.GetMouseButtonDown(0) && controller.canAttack && magicCommands.menu_cursor == 1 && !controller.hanging)
            {
                //Feu
                if (playerStats.currentMP > 5 && canUseMagic && !PlayerManager.instance.tier2Fire && !PlayerManager.instance.tier3Fire)
                {
                    controller.state = "magic fire";
                    anim.SetBool("Fire", true);
                    controller.isAttacking = true;
                    controller.canMove = false;
                    controller.canAttack = false;
                    playerStats.currentMP -= 5;
                    

                    if (lockOnTarget.HasFollowTarget)
                    {
                        Quaternion lookRotation = Quaternion.LookRotation(lockOnTarget.followTarget.transform.position - transform.position);
                        lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                        transform.rotation = lookRotation;
                    }

                }
                //Feura

                //Feurava



            }
        }
        
    }

    

    void StopFire()
    {
        fireExplosion.Stop();
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.5f);
        
        delay = delayDefault;
        anim.SetBool("Fire", false);
        controller.canMove = true;
        controller.isAttacking = false;
        controller.canAttack = true;
    }
}
