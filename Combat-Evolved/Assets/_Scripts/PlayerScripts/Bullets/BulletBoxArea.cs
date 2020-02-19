using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoxArea : MonoBehaviour
{
    public GameObject bullet;
    public Rigidbody rb;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Crate")) 
        {
            other.GetComponent<CrateRotate>().HitCrate();
            Destroy(bullet);
            Destroy(this.gameObject);
        }
    }
}
