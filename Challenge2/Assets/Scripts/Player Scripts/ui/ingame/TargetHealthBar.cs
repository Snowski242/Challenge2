using System.Collections;
using System.Collections.Generic;
using ThirdPersonCamera;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class TargetHealthBar : MonoBehaviour
{
    public bool enable;
    public Image fillImage;
    public Image lerpImage;
    public Image backgroundImage;
    public Slider backImage;
    public Slider slider;

    public LockOnTarget lockOnTarget;

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
        if (lockOnTarget.HasFollowTarget)
        {
            enable = true;
            var enemyStats = lockOnTarget.followTarget.GetComponent<EnemyStats>();

            if (slider.value <= slider.minValue)
            {
                fillImage.enabled = false;
            }

            if (slider.value > slider.minValue)
            {
                fillImage.enabled = true;
            }
            float fillValue = enemyStats.currentHealth / enemyStats.maxHealth;

            slider.value = fillValue;


            float fillB = backImage.value;
            if (fillB > fillValue)
            {
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                backImage.value = Mathf.Lerp(fillB, fillValue, percentComplete);
            }

            if (fillB < fillValue)
            {
                backImage.value = fillValue;
            }
        }
        else if(lockOnTarget.HasFollowTarget == false || lockOnTarget.followTarget == null)
        {
            enable = false;
        }
        
        if(enable)
        {
            slider.enabled = true;
            backImage.enabled = true;
            fillImage.enabled = true;
            backgroundImage.enabled = true;
            lerpImage.enabled = true;
        }
        else
        {
            fillImage.enabled = false;
            backgroundImage.enabled = false;
            lerpImage.enabled = false;
        }
        


    }

    public void UpdateHealthUI()
    {
        Debug.Log("Method running");
        lerpTimer = 0f;

    }
}
