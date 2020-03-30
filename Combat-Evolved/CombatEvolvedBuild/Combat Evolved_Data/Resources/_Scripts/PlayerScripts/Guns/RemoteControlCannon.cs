using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControlCannon : GunBase
{
    BulletBase bullet;

    public override void ExtendedFireBullet(BulletBase bulletPrefab) 
    {
        if(bullet)
            return;
        
        BulletBase clone = Instantiate(bulletPrefab.gameObject, firePoint.transform.position, transform.rotation).GetComponent<BulletBase>();
        clone.damage *= damageModifier;
        clone.source = owner;
        bullet = clone;
    }
    
    private void Update() 
    {
        if (bullet) {
            bullet.SetDirection( owner.gunDirection );
        }
    }
}
