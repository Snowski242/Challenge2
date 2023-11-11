using System.Collections;
using System.Collections.Generic;
using ThirdPersonCamera;
using UnityEngine;

public class CappiAttack : MonoBehaviour
{
    public Animator anim;
    private CappiController controller;
    public PlayerStats playerStats;
    public PlayerCommands playerCommands;
    public LockOnTarget lockOnTarget;
    [Header("Combo Settings")]
    public float cooldownTime = 0.6f;
    public float nextFireTime = 0.6f;
    public int noOfClicks = 0;
    float lastClickedTime = 0;
    float maxComboDelay = 1;

    private void Start()
    {
        
        controller = GetComponent<CappiController>();
    }
    void Update()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.2f && anim.GetCurrentAnimatorStateInfo(0).IsName("CappiAttack"))
        {
            anim.SetBool("hit1", false);
            
        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.2f && anim.GetCurrentAnimatorStateInfo(0).IsName("CappiAttack2G"))
        {
            anim.SetBool("hit2", false);

        }
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.2f && anim.GetCurrentAnimatorStateInfo(0).IsName("CappiAttack3G"))
        {
            
            noOfClicks = 0;
            anim.SetBool("hit3", false);
            controller.isAttacking = false;
            controller.canMove = true;
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("CappiStance") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.09f && noOfClicks > 0)
        {
            
            noOfClicks = 0;
            controller.isAttacking = false;
            controller.canMove = true;
        }


        if (Time.time - lastClickedTime > maxComboDelay)
        {
            
            controller.isAttacking = false;
            lastClickedTime = 0;
            
        }

        //cooldown time
        if (Time.time > nextFireTime)
        {
            // Check for mouse input
            if (Input.GetMouseButtonDown(0) && controller.canAttack && playerCommands.menu_cursor == 1 && !controller.hanging && controller.isGrounded)
            {
                controller.state = "attack ground";
                controller.isAttacking = true;
                controller.canMove = false;
                OnClick();

            }
        }
    }

    void OnClick()
    {
        //so it looks at how many clicks have been made and if one animation has finished playing starts another one.
        lastClickedTime = Time.time;
        noOfClicks++;
        
        
        
            noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);
            if (noOfClicks == 1)
            {
                playerStats.baseDmg = 1;
            anim.SetBool("hit1", true);

            controller.canMove = false;

            if (lockOnTarget.HasFollowTarget)
            {
                Quaternion lookRotation = Quaternion.LookRotation(lockOnTarget.followTarget.transform.position - transform.position);
                lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lookRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = lookRotation;
            }
        }
            

            if (noOfClicks >= 2 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.55f && anim.GetCurrentAnimatorStateInfo(0).IsName("CappiAttack"))
            {
            playerStats.baseDmg = 1.35f;

            anim.SetBool("hit1", false);
            anim.SetBool("hit2", true);
            controller.canMove = false;
            }
            if (noOfClicks >= 3 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && anim.GetCurrentAnimatorStateInfo(0).IsName("CappiAttack2G"))
            {
            playerStats.baseDmg = 1.65f;
            anim.SetBool("hit2", false);
            anim.SetBool("hit3", true);

        }
        controller.canMove = false;

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        
        
    }
}
