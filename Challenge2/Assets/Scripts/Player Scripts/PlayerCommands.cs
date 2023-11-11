using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommands : MonoBehaviour
{
    [Header("Sounds and Cursor")]
    public int menu_cursor;
    public GameObject[] menuItems;
    public AudioSource audioSource;
    public AudioSource confirm;
    [Header("Magic List")]
    public MagicCommands magicCommands;
    public GameObject magicList;

    public bool canInput = true;
    void Start()
    {
        menu_cursor = 1;
        magicList.SetActive(false);
    }
    void Update()
    {
        if(Input.mouseScrollDelta.y > 0.3 && canInput)
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

        if (Input.GetMouseButtonDown(0))
        {
            if(canInput && menu_cursor == 2)
            {
                confirm.Play();
                canInput = false;
                PlayerManager.instance.dontJump = true;
                magicList.SetActive(true);
                magicCommands.canInput = true;
            }
        }

    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(0.1f);
        canInput = true;
    }
}
