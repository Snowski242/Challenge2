using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToPoint : MonoBehaviour
{
    public GameObject Teleport;
    public GameObject TeleportTarget;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            TeleportTarget.transform.position = Teleport.transform.position;
        }
    }
}
