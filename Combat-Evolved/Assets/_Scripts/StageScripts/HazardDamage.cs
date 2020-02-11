using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDamage : MonoBehaviour
{
    public PlayerController cause;
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerManager.TANKWALLCOLLIDER)
        {
            collision.transform.parent.GetComponentInChildren<PlayerHealth>().TakeDamage(damage, cause);
        }
        else if(collision.CompareTag("TNT"))
        {
            TNT tnt = collision.GetComponent<TNT>();
            tnt.StartCoroutine(tnt.DelayedExplode(cause));
        }
    }
}
