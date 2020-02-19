using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateRotate : MonoBehaviour
{
    private bool isHit = false;
    public float normalSpeed = 50f;
    public float hitSpeed = 100;
    public float currentSpeed;
    public float regenerateInterval = 1f;
    public float updateSpeedSecs = 5f;

    private float elapsed;

    Coroutine accelRoutine;
    Coroutine decelRoutine;

    void Start() 
    {
        // currentSpeed = normalSpeed;
    }

    void Update()
    {
        transform.Rotate(0, currentSpeed * Time.deltaTime, 0);
    }

    public void HitCrate() 
    {
        if (!isHit)
            Accelerate();
        else if (isHit) 
        {
            elapsed = 0;
            currentSpeed = hitSpeed;
        }
    }

    private void Decelerate() 
    {
        isHit = false;
        decelRoutine = StartCoroutine(DecelerateRoutine());
    }

    private void Accelerate() 
    {
        isHit = true;
        accelRoutine = StartCoroutine(AccelerateRoutine());
    }

    private IEnumerator AccelerateRoutine() 
    {
        elapsed = 0;
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime / 2;
            currentSpeed = Mathf.Lerp(hitSpeed, normalSpeed, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }
        accelRoutine = null;
        currentSpeed = normalSpeed;
        isHit = false;
        // yield return new WaitForSecondsRealtime(2);
        // Decelerate();
        // decelRoutine = StartCoroutine(DecelerateRoutine());
    }

    private IEnumerator DecelerateRoutine() 
    {
        elapsed = 0;
        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime / 2;

            if (elapsed == 0) {
                decelRoutine = null;
                yield break;
            }

            currentSpeed = Mathf.Lerp(currentSpeed, normalSpeed, elapsed / updateSpeedSecs);
            yield return null;
        }
        decelRoutine = null;
        print("finish");
        currentSpeed = normalSpeed;
        // yield return new WaitForSecondsRealtime(2);
        
    }


}
