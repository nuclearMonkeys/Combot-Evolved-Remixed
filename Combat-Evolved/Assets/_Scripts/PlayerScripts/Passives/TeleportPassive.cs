using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPassive : PassiveBase
{
    public GameObject firepoint;
    public PlayerController owner;

    public string soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<PlayerController>();
    }

    public override void ActivatePassive(PlayerController pc) 
    {
        FireBullet(pc.gameObject.GetComponent<PlayerWeapons>().bulletPrefab);
    }

    public void FireBullet(BulletBase bulletPrefab) 
    {
        BulletBase clone = Instantiate(bulletPrefab.gameObject, firepoint.transform.position, transform.rotation).GetComponent<BulletBase>();
        clone.damage = 0;
        clone.source = owner;
    }
}
