using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCollider : MonoBehaviour
{
    public Flamethrower flamethrower;
    public ParticleSystem flameParticles;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start() 
    {
        flameParticles = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        // flameParticles.Pause();
        
    }

    void OnParticleCollision(GameObject other) 
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        PlayerHealth playerHealth = other.GetComponentInChildren<PlayerHealth>();
        Block block = other.GetComponent<Block>();
        Crate crate = other.GetComponent<Crate>();
        
        if(playerHealth)
            flamethrower.Hit(playerHealth);
        else if(block)
            block.hitPoints -= 0.05f;
        else if(crate)
            crate.currentHP -= 0.05f;
    }
}
