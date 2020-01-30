using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    private PlayerController myPC;
    // Bullet Prefab
    public BulletBase bulletPrefab;
    // Reference to the Gun Object on the player
    public GunBase gunReference;

    private void Start()
    {
        myPC = GetComponent<PlayerController>();
    }

    // When Getting Bullet Crate, call this
    public void AssignBullet(BulletBase newBullet)
    {
        bulletPrefab = newBullet;
    }

    // When Getting Gun Crate, call this
    public void AssignGun(GunBase newGunPrefab)
    {
        // Instantiate new Gun to replace old Gun
        Transform headTransform = gunReference.transform.parent;        
        Destroy(gunReference.gameObject);
        gunReference = Instantiate(newGunPrefab, headTransform);
        gunReference.transform.localPosition = Vector2.zero;
        // Updates the color of the new gun
        foreach (SpriteRenderer sr in gunReference.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = myPC.tankColor;
        }
    }
}
