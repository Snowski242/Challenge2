using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Mun")
        {
            var coin = collision.gameObject.GetComponent<Coin>();
            coin.SetTarget(transform.parent.position);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Mun")
        {
            var coin = collision.gameObject.GetComponent<Coin>();
            coin.SetTarget(transform.parent.position);
        }

        if (collision.gameObject.tag == "HPBall")
        {
            var coin = collision.gameObject.GetComponent<HPBall>();
            coin.SetTarget(transform.parent.position);
        }

        if (collision.gameObject.tag == "MPBall")
        {
            var coin = collision.gameObject.GetComponent<MPBall>();
            coin.SetTarget(transform.parent.position);
        }
    }
}
