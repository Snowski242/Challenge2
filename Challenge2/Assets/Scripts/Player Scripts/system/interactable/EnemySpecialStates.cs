using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialStates : MonoBehaviour
{
    public bool superArmor;
    public void TurnOnSuperArmor()
    {
        superArmor = true;
    }

    public void TurnOffSuperArmor()
    {
        superArmor = false;
    }
}
