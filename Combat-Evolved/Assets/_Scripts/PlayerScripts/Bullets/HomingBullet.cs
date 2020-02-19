using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : BulletBase
{
    public GameObject homingAreaPrefab;
    GameObject clone;

    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        
        clone = Instantiate(homingAreaPrefab, this.transform.position, this.transform.rotation);
        clone.GetComponent<HomingArea>().homingBullet = this;
        clone.GetComponent<HomingArea>().rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if is a bullet object
        if (CompareTag("Bullet"))
        {
            // if hit player
            if (other.CompareTag("Player"))
            {
                // player takes damage
                other.GetComponent<PlayerHealth>().TakeDamage(damage, source);
                Destroy(this.gameObject);
                Destroy(clone);
            }
            // if hit TNT
            else if (other.CompareTag("TNT"))
            {
                // Explode the TNT
                other.GetComponent<TNT>().Explode(source);
                Destroy(clone);
                Destroy(gameObject);
            }
            // if hit Block
            else if (other.gameObject.layer == LayerManager.BLOCK)
            {
                // Destroy the bullet
                Destroy(clone);
                Destroy(gameObject);
            }
            // if hit Ready
            else if (other.CompareTag("ReadyLine"))
            {
                // Set the ready line
                SpriteRenderer sprite = other.GetComponent<SpriteRenderer>();
                if (sprite.color != Color.yellow)
                    sprite.color = Color.yellow;
                else
                    sprite.color = Color.red;
                Destroy(clone);
                Destroy(this.gameObject);
                TankSelectionManager.instance.CheckAllPlayerStatus();
            }
        }
        // if is a TNT object
        else if(CompareTag("TNT"))
        {
            // if TNT hit player
            if (other.CompareTag("Player") || other.gameObject.layer == LayerManager.BLOCK || other.CompareTag("TNT"))
            {
                // Explode the tnt
                TNT tnt = GetComponent<TNT>();
                tnt.Explode(source);
            }
        }
    }
}
