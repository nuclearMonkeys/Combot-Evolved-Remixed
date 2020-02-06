using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckCannon : GunBase
{
    bool isSucking;
    float suckingDuration = 3;
    int maxAmmo = 5;
    Stack<GameObject> ammos;
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
            ammo.transform.position = firePoint.transform.position;
            ammo.SetDirection(transform.right);
            ammo.source = GetComponentInParent<PlayerController>();
        }
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
        if(collision.CompareTag("Bullet") && isSucking && ammos.Count < maxAmmo)
        {
            collision.gameObject.SetActive(false);
            ammos.Push(collision.gameObject);
        }
    }
}
