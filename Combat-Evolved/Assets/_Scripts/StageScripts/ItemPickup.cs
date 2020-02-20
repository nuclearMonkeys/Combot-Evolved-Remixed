using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public bool debug;

    public GunBase gunPrefab;
    public BulletBase bulletPrefab;
    public PassiveBase passivePrefab;

    public List<GameObject> guns;
    public List<GameObject> bullets;
    public List<GameObject> passives;

    void Start() 
    {
        if(debug)
            return;
        
        if(Random.value < .33f) 
        {
            gunPrefab = guns[(int)(Random.value * guns.Count)].GetComponent<GunBase>();
        }
        else if (Random.value < .66f) 
        {
            bulletPrefab = bullets[(int)(Random.value * bullets.Count)].GetComponent<BulletBase>();
        }
        else
        {
            passivePrefab = passives[(int)(Random.value * passives.Count)].GetComponent<PassiveBase>();
        }
        gunPrefab = guns[4].GetComponent<GunBase>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerWeapons pw = collision.GetComponentInParent<PlayerWeapons>();
            if (gunPrefab)
                pw.AssignGun(gunPrefab);
            else if (bulletPrefab)
                pw.AssignBullet(bulletPrefab);
            else if (passivePrefab)
                pw.AssignPassive(passivePrefab);
            Destroy(gameObject);
        }
    }
}
