using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        ICollectable collectible = collision.GetComponent<ICollectable>();
        if(collectible != null)
        {
            collectible.Collect();
        }
    }

    


}

