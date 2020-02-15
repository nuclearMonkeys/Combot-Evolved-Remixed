using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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
        if (SceneManager.GetActiveScene().name.Equals("Lobby"))
            return;
        currentHP -= amount;
        healthSlider.value = currentHP / maxHP;

        if (currentHP <= 0)
            Die(cause);
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
        healthSlider.value = currentHP / maxHP;
    }

    public void Die(PlayerController cause) 
    {
        // spawns a placeholder for camera to track
        StartCoroutine(DelayedCameraRemoval(transform.position));
        // move away from playable
        transform.parent.position = new Vector3(1000, 1000);
        CameraController.instance.targets.Remove(transform.parent);
        // decrement players alive
        sceneManager.Instance.currentLiving--;
        sceneManager.Instance.nextScene();
        // if die from stage hazard
        if (cause != null)
        {
            ScoreboardManagerScript.instance.updateScores(cause.tankID);
            GameObject deathMessages = GameObject.Find("deathMessage");
            if (deathMessages != null)
            {
                deathMessages.GetComponent<deathMessages>().setMessage(cause.tankID, this.GetComponentInParent<PlayerController>().tankID);
            }
        }
        else
        {
            GameObject deathMessages = GameObject.Find("deathMessage");
            int tankID = this.GetComponentInParent<PlayerController>().tankID;
            if (deathMessages != null)
            {
                deathMessages.GetComponent<deathMessages>().setMessage(tankID, tankID);
            }
        }
    }


    IEnumerator DelayedCameraRemoval(Vector2 deathPosition)
    {
        GameObject placeHolder = new GameObject();
        placeHolder.transform.position = deathPosition;
        CameraController.instance.targets.Add(placeHolder.transform);
        yield return new WaitForSeconds(2);
        CameraController.instance.targets.Remove(placeHolder.transform);
        Destroy(placeHolder);
    }
}
