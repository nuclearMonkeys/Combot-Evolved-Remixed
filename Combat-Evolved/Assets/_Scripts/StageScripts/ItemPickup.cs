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
    string message = "";

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
