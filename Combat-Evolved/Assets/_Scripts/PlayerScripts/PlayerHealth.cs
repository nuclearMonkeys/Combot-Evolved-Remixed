using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public float currentHP;
    public float maxHP;
    public GameObject smokeParticlePrefab;
    PlayerController pc;
    int lastSmokeSpawn;

    void Start() 
    {
        pc = GetComponentInParent<PlayerController>();
        currentHP = maxHP;
        lastSmokeSpawn = -1;
    }

    // Taking any source of damage
    public void TakeDamage(float amount, Vector2 hitPosition, PlayerController cause = null)
    {
        if (SceneManager.GetActiveScene().name.Equals("Lobby"))
            return;
        currentHP -= amount;
        healthSlider.value = currentHP / maxHP;

        if((int) currentHP != lastSmokeSpawn)
        {
            lastSmokeSpawn = (int)currentHP;
            GameObject smoke = Instantiate(smokeParticlePrefab,
                        transform);
            smoke.transform.position = hitPosition;
        }

        if (currentHP <= 0)
            Die(cause);
    }

    public float getHealthPercentage()
    {
        return currentHP / maxHP;
    }

    // Refill health
    public void ResetHealth()
    {
        currentHP = maxHP;
        healthSlider.value = currentHP / maxHP;

        // Removes smoke from the Player
        foreach (Transform t in transform)
            if (t.gameObject.CompareTag("Cosmetics"))
                Destroy(t.gameObject);
    }

    public void healPlayer(float amountToHeal = -1f)
    {
        if (amountToHeal == -1f || amountToHeal > 1f)
        {
            amountToHeal = 1f;
        }
        if (maxHP * amountToHeal > currentHP)
        {
            currentHP = maxHP * amountToHeal;
        }
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        healthSlider.value = currentHP / maxHP;

    }

    public void Die(PlayerController cause) 
    {
        // spawns a placeholder for camera to track
        StartCoroutine(DelayedCameraRemoval());
        // deactivate player
        transform.parent.gameObject.SetActive(false);
        // decrement players alive
        sceneManager.Instance.currentLiving--;
        sceneManager.Instance.nextScene();
        // if died from other player
        if (cause != null)
        {
            ScoreboardManagerScript.instance.updateScores(cause.tankID, cause.tankID == pc.tankID);
            deathMessages.instance.setMessage(cause.tankID, pc.tankID);
        }
        // if died from stage hazard
        else
        {
            int tankID = pc.tankID;
            deathMessages.instance.setMessage(tankID, tankID);
        }
    }

    // Waits 2 seconds to be untracked from the camera
    IEnumerator DelayedCameraRemoval()
    {
        yield return new WaitForSeconds(2);
        CameraController.instance.targets.Remove(transform.parent);
    }
}
