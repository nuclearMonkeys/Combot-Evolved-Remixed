using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletBase : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float damage;
    public float speed;
    public PlayerController source;

    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    public void SetDirection(Vector2 direction)
    {
        // if this component was added dynamically
        if(!rb)
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
        }
        transform.right = direction;
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!ExtendedOnTriggerEnter2D(other))
            return;
        // if is a bullet object
        if (CompareTag("Bullet"))
        {
            // if hit player
            if (other.CompareTag("Player"))
            {
                // player takes damage
                other.GetComponent<PlayerHealth>().TakeDamage(damage, source);
                Destroy(this.gameObject);
            }
            // if hit TNT
            else if (other.CompareTag("TNT"))
            {
                // Explode the TNT
                other.GetComponent<TNT>().Explode(source);
                Destroy(gameObject);
            }
            // if hit Block
            else if (other.gameObject.layer == LayerManager.BLOCK)
            {
                // Destroy the bullet
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
                Destroy(this.gameObject);
                TankSelectionManager.instance.CheckAllPlayerStatus();
            }
            else if (other.CompareTag("Crate")) 
            {
                other.GetComponent<StageObjRotate>().HitObj();
                Destroy(this.gameObject);
            }
            else if (other.CompareTag("StagePlane")) 
            {
                other.GetComponent<StageObjRotate>().HitObj();
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

    // return whether or not to continue checking trigger
    public virtual bool ExtendedOnTriggerEnter2D(Collider2D other) { return true; }
}
