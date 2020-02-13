using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCollider : MonoBehaviour
{
    public Flamethrower flamethrower;

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            flamethrower.hittingPlayer = true;
            flamethrower.Hit(other.GetComponent<PlayerHealth>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        flamethrower.hittingPlayer = false;
    }
}
