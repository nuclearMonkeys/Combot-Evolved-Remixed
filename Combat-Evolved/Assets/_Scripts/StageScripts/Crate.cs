using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public bool isGunCrate;
    public bool isBulletCrate;
    public bool isPassiveCrate;

    public GunBase gunPrefab;
    public BulletBase bulletPrefab;
    public PassiveBase passivePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerWeapons pw = collision.GetComponentInParent<PlayerWeapons>();
            if (isGunCrate)
                pw.AssignGun(gunPrefab);
            else if (isBulletCrate)
                pw.AssignBullet(bulletPrefab);
            else if (isPassiveCrate)
                pw.AssignPassive(passivePrefab);
            Destroy(gameObject);
        }
    }
}
