using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : GunBase
{
    public GameObject flameArc;
    public float flameDamage = 2;
    public float fireTime = 3;
    bool gasing = false;
    bool hittingPlayer = false;

    private void Start() 
    {
        flameArc.SetActive(false);
    }

    public override void FireBullet(BulletBase bulletPrefab, PlayerController source) 
    {
        if(!gasing)
            StartCoroutine(Gas());
    }

    IEnumerator Gas()
    {
        gasing = true;
        yield return new WaitForSeconds(fireTime);
        gasing = false;
    }

    public void Hit(PlayerController playerController) 
    {
        if (hittingPlayer)
            playerController.GetComponent<PlayerHealth>().TakeDamage(flameDamage * Time.deltaTime);
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
