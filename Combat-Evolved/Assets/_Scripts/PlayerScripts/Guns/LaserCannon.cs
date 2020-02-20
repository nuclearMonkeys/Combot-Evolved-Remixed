using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : GunBase
{
    LineRenderer lr;
    // damage per second
    public float laserDamage = 1;
    public float endDamageMultiplier = 4;
    public float zappingTime = 3;
    bool zapping = false;

    public override void ExtendedStart()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    public override void FireBullet(BulletBase bulletPrefab)
    {
        if(!zapping)
        {
            StartCoroutine(Zap());
        }
    }

    IEnumerator Zap()
    {
        zapping = true;
        // yield return new WaitForSecondsRealtime(2f);
        yield return new WaitForSeconds(zappingTime);
        zapping = false;
    }

    IEnumerator FireEndLaser() 
    {
        lr.startWidth = 0.1f;
        yield return new WaitForSeconds(zappingTime/2);

        float referenceDamage = laserDamage;
        laserDamage = laserDamage * endDamageMultiplier;
        lr.startWidth = 1f;
        

        yield return new WaitForSeconds(zappingTime/2);
        laserDamage = referenceDamage;
    }

    private void Update()
    {
        if(zapping)
        {
            if(!lr.enabled)
                StartCoroutine(FireEndLaser());

            
            lr.enabled = true;
            lr.SetPosition(0, firePoint.transform.position);
            RaycastHit2D playerhit = Physics2D.Raycast(firePoint.transform.position, transform.right, 100000, 1 << LayerManager.TANKBODY);
            if (playerhit)
            {
                lr.SetPosition(1, playerhit.point);
                playerhit.collider.GetComponent<PlayerHealth>().TakeDamage(laserDamage * Time.deltaTime, owner);
            }
            else
            {
                RaycastHit2D blockhit = Physics2D.Raycast(firePoint.transform.position, transform.right, 100000, 1 << LayerManager.BLOCK | 1 << LayerManager.STAGEHAZARD);
                if (blockhit)
                {
                    lr.SetPosition(1, blockhit.point);
                }
            }
        }
        else
        {
            lr.enabled = false;
        }
    }
}
