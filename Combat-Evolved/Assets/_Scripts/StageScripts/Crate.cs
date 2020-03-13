using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GunBase gunPrefab;
    public BulletBase bulletPrefab;
    public PassiveBase passivePrefab;

    public List<GameObject> guns;
    public List<GameObject> bullets;
    public List<GameObject> passives;

    public bool hasHealth = false;
    public bool hasStamina = false;

    public GameObject pickupPrefab;
    public float currentHP;
    public float maxHP;

    void Start()
    {
        maxHP = Random.Range(1, 7);
        currentHP = maxHP;

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

        if(Random.value < .5f) 
            hasHealth = true;
        else
            hasStamina = true;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
            Break();
    }

    void Break() 
    {
        ItemPickup itemPickup = Instantiate(pickupPrefab, this.transform.position, Quaternion.identity).GetComponent<ItemPickup>();
        itemPickup.gunPrefab = gunPrefab;
        itemPickup.bulletPrefab = bulletPrefab;
        itemPickup.passivePrefab = passivePrefab;
        Destroy(this.gameObject);
    }
}
