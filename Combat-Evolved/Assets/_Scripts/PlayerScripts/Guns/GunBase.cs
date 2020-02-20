using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    // specifying stats
    public float fireRate = .5f;
    public int fireStaminaUsage = 1;
    // how much to modify bullet damage
    public float damageModifier;
    // how many bullets to fire per shot
    public int bulletsPerShot;
    // if multiple bullets, how far to spread
    public float spreadInAngles;
    // where to fire the bullet from
    public GameObject firePoint;
    // gun owner
    public PlayerController owner;
    // sound to play when firing
    public string soundEffect;

    private void Start()
    {
        owner = GetComponentInParent<PlayerController>();
        ExtendedStart();
    }

    public virtual void ExtendedStart() { }

    public void FireBullet(BulletBase bulletPrefab)
    {
        // Plays sound effect
        AudioManager.instance.PlaySound(soundEffect, gameObject);
        ExtendedFireBullet(bulletPrefab);
    }

    public virtual void ExtendedFireBullet(BulletBase bulletPrefab)
    {
        // One bullet guaranteed to travel straight
        BulletBase clone = Instantiate(bulletPrefab.gameObject, firePoint.transform.position, transform.rotation).GetComponent<BulletBase>();
        clone.damage *= damageModifier;
        clone.source = owner;

        // Addditional bullets have random spread
        for (int i = 0; i < bulletsPerShot - 1; i++)
        {
            Quaternion offsetRotation = Quaternion.Euler(new Vector3(0, 0, Random.value * spreadInAngles - spreadInAngles / 2));
            clone = Instantiate(bulletPrefab.gameObject, firePoint.transform.position, transform.rotation * offsetRotation).GetComponent<BulletBase>();
            clone.damage *= damageModifier;
            clone.source = owner;
        }
    }
}
