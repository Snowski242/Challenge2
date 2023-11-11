using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook cine;
    public Transform player;
    public Transform cappi;
    void Start()
    {
        cine = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            ResetCamera();
        }
        
    }

    public void ResetCamera()
    { 
        cine.ForceCameraPosition(player.transform.position, player.transform.rotation);
        Camera.main.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y, 0);
    }
}
