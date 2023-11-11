using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MPUI : MonoBehaviour
{
    public PlayerStats stats;
    public Image fillImage;
    public Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
    }


    void Update()
    {
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value > slider.minValue)
        {
            fillImage.enabled = true;
        }
        float fillValue = stats.currentMP / stats.maxMP;

        slider.value = Mathf.Lerp(fillValue, stats.currentMP / stats.maxMP, 3f);
    }
}
