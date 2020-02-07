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
        Destroy(this.gameObject, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player")) 
        {
            other.GetComponent<PlayerHealth>().TakeDamage(this);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.layer == LayerManager.BLOCK)
        {
            Destroy(gameObject);
        }
        else if(other.CompareTag("ReadyLine")) 
        {
            SpriteRenderer sprite = other.GetComponent<SpriteRenderer>();
            if(sprite.color != Color.yellow)
                sprite.color = Color.yellow;
            else
                sprite.color = Color.red;
            Destroy(this.gameObject);
            TankSelectionManager.instance.CheckAllPlayerStatus();
        }
    }
}
