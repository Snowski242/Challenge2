using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoManager : MonoBehaviour
{
    public TrailRenderer trail;
    public CappiController controller;
    public SphereCollider hitbox;
    public ParticleSystem hitEffect;
    public AudioSource yoyoHit;
    // Start is called before the first frame update
    void Start()
    {
        trail.enabled = false;
        hitbox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
       
        if(controller.isAttacking)
        {
            trail.enabled = true;
            hitbox.enabled=true;
        }
        else
        {
            trail.enabled = false;
            hitbox.enabled = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.transform.GetComponent<EnemyInteract>() != null)
            {
                var script = other.transform.GetComponent<EnemyInteract>();
                if (script.isInteracting)
                {
                    yoyoHit.Play();
                }
            }
        }
    }
}
