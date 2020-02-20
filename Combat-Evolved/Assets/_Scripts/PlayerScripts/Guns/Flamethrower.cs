using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : GunBase
{
    public ArcCollider flameArc;
    public float flameDamage = 2;
    public float fireTime = 3;
    private bool gasing = false;

    private void Start() 
    {
        owner = this.GetComponentInParent<PlayerController>();
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
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource) Destroy(audioSource);
    }

    public void Hit(PlayerHealth playerHealth) 
    {
        playerHealth.TakeDamage(flameDamage * 0.2f, owner);
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
