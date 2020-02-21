using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    void Update()
    {
        if (Input.GetAxis("Jump") == 1) 
        {
            StartCoroutine(CircularFire());
        }
        // this.transform.Rotate(0, 0, 50 * Time.deltaTime);
    }

    IEnumerator CircularFire() 
    {
        print("CircularFire");
        yield return null;
    }
}
