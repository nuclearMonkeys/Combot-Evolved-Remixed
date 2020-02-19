using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingArea : MonoBehaviour
{
    [HideInInspector] public HomingBullet homingBullet;
    public float updateSpeedSecs = 0.30f;
    public Rigidbody2D rb;

    private void Start() 
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController && homingBullet.source != playerController) 
        {
            StartCoroutine(SmoothSetDirection(other.gameObject));
        }
    }

    private IEnumerator SmoothSetDirection(GameObject other) 
    {
        Vector3 newDirection = (other.gameObject.transform.position - 
            homingBullet.gameObject.transform.position).normalized;

        float elapsed = 0f;

        while (elapsed < updateSpeedSecs) 
        {
            elapsed += Time.deltaTime;
            Vector3 transitionDirection = Vector3.Slerp(
                rb.velocity, 
                (other.gameObject.transform.position - 
                    homingBullet.gameObject.transform.position).normalized,
                elapsed / updateSpeedSecs);
            homingBullet.SetDirection(transitionDirection);
            yield return new WaitForEndOfFrame();
        }
        // yield return null;

        homingBullet.SetDirection(newDirection);
    }
}
