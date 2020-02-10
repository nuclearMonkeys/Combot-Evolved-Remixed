using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckCannon : GunBase
{
    bool isSucking;
    float suckingDuration = 3;
    int maxAmmo = 5;
    [SerializeField] public Stack<GameObject> ammos;
    public GameObject suctionSprite;

    private void Start()
    {
        ammos = new Stack<GameObject>();
        suctionSprite.SetActive(false);
    }

    public override void FireBullet(BulletBase bulletPrefab, PlayerController source)
    {
        if (isSucking)
            return;
        // if no ammo, suck
        if (ammos.Count == 0)
        {
            fireStaminaUsage = 5;
            StartCoroutine(Suck());
        }
        // otherwise shoot
        else
        {
            fireStaminaUsage = 1;
            BulletBase ammo = ammos.Pop().GetComponent<BulletBase>();
            ammo.gameObject.SetActive(true);
            // adapt offset based on ammo size
            ammo.transform.position = firePoint.transform.position + transform.right * ammo.transform.localScale.x;
            ammo.SetDirection(transform.right);
            ammo.source = GetComponentInParent<PlayerController>();
            if(ammos.Count == 0)
                StartCoroutine(DelayedStaminaIncrease());
        }
    }

    IEnumerator DelayedStaminaIncrease()
    {
        yield return null;
        fireStaminaUsage = 5;
    }

    IEnumerator Suck()
    {
        isSucking = true;
        suctionSprite.SetActive(true);
        yield return new WaitForSeconds(suckingDuration);
        suctionSprite.SetActive(false);
        isSucking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isSucking && ammos.Count < maxAmmo)
        {
            // prevent sucking twice if bullet has multiple colliders
            if (ammos.Contains(collision.gameObject))
            {
                Debug.Log("Ammos already contains " + collision.name);
                Debug.Break();
            }
            // if sucking bullet
            if(collision.CompareTag("Bullet"))
            {
                collision.gameObject.SetActive(false);
                ammos.Push(collision.gameObject);
                fireStaminaUsage = 1;
            }
            // if sucking a TNT
            else if(collision.CompareTag("TNT"))
            {
                // get the type of the current bullet
                BulletBase currentBullet = GetComponentInParent<PlayerWeapons>().bulletPrefab.GetComponent<BulletBase>();
                // attach bullet script to tnt
                BulletBase tntBullet = collision.gameObject.AddComponent(currentBullet.GetType()) as BulletBase;
                // set speed and source
                tntBullet.damage = 0;
                tntBullet.speed = currentBullet.speed;
                tntBullet.source = GetComponentInParent<PlayerController>();
                collision.gameObject.SetActive(false);
                ammos.Push(collision.gameObject);
                fireStaminaUsage = 1;
            }
        }
    }
}
