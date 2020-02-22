using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPassive : PassiveBase
{
    public GameObject firepoint;
    public PlayerController owner;
    [SerializeField] private GameObject bullet;

    public string soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        owner = GetComponentInParent<PlayerController>();
        firepoint = GameObject.Find("Barrel/FirePoint");
    }

    public override void ActivatePassive(PlayerController pc) 
    {
        if (!bullet)
            FireBullet(pc.gameObject.GetComponent<PlayerWeapons>().bulletPrefab);
        else if (bullet)
            Teleport();
    }

    public void FireBullet(BulletBase bulletPrefab) 
    {
        // in case there's a pre-existing bullet
        if (bullet)
            return;
        
        // I'm so sorry
        BulletBase clone = Instantiate(bulletPrefab.gameObject, firepoint.transform.position, transform.rotation).GetComponent<BulletBase>();
        GameObject barrel = GameObject.Find("Barrel");

        bullet = clone.gameObject;

        clone.damage = 0;
        clone.source = owner;
        clone.SetDirection( (clone.gameObject.transform.position - barrel.transform.position).normalized );
        cooldown = 0;
    }

    public void Teleport() 
    {
        if (!bullet)
            return;
        owner.transform.position = bullet.transform.position;
        Destroy(bullet);
    }
}
