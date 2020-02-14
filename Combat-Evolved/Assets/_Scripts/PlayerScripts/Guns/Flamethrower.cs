using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : GunBase
{
    public GameObject flameArc;
    public float flameDamage = 2;
    public float fireTime = 3;
    bool gasing = false;
    public bool hittingPlayer = false;

    private void Start() 
    {
        flameArc.SetActive(false);
    }

    public override void FireBullet(BulletBase bulletPrefab) 
    {
        if(!gasing)
            StartCoroutine(Gas());
    }

    IEnumerator Gas()
    {
        gasing = true;
        yield return new WaitForSeconds(fireTime);
        gasing = false;
        hittingPlayer = false;
    }

    public void Hit(PlayerHealth playerHealth) 
    {
        if (hittingPlayer)
        {
            print(flameDamage * 0.2f);
            playerHealth.TakeDamage(flameDamage * 0.2f, owner);
        }
    }

    private void Update() 
    {
        if(!gasing) 
        {
            flameArc.SetActive(false);
            return;
        }
        flameArc.SetActive(true);
    }
}
