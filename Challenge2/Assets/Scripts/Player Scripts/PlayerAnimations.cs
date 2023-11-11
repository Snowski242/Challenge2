using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;
    private string currentState;
    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.Play("CappiStance");
    }

    void ChangeAnimationState(string newState)
    {
        if(currentState == newState)
        {
            return;
        }

        animator.Play(newState);

        currentState = newState;
    }
}
