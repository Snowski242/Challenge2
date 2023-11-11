using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public PlayerStats stats;
    public Image fillImage;
    public Slider backImage;
    public Slider slider;

    public float lerpTimer;
    public float chipSpeed = 3f;

    private void OnEnable()
    {
        CappiDamage.OnHit += UpdateHealthUI;
    }

    private void OnDisable()
    {
        CappiDamage.OnHit -= UpdateHealthUI;
    }
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        if(slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if(slider.value > slider.minValue)
        {
            fillImage.enabled = true;
        }
        float fillValue = stats.currentHealth /stats.maxHealth;
        
        slider.value = fillValue;

        
        float fillB = backImage.value;
        if(fillB > fillValue)
        {
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backImage.value = Mathf.Lerp(fillB, fillValue, percentComplete);
        }

        if(fillB < fillValue)
        {
            backImage.value = fillValue;
        }
        

    }

    public void UpdateHealthUI()
    {
        Debug.Log("Method running");
        lerpTimer = 0f;
        
    }
}
