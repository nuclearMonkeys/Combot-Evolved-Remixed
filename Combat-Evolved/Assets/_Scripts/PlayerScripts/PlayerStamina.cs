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
    public int maxStamina = 5;
    
    public float regenerateInterval= 1f;
    public float updateSpeedSecs = 0.15f;

    Coroutine barRoutine;
    Coroutine regenRoutine;
    
    void Start()
    {
        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value    = currentStamina;
    }

    // Query to check if has enough stamina
    public bool HasEnoughStamina(int amount)
    {
        return amount <= currentStamina;
    }

    // Reduce stamina
    public void DecrementStamina(int amount) 
    {
        if(!HasEnoughStamina(amount))
        {
            Debug.Log("Not Enough Stamina!!! Please Check HasEnoughStamina!!");
            Debug.Break();
        }
        // stop existing bar animation
        if (barRoutine != null)
        {
            StopCoroutine(barRoutine);
            staminaSlider.value = currentStamina;
        }
        // start bar animation
        currentStamina -= amount;
        barRoutine = StartCoroutine(BarAnimation(currentStamina));
        // regen if not already regening
        if (regenRoutine == null)
            regenRoutine = StartCoroutine(RegenerateStamina());
    }

    // Passively regenerates stamina
    private IEnumerator RegenerateStamina() 
    {
        if (currentStamina >= maxStamina)
        {
            Debug.Log("Too much Stamina!! Something went wrong...");
            Debug.Break();
        }

        yield return new WaitForSeconds(regenerateInterval);

        // stop existing bar animation
        if (barRoutine != null)
        {
            StopCoroutine(barRoutine);
            staminaSlider.value = currentStamina;
        }
        // start bar animation
        currentStamina += staminaRegenAmount;
        barRoutine = StartCoroutine(BarAnimation(currentStamina));

        // continue regening if not max stamina
        if (currentStamina < maxStamina)
            regenRoutine = StartCoroutine(RegenerateStamina());
        // stop regening if max
        else
            regenRoutine = null;
    }

    // Animates the stamina bar to a certain value
    private IEnumerator BarAnimation(int value) 
    {
        float elapsed = 0f;

        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            staminaSlider.value = Mathf.Lerp(staminaSlider.value, value, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }
        staminaSlider.value = value;
        // successfully ended a bar animation routine
        barRoutine = null;
    }

    // Refills stamina
    public void ResetStamina()
    {
        StopAllCoroutines();
        regenRoutine = null;
        barRoutine = null;
        currentStamina = maxStamina;
        staminaSlider.value = currentStamina;
    }
}
