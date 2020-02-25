using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushArea : MonoBehaviour
{
    public PushPassive pp;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.CompareTag("Player")) 
        {
            PlayerController otherpc = other.GetComponentInParent<PlayerController>();
            pp.PushPlayer(otherpc);
        }
    }
}
