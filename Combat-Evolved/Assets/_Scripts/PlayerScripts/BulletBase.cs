using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage;
    public float speed;
    public PlayerController source;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet")) 
        {
            // if not from the same source
            if(other.GetComponent<BulletBase>().source != source)
                Destroy(other.gameObject);
        }
        else if(other.CompareTag("Player")) 
        {
            other.GetComponent<PlayerHealth>().TakeDamage(this);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.layer == LayerManager.BLOCK)
        {
            Destroy(gameObject);
        }
    }
}
