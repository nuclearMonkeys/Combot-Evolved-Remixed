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

    // Taking any source of damage
    public void TakeDamage(float amount, PlayerController cause = null)
    {
        print("Taking " + amount + " From " + (cause == null ? "Hazard" : cause.name));
        currentHP -= amount;
        healthSlider.value = currentHP / maxHP;

        if (currentHP <= 0)
            Die(cause);
    }

    public void Die(PlayerController cause) 
    {
        // Later on, tell GameManager this Player X killed
        // Player Y or something...
        Destroy(transform.parent.gameObject);
        // if die from stage hazard
        if(cause != null)
        {
            ScoreboardManagerScript.instance.updateScores(cause.tankID);
            GameObject deathMessages = GameObject.Find("deathMessage");
            if (deathMessages != null)
            {
                deathMessages.GetComponent<deathMessages>().setMessage(cause.tankID, this.GetComponentInParent<PlayerController>().tankID);
            }
        }
    }
}
