using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public bool isGunCrate;
    public bool isBulletCrate;

    public GunBase gunPrefab;
    public BulletBase bulletPrefab;

    // Just Debugging Tools
    private void Start()
    {
        // if neither or both bools are on
        if((isGunCrate && isBulletCrate) || (!isGunCrate && !isBulletCrate))
        {
            Debug.Log("One Crate Option Should Be Checked!!");
            Debug.Break();
        }
        if (isGunCrate)
        {
            if (gunPrefab == null || bulletPrefab != null)
            {
                Debug.Log("Gun Crate Should Have Gun Prefab Only!!");
                Debug.Break();
            }
        }
        if (isBulletCrate)
        {
            if (gunPrefab != null || bulletPrefab == null)
            {
                Debug.Log("Bullet Crate Should Have Bullet Prefab Only!!");
                Debug.Break();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerWeapons pw = collision.GetComponentInParent<PlayerWeapons>();
            if (isGunCrate)
                pw.AssignGun(gunPrefab);
            else if (isBulletCrate)
                pw.AssignBullet(bulletPrefab);
            Destroy(gameObject);
        }
    }
}
