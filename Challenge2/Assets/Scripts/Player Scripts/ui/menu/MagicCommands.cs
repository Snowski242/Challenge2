using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCommands : MonoBehaviour
{
    public int menu_cursor;
    public GameObject[] menuItems;
    public AudioSource audioSource;
    public AudioSource back;
    public Animator spellAnimator;
    public bool canInput = false;

    public PlayerCommands playerCommands;
    public GameObject magicList;
    void Start()
    {
        menu_cursor = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0.3 && canInput)
        {
            menu_cursor--;
            audioSource.Play();
            canInput = false;
            if (menu_cursor < 1)
            {
                menu_cursor = menuItems.Length;
            }

            StartCoroutine(InputDelay());
        }

        if (Input.mouseScrollDelta.y < -0.3 && canInput)
        {
            menu_cursor++;
            audioSource.Play();
            canInput = false;
            if (menu_cursor > menuItems.Length)
            {
                menu_cursor = 1;
            }

            StartCoroutine(InputDelay());
        }

        if(Input.GetKeyDown(KeyCode.Space) && canInput) 
        {
            back.Play();
            canInput = false;
            
            spellAnimator.SetBool("Selected", false);
            playerCommands.canInput = true;
            StartCoroutine(Back());
            
        }

    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.1f);
        canInput = true;
    }

    IEnumerator Back()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerManager.instance.dontJump = false;
        magicList.SetActive(false);
    }
}
