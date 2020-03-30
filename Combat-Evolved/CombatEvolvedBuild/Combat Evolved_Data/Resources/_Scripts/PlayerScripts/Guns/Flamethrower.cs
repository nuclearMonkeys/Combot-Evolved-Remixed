using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : GunBase
{
    public ArcCollider flameArc;
    public float flameDamage = 2;
    public float fireTime = 3;
    private bool gasing = false;

    public override void ExtendedStart()
    {
        fadeInSound = true;
    }

    public override void ExtendedFireBullet(BulletBase bulletPrefab) 
    {
        print("Fire");
        if(!gasing)
            StartCoroutine(Gas());
    }

    IEnumerator Gas()
    {
        gasing = true;
        yield return new WaitForSeconds(fireTime);
        gasing = false;
        AudioManager.instance.StopSound(soundEffect, gameObject, true);
    }

    public void Hit(PlayerHealth playerHealth) 
    {
        playerHealth.TakeDamage(flameDamage * 0.2f, playerHealth.transform.position, owner);
    }

    private void Update() 
    {
        if(!gasing) 
        {
            flameArc.flameParticles.Stop();
            return;
        }
        flameArc.flameParticles.Play();
    }
}
