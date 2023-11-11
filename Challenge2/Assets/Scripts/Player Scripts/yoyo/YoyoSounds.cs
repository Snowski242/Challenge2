using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoyoSounds : MonoBehaviour
{
    public AudioSource yoyoSwing;
    public AudioSource footSteps;
    
    void YoyoSwing()
    {
        yoyoSwing.Play();
    }

    public void Footsteps()
    {
        footSteps.Play();
    }

}
