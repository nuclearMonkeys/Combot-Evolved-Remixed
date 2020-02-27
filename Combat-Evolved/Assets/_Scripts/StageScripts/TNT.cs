using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TNT : MonoBehaviour
{
    public float explosionRadius;
    public float explosionDamage;
    public GameObject explosionPrefab;

    public void Explode(PlayerController cause)
    {
        CameraController.instance.ShakeCamera();
        // create explosion object
        HazardDamage explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<HazardDamage>();
        // set the damage and size of explosion
        explosion.damage = explosionDamage;
        explosion.cause = cause;
        explosion.transform.localScale = new Vector3(explosionRadius, explosionRadius, 1);
        // destroy the TNT crate
        Destroy(gameObject);
        Destroy(explosion.gameObject, .5f);
    }

    public IEnumerator DelayedExplode(PlayerController cause)
    {
        yield return new WaitForSeconds(.1f);
        Explode(cause);
    }
}