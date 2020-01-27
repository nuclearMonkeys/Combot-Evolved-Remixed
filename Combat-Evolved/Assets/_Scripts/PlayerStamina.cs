using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider staminaSlider;
    private int staminaRegenAmount = 1;
    public int currentStamina = 0;
    public bool resetStamina;
    public int maxStamina = 5;
    
    public int cooldown = 0;
    public float updateSpeedSecs = 0.5f;
    
    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value    = currentStamina;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RegenerateStamina();
        if (cooldown > 0)
            cooldown--;
    }

    public void DecrementStamina() 
    {
        if (currentStamina > 0)
            StartCoroutine(BarAnimation(--currentStamina));
    }

    public void RegenerateStamina() 
    {
        if (currentStamina != maxStamina && cooldown == 0) 
        {
            StartCoroutine(BarAnimation(++currentStamina));
            cooldown = 30;
        }
    }

    private IEnumerator BarAnimation(int value) 
    {
        float elapsed = 0f;

        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            staminaSlider.value = Mathf.Lerp(staminaSlider.value, value, elapsed / updateSpeedSecs);
            yield return null;
        }
        
        staminaSlider.value = value;
    }
}
