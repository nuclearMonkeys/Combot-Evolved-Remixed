using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public Sprite bulletSprite;
    public Sprite gunSprite;
    public Sprite passiveSprite;

    public Sprite heartSprite;
    public Sprite staminaSprite;

    public GunBase gunPrefab;
    public BulletBase bulletPrefab;
    public PassiveBase passivePrefab;

    public List<GameObject> guns;
    public List<GameObject> bullets;
    public List<GameObject> passives;

    public bool debug = false;
    public bool hasHealth = false;
    public bool hasStamina = false;

    public GameObject pickupPrefab;
    public float currentHP;
    public float maxHP;

    string message = "";
    public List<SpriteRenderer> regainSprites;
    public List<SpriteRenderer> equipmentSprites;

    void Start()
    {
        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        Sprite equipmentSprite;
        Sprite regainSprite;

        foreach(SpriteRenderer child in children)
        {
            if(child.gameObject.name.StartsWith("RegainSprite"))
            {
                regainSprites.Add(child.GetComponent<SpriteRenderer>());
            }
            else if(child.gameObject.name.StartsWith("EquipmentSprite")) 
            {
                equipmentSprites.Add(child.GetComponent<SpriteRenderer>());
            }
        }

        maxHP = Random.Range(1, 7);
        currentHP = maxHP;

        if(Random.value < .5f)
        {
            regainSprite = heartSprite;
            hasHealth = true;
        }
        else 
        {
            regainSprite = staminaSprite;
            hasStamina = true;
        }

        if(Random.value < .33f) 
        {
            gunPrefab = guns[(int)(Random.value * guns.Count)].GetComponent<GunBase>();
            equipmentSprite = gunSprite;
            message = gunPrefab.name.ToUpper();
        }
        else if (Random.value < .66f) 
        {
            bulletPrefab = bullets[(int)(Random.value * bullets.Count)].GetComponent<BulletBase>();
            equipmentSprite = bulletSprite;
            message = bulletPrefab.name.ToUpper();
        }
        else
        {
            passivePrefab = passives[(int)(Random.value * passives.Count)].GetComponent<PassiveBase>();
            equipmentSprite = passiveSprite;
            message = passivePrefab.name.ToUpper();
        }

        foreach(SpriteRenderer rs in regainSprites)
            rs.sprite = regainSprite;

        foreach(SpriteRenderer es in equipmentSprites)
            es.sprite = equipmentSprite;

    }

    void Update() 
    {
        if (currentHP <= 0)
            Break();
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        AudioManager.instance.PlaySound("crack_glass");
    }

    void Break() 
    {
        ItemPickup itemPickup = Instantiate(pickupPrefab, this.transform.position, Quaternion.identity).GetComponent<ItemPickup>();
        itemPickup.gunPrefab = gunPrefab;
        itemPickup.bulletPrefab = bulletPrefab;
        itemPickup.passivePrefab = passivePrefab;
        itemPickup.message = message;
        Destroy(this.gameObject);
    }
}
