using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRotate : MonoBehaviour
{
    private bool isHit = false;
    public float normalSpeed = 50f;
    public float hitSpeed = 100;
    public float currentSpeed;
    public float regenerateInterval = 1f;
    public float updateSpeedSecs = 5f;

    Coroutine accelRoutine;
    Coroutine decelRoutine;

    void Start() 
    {
        // currentSpeed = normalSpeed;
    }

    void Update()
    {
        
        if (Input.GetAxis("Jump") == 1) {
            // Accelerate();
            Decelerate();
        }
        transform.Rotate(0, currentSpeed * Time.deltaTime, 0);
    }

    private void Decelerate() 
    {
        decelRoutine = StartCoroutine(DecelerateRoutine());
    }

    private void Accelerate() 
    {
        accelRoutine = StartCoroutine(AccelerateRoutine());
    }

    private IEnumerator AccelerateRoutine() 
    {
        float elapsed = 0;
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            currentSpeed = Mathf.Lerp(normalSpeed, hitSpeed, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }
        accelRoutine = null;
        currentSpeed = hitSpeed;
    }

    private IEnumerator DecelerateRoutine() 
    {
        float elapsed = 0;
        print("hey");
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            currentSpeed = Mathf.Lerp(hitSpeed, normalSpeed, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }
        accelRoutine = null;
        currentSpeed = normalSpeed;
    }


}
