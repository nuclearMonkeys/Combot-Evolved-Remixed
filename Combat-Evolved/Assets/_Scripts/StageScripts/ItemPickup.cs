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

    public ScrollingText scrollingTextPrefab;
    string message = "";

    void Start() 
    {
        if(debug)
            return;
        
        if(Random.value < .33f) 
        {
            gunPrefab = guns[(int)(Random.value * guns.Count)].GetComponent<GunBase>();
            message = gunPrefab.name;
        }
        else if (Random.value < .66f) 
        {
            bulletPrefab = bullets[(int)(Random.value * bullets.Count)].GetComponent<BulletBase>();
            message = bulletPrefab.name;
        }
        else
        {
            passivePrefab = passives[(int)(Random.value * passives.Count)].GetComponent<PassiveBase>();
            message = passivePrefab.name;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerWeapons pw = collision.GetComponentInParent<PlayerWeapons>();
            while (true)
            {
                if (gunPrefab && pw.getGunBase().name.Contains(gunPrefab.name) == false)
                {
                    pw.AssignGun(gunPrefab);
                    break;
                }
                    
                else if (bulletPrefab && pw.getBulletBase().name.Contains(bulletPrefab.name) == false)
                {
                    pw.AssignBullet(bulletPrefab);
                    break;
                }
                else if (passivePrefab && pw.getPassiveBase().name.Contains(passivePrefab.name) == false)
                {
                    pw.AssignPassive(passivePrefab);
                    break;
                }
                Start();
            }
            Destroy(gameObject);
            Instantiate(scrollingTextPrefab, collision.transform.parent).SetText(message);
            float percent = collision.GetComponent<PlayerHealth>().getHealthPercentage();

            
        }
    }
}
