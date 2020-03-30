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

    public bool hasHealth = false;
    public bool hasStamina = false;

    public ScrollingText scrollingTextPrefab;
    public string message = "";

    void Start() 
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerWeapons pw = collision.GetComponentInParent<PlayerWeapons>();
            while (true)
            {
                if(hasHealth)
                    pw.GetComponentInChildren<PlayerHealth>().healPlayer(2);
                else if(hasStamina)
                    pw.GetComponentInChildren<PlayerStamina>().ResetStamina();

                if (gunPrefab)
                {
                    if (pw.getGunBase().name.Contains(gunPrefab.name) == true) 
                    {
                        guns.Remove(gunPrefab.gameObject);
                        gunPrefab = guns[(int)(Random.value * guns.Count)].GetComponent<GunBase>();
                        message = gunPrefab.name.ToUpper();
                    }
                    pw.AssignGun(gunPrefab);
                    break;
                }
                    
                else if (bulletPrefab)
                {
                    if (pw.getBulletBase().name.Contains(bulletPrefab.name) == true) 
                    {
                        bullets.Remove(bulletPrefab.gameObject);
                        bulletPrefab = bullets[(int)(Random.value * bullets.Count)].GetComponent<BulletBase>();
                        message = bulletPrefab.name.ToUpper();
                    }
                    pw.AssignBullet(bulletPrefab);
                    break;
                }
                else if (passivePrefab && pw.getPassiveBase().name.Contains(passivePrefab.name) == false)
                {
                    if (pw.getPassiveBase().name.Contains(passivePrefab.name) == true) 
                    {
                        passives.Remove(passivePrefab.gameObject);
                        passivePrefab = passives[(int)(Random.value * passives.Count)].GetComponent<PassiveBase>();
                        message = passivePrefab.name.ToUpper();
                    }
                    pw.AssignPassive(passivePrefab);
                    break;
                }
                Start();
            }
            AudioManager.instance.PlaySound("Ammo");
            Destroy(gameObject);
            Instantiate(scrollingTextPrefab, collision.transform.parent).SetText(message);
            float percent = collision.GetComponent<PlayerHealth>().getHealthPercentage();

            
        }
    }
}
