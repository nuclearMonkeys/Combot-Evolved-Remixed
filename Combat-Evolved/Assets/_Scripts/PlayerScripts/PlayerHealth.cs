﻿using UnityEngine;
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

    public void TakeDamage(DefaultBullet bullet) 
    {
        currentHP -= bullet.damage;
        healthSlider.value = currentHP / maxHP;
        
        if (currentHP <= 0.0f) 
        {
            Die(bullet);
        }
    }

    public void Die(DefaultBullet bullet) 
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
