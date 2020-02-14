using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingArea : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Player")) 
        {
            print("Homing at Player: " + other);
        }
    }
}
