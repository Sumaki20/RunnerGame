using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    PlayerMovement playerMovement;
    public Slider slider;
    public Slider backSlider;
    public Gradient gradient;
    public Image fill;
    public float lerpTimer;
    public float chipSpeed = 2f;

    private void Start()
    {
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
        UpdateHealth();

    }
    public void SetMaxHealth(int Health)
    {
        slider.maxValue = Health;
        slider.value = Health;
        backSlider.maxValue = Health;
        backSlider.value = Health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int Health) 
    {
        slider.value = Health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        
        
    }

    public void UpdateHealth()
    {
        float fillA = slider.value = playerMovement.currentHealth;
        float fillB = backSlider.value;
        if (fillB > fillA)
        {
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backSlider.value = Mathf.Lerp(fillB, fillA, percentComplete);
        }
    }
}
