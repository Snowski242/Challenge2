using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPBall : MonoBehaviour, ICollectable
{
    public bool canbePickedUp = false;
    Rigidbody rb;
    bool hasTarget;

    [SerializeField] public int healAmount;
    Vector3 targetPosition;
    [SerializeField] float moveSpeed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CanBePickedUp());
    }

    public void Collect()
    {
        Debug.Log("Heal!");

        var stats = FindObjectOfType<PlayerStats>();
        stats.RegainMP(healAmount);
        var healthUI = FindObjectOfType<HealthUI>();
        healthUI.lerpTimer = 3f;
        //AudioSource.PlayClipAtPoint(soundEffect, transform.position);

        Destroy(gameObject);


    }

    private void FixedUpdate()
    {
        if (hasTarget && canbePickedUp)
        {
            Vector3 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector3(targetDirection.x, targetDirection.y, targetDirection.z) * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    IEnumerator CanBePickedUp()
    {
        yield return new WaitForSeconds(0.8f);
        canbePickedUp = true;
    }
}
