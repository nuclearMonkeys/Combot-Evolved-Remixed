using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObjRotate : MonoBehaviour
{
    [Header("Speed Variables")]
    
    public float normalSpeed = 50f;
    public float hitSpeed = 100;
    public float currentSpeed;
    public float regenerateInterval = 1f;
    public float updateSpeedSecs = 5f;
    public bool enableSound;
    private float soundPlaytime = 0f;

    [Header("Rotate Variables")]
    public bool constantRotate;
    public bool x;
    public bool y;
    public bool z;

    private bool isHit = false;
    private float elapsed;
    Coroutine accelRoutine;

    void Start() 
    {
        // currentSpeed = normalSpeed;
    }

    void Update()
    {
        if (!constantRotate && !isHit)
            return;

        transform.Rotate(
            x ? currentSpeed * Time.deltaTime : 0,
            y ? currentSpeed * Time.deltaTime : 0, 
            z ? currentSpeed * Time.deltaTime : 0 );
    }

    public void HitObj() 
    {
        if (!isHit)
            Decelerate();
        else if (isHit) 
        {
            elapsed = 0;
            currentSpeed = hitSpeed;
        }
    }

    private void Decelerate() 
    {
        isHit = true;
        accelRoutine = StartCoroutine(DecelerateRoutine());
    }

    private IEnumerator DecelerateRoutine() 
    {
        elapsed = 0;
        while (elapsed < updateSpeedSecs ||
            constantRotate ? true : Mathf.Floor(gameObject.transform.eulerAngles.y % 180) > 1) 
        {
            elapsed += Time.deltaTime / 2;

            if (soundPlaytime > 0)
                soundPlaytime -= Time.deltaTime / 1.5f;
            else
                soundPlaytime = elapsed / 11;
        
            if(soundPlaytime <= 0 && enableSound)
                AudioManager.instance.PlaySound("gears_grinding");
            currentSpeed = Mathf.Lerp(hitSpeed, normalSpeed, elapsed / updateSpeedSecs);
            yield return new WaitForEndOfFrame();
        }
        accelRoutine = null;
        currentSpeed = normalSpeed;
        isHit = false;
        
    }
}
