using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicButtonDefinition : MonoBehaviour
{
    public int buttonNum;
    public Animator animator;
    public MagicCommands playerCommands;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonNum == playerCommands.menu_cursor)
        {
            animator.SetBool("Selected", true);
        }
        else
        {
            animator.SetBool("Selected", false);
        }
    }
}
