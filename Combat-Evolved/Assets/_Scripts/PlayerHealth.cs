using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    public float currentHP;

    public bool resetHP;
    public float maxHP;
    public UnityEvent damageEvent;
    public UnityEvent deathEvent;

    void Start() 
    {
        if (resetHP)
            currentHP = maxHP;
    }

    public void TakeDamage(DefaultBullet bullet) 
    {
        currentHP -= bullet.damage;
        
        if (currentHP < 0.0f) 
        {
            Die(bullet);
        }
    }

    public void Die(DefaultBullet bullet) 
    {
        // Later on, tell GameManager this Player X killed
        // Player Y or something...
        Destroy(this.gameObject);
    }
}
