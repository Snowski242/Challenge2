using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 3f;
    public Vector3 offset = new Vector3 (0, 4, 0);
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        transform.localPosition += offset;
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + cam.forward); 
    }
}
