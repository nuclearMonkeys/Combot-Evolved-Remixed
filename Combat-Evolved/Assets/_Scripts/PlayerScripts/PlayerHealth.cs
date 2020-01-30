using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;

    public float currentHP;
    public bool resetHP;
    public float maxHP;

    void Start() 
    {
        if (resetHP)
            currentHP = maxHP;
    }

    // For Taking Damage by Bullet
    public void TakeDamage(BulletBase bullet) 
    {
        currentHP -= bullet.damage;
        healthSlider.value = currentHP / maxHP;
        
        if (currentHP <= 0.0f) 
        {
            Die(bullet);
        }
    }

    // For Taking Damage by Stage Hazard
    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        healthSlider.value = currentHP / maxHP;

        if (currentHP <= 0)
            Die(null);
    }

    public void Die(BulletBase bullet) 
    {
        // Later on, tell GameManager this Player X killed
        // Player Y or something...
        Destroy(transform.parent.gameObject);
        // if die from stage hazard
        if(bullet != null)
        {
            ScoreboardManagerScript.instance.updateScores(bullet.source.tankID);
            GameObject deathMessages = GameObject.Find("deathMessage");
            if (deathMessages != null)
            {
                deathMessages.GetComponent<deathMessages>().setMessage(bullet.source.tankID, this.GetComponentInParent<PlayerController>().tankID);
            }
        }
    }
}
