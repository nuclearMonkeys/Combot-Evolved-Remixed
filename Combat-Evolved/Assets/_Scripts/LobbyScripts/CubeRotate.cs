using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    public float maxCoolDown = 0.8f;
    //public float cooldown = 0.8f;
    public float timeRemaining = 0.8f;

    void Update ()
    {
        if( timeRemaining <= 0f )
        {
            StartCoroutine( RotateAround( Vector3.right, 90.0f, 1.0f) );
            timeRemaining = maxCoolDown;
        }
        timeRemaining -= Time.deltaTime;
    }
 
    IEnumerator RotateAround( Vector3 axis, float angle, float duration )
    {
        float elapsed = 0.0f;
        float rotated = 0.0f;
        while( elapsed < duration ) {
            float step = angle / duration * Time.deltaTime;
            transform.RotateAround(transform.position, axis, step );
            elapsed += Time.deltaTime;
            rotated += step;
            yield return null;
        }
        transform.RotateAround(transform.position, axis, angle - rotated );
    }
}